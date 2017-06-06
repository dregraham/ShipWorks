using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using log4net;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Common;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval;
using ShipWorks.Stores;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps
{
    /// <summary>
    /// Get the label for the shipment
    /// </summary>
    [Component(RegistrationType.Self)]
    public class LabelRetrievalStep
    {
        private readonly IStoreTypeManager storeTypeManager;
        private readonly ILabelServiceFactory labelServiceFactory;
        private readonly ILog log;
        private readonly IOrderedCompositeManipulator<ILabelRetrievalShipmentManipulator, ShipmentEntity> shipmentManipulator;
        private readonly ICompositeValidator<ILabelRetrievalShipmentValidator, ShipmentEntity> shipmentValidator;

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelRetrievalStep(
            IStoreTypeManager storeTypeManager,
            ILabelServiceFactory labelServiceFactory,
            IOrderedCompositeManipulator<ILabelRetrievalShipmentManipulator, ShipmentEntity> shipmentManipulator,
            ICompositeValidator<ILabelRetrievalShipmentValidator, ShipmentEntity> shipmentValidator,
            Func<Type, ILog> createLogger)
        {
            this.shipmentValidator = shipmentValidator;
            this.shipmentManipulator = shipmentManipulator;
            this.labelServiceFactory = labelServiceFactory;
            this.storeTypeManager = storeTypeManager;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Get a label for a shipment
        /// </summary>
        public LabelRetrievalResult GetLabel(IShipmentPreparationResult result)
        {
            if (!result.Success)
            {
                return new LabelRetrievalResult(result);
            }

            ShippingException lastException = null;

            foreach (ShipmentEntity shipment in result.Shipments)
            {
                try
                {
                    return GetLabelForShipment(result, shipment);
                }
                catch (Exception ex) when (CanHandleException(ex))
                {
                    lastException = ex as ShippingException ?? new ShippingException(ex.Message, ex);
                }
            }

            return new LabelRetrievalResult(result, lastException);
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
                ex is ShippingException;
        }

        /// <summary>
        /// Try to get a label for the specific shipment entity
        /// </summary>
        private LabelRetrievalResult GetLabelForShipment(IShipmentPreparationResult result, ShipmentEntity shipment)
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

            IDownloadedLabelData labelData = labelService.Create(modifiedShipment);

            return new LabelRetrievalResult(result, labelData, modifiedShipment, clone, fieldsToRestore);
        }
    }
}
