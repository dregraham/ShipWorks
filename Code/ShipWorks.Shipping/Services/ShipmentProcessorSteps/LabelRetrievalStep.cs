﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Common;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval;
using ShipWorks.Stores;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps
{
    /// <summary>
    /// Get the label for the shipment
    /// </summary>
    [NamedComponent("LabelRetrievalStep", typeof(ILabelRetrievalStep))]
    public class LabelRetrievalStep : ILabelRetrievalStep
    {
        private readonly IStoreTypeManager storeTypeManager;
        private readonly ILabelServiceFactory labelServiceFactory;
        private readonly ILog log;
        private readonly IOrderedCompositeManipulator<ILabelRetrievalShipmentManipulator, ShipmentEntity> shipmentManipulator;
        private readonly ICompositeValidator<ILabelRetrievalShipmentValidator, ShipmentEntity> shipmentValidator;
        private readonly IAutoReturnShipmentService autoReturn;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParamsAttribute]
        public LabelRetrievalStep(
            IStoreTypeManager storeTypeManager,
            ILabelServiceFactory labelServiceFactory,
            IOrderedCompositeManipulator<ILabelRetrievalShipmentManipulator, ShipmentEntity> shipmentManipulator,
            ICompositeValidator<ILabelRetrievalShipmentValidator, ShipmentEntity> shipmentValidator,
            IAutoReturnShipmentService autoReturn,
            Func<Type, ILog> createLogger)
        {
            this.shipmentValidator = shipmentValidator;
            this.shipmentManipulator = shipmentManipulator;
            this.labelServiceFactory = labelServiceFactory;
            this.storeTypeManager = storeTypeManager;
            this.autoReturn = autoReturn;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Get a label for a shipment
        /// </summary>
        public async Task<IEnumerable<ILabelRetrievalResult>> GetLabels(IShipmentPreparationResult shipmentPreparationResult)
        {
            List<ILabelRetrievalResult> labelRetrievalResults = new List<ILabelRetrievalResult>();
            if (!shipmentPreparationResult.Success)
            {
                labelRetrievalResults.Add(new LabelRetrievalResult(shipmentPreparationResult));
                return labelRetrievalResults;
            }

            ShippingException lastShipmentException = null;

            foreach (ShipmentEntity shipment in shipmentPreparationResult.Shipments)
            {
                ILabelRetrievalResult shipmentResult = null;
                ILabelRetrievalResult returnShipmentResult = null;

                try
                {
                    shipmentResult = await GetLabelForShipment(shipmentPreparationResult, shipment).ConfigureAwait(false);
                }
                catch (Exception ex) when (CanHandleException(ex))
                {
                    Exception handeledException = ex;

                    if (ex is AggregateException)
                    {
                        handeledException = ex.GetBaseException();
                    }

                    lastShipmentException = handeledException as ShippingException ?? new ShippingException(handeledException.Message, ex);
                }

                if (shipmentResult != null && shipmentResult.Success)
                {
                    labelRetrievalResults.Add(shipmentResult);

                    if (shipment.IncludeReturn)
                    {
                        returnShipmentResult = await ProcessAutomaticReturn(shipment, shipmentPreparationResult).ConfigureAwait(false);
                        labelRetrievalResults.Add(returnShipmentResult);
                    }

                    return labelRetrievalResults;
                }
            }

            // Failed to retrieve any shipment labels
            labelRetrievalResults.Add(new LabelRetrievalResult(shipmentPreparationResult, lastShipmentException));
            return labelRetrievalResults;
        }

        /// <summary>
        /// Create a return shipment and retreive a label for it
        /// </summary>
        private async Task<ILabelRetrievalResult> ProcessAutomaticReturn(ShipmentEntity shipment, IShipmentPreparationResult result)
        {
            ShipmentEntity returnShipment = autoReturn.CreateReturn(shipment);

            if (autoReturn.ReturnException != null)
            {
                return new LabelRetrievalResult(result, returnShipment, autoReturn.ReturnException);
            }

            try
            {
                return await GetLabelForShipment(result, returnShipment).ConfigureAwait(false);
            }
            catch (Exception ex) when (CanHandleException(ex))
            {
                ShippingException returnException = ex as ShippingException ?? new ShippingException(ex.Message, ex);
                return new LabelRetrievalResult(result, returnShipment, returnException);
            }
        }

        /// <summary>
        /// Can the exception be handled
        /// </summary>
        private bool CanHandleException(Exception ex)
        {
            bool canHandle = ex is InsureShipException ||
                ex is ShipWorksLicenseException ||
                ex is TangoException ||
                ex is TemplateTokenException ||
                ex is ShippingException ||
                ex is CarrierException;

            if (!canHandle)
            {
                return CanHandleException(ex.GetBaseException());
            }

            return canHandle;
        }
        
        /// <summary>
        /// Try to get a label for the specific shipment entity
        /// </summary>
        private async Task<LabelRetrievalResult> GetLabelForShipment(IShipmentPreparationResult result, ShipmentEntity shipment)
        {
            ShipmentEntity modifiedShipment = shipmentManipulator.Apply(shipment);

            // Validate the shipment before attempting to process the shipment
            ICompositeValidatorResult validation = shipmentValidator.Apply(modifiedShipment);
            if (validation.Failure)
            {
                throw new ShippingException(validation.Errors.First());
            }

            // We're going to allow the store to confirm the shipping address for the shipping label, but we want to
            // make a note of the original shipping address first, so we can reset the address back after the label
            // has been generated. This will result in the customer still being able to see where the package went
            // according to the original cart order
            ShipmentEntity clone = EntityUtility.CloneEntity(modifiedShipment);

            // Instantiate the store class to allow it a chance to confirm the shipping address before
            // the shipping label is created. We don't use the method on the ShippingManager to do this
            // since we want to track the fields that changed.
            StoreType storeType = storeTypeManager.GetType(result.Store);
            List<ShipmentFieldIndex> fieldsToRestore = storeType.OverrideShipmentDetails(modifiedShipment);

            log.InfoFormat("Shipment {0}  - ShipmentType.Process Start", modifiedShipment.ShipmentID);

            ILabelService labelService = labelServiceFactory.Create(modifiedShipment.ShipmentTypeCode);
            Debug.Assert(Transaction.Current == null, "No transaction should exist at this point.");

            TelemetricResult<IDownloadedLabelData> labelData = await labelService.Create(modifiedShipment).ConfigureAwait(false);

            return new LabelRetrievalResult(result, labelData, modifiedShipment, clone, fieldsToRestore);
        }
    }
}
