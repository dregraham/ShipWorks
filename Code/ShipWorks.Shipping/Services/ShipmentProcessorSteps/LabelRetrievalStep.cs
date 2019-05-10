using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
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
        public async Task<Tuple<ILabelRetrievalResult, ILabelRetrievalResult>> GetLabels(IShipmentPreparationResult result)
        {
            if (!result.Success)
            {
                return new Tuple<ILabelRetrievalResult, ILabelRetrievalResult>(new LabelRetrievalResult(result), null);
            }

            ShippingException lastShipmentException = null;

            foreach (ShipmentEntity shipment in result.Shipments)
            {
                ILabelRetrievalResult shipmentResult = null;
                ILabelRetrievalResult returnShipmentResult = null;

                try
                {
                    shipmentResult = await GetLabelForShipment(result, shipment).ConfigureAwait(false);
                }
                catch (Exception ex) when (CanHandleException(ex))
                {
                    lastShipmentException = ex as ShippingException ?? new ShippingException(ex.Message, ex);
                }

                if (shipment.IncludeReturn && shipmentResult != null && shipmentResult.Success)
                {
                    returnShipmentResult = await ProcessAutomaticReturn(shipment, result).ConfigureAwait(false);

                    return new Tuple<ILabelRetrievalResult, ILabelRetrievalResult>(shipmentResult, returnShipmentResult);
                }
            }

            // Failed to retrieve any shipment
            return new Tuple<ILabelRetrievalResult, ILabelRetrievalResult>(new LabelRetrievalResult(result, lastShipmentException), null);
        }

        /// <summary>
        /// Create a return shipment and retreive a label for it
        /// </summary>
        private async Task<ILabelRetrievalResult> ProcessAutomaticReturn(ShipmentEntity shipment, IShipmentPreparationResult result)
        {
            ShippingException returnException = null;
            ShipmentEntity returnShipment = autoReturn.CreateReturn(shipment);

            if (shipment.ApplyReturnProfile)
            {
                try
                {
                    autoReturn.ApplyReturnProfile(returnShipment, shipment.ReturnProfileID);
                }
                catch (NotFoundException ex)
                {
                    returnException = new ShippingException(ex.Message, ex);
                    return new LabelRetrievalResult(result, returnShipment, returnException);
                }
            }

            try
            {
                return await GetLabelForShipment(result, returnShipment).ConfigureAwait(false);
            }
            catch (Exception ex) when (CanHandleException(ex))
            {
                returnException = ex as ShippingException ?? new ShippingException(ex.Message, ex);
            }

            return new LabelRetrievalResult(result, returnShipment, returnException);
        }

        /// <summary>
        /// Can the exception be handled
        /// </summary>
        private bool CanHandleException(Exception ex)
        {
            return ex is InsureShipException ||
                ex is ShipWorksLicenseException ||
                ex is TangoException ||
                ex is TemplateTokenException ||
                ex is ShippingException ||
                ex is CarrierException;
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
