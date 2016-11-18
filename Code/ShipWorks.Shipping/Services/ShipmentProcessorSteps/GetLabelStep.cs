using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using log4net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Common;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps.GetLabel;
using ShipWorks.Stores;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps
{
    /// <summary>
    /// Get the label for the shipment
    /// </summary>
    [Component(RegistrationType.Self)]
    public class GetLabelStep
    {
        private readonly IStoreTypeManager storeTypeManager;
        private readonly ILabelServiceFactory labelServiceFactory;
        private readonly ILog log;
        private readonly IApplyOrderedManipulators<IGetLabelManipulator, ShipmentEntity> applyManipulators;
        private readonly IApplyValidators<IGetLabelValidator, ShipmentEntity> applyValidators;

        /// <summary>
        /// Constructor
        /// </summary>
        public GetLabelStep(
            IStoreTypeManager storeTypeManager,
            ILabelServiceFactory labelServiceFactory,
            IApplyOrderedManipulators<IGetLabelManipulator, ShipmentEntity> applyManipulators,
            IApplyValidators<IGetLabelValidator, ShipmentEntity> applyValidators,
            Func<Type, ILog> createLogger)
        {
            this.applyValidators = applyValidators;
            this.applyManipulators = applyManipulators;
            this.labelServiceFactory = labelServiceFactory;
            this.storeTypeManager = storeTypeManager;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Get a label for a shipment
        /// </summary>
        public GetLabelResult GetLabel(IPrepareShipmentResult result)
        {
            if (!result.Success)
            {
                return new GetLabelResult(result);
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

            return new GetLabelResult(result, lastException);
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
        private GetLabelResult GetLabelForShipment(IPrepareShipmentResult result, ShipmentEntity shipment)
        {
            ShipmentEntity modifiedShipment = applyManipulators.Apply(shipment);

            // Validate the shipment before attempting to process the shipment
            IApplyValidatorsResult validation = applyValidators.Apply(modifiedShipment);
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

            return new GetLabelResult(result, labelData, modifiedShipment, clone, fieldsToRestore);
        }
    }
}
