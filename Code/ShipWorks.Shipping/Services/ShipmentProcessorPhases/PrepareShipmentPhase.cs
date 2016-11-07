using System;
using System.Collections.Generic;
using log4net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.Services.ShipmentProcessorPhases
{
    /// <summary>
    /// Prepare the shipment for processing
    /// </summary>
    [Component(RegistrationType.Self)]
    public class PrepareShipmentPhase
    {
        private readonly ILog log;
        private readonly ISecurityContext securityContext;
        private readonly IStoreManager storeManager;
        private readonly IShippingManager shippingManager;
        private readonly IShipmentPreProcessorFactory shipmentPreProcessorFactory;
        private readonly IResourceLockFactory resourceLockFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrepareShipmentPhase(IStoreManager storeManager, ISecurityContext securityContext,
            IShippingManager shippingManager,
            IResourceLockFactory resourceLockFactory,
            IShipmentPreProcessorFactory shipmentPreProcessorFactory,
            Func<Type, ILog> getLogger)
        {
            this.resourceLockFactory = resourceLockFactory;
            this.storeManager = storeManager;
            this.securityContext = securityContext;
            this.shippingManager = shippingManager;
            this.shipmentPreProcessorFactory = shipmentPreProcessorFactory;
            log = getLogger(GetType());
        }

        /// <summary>
        /// Action that will execute if a counter rate carrier is configured while processing
        /// </summary>
        public Action CounterRateCarrierConfiguredWhileProcessing { get; set; }

        /// <summary>
        /// Process a single shipment
        /// </summary>
        public PrepareShipmentResult PrepareShipment(ProcessShipmentState state)
        {
            ShipmentEntity shipment = state.OriginalShipment;
            IDictionary<long, Exception> licenseCheckCache = state.LicenseCheckCache;
            RateResult selectedRate = state.ChosenRate;

            var cancelProcessing = false;

            // Processing was canceled by the best rate processing dialog
            if (cancelProcessing)
            {
                return new PrepareShipmentResult(null, state, new ShippingException("Canceled"));
            }

            if (!state.Success)
            {
                return new PrepareShipmentResult(null, state, state.Exception);
            }

            log.InfoFormat("Shipment {0}  - Process Start", shipment.ShipmentID);

            try
            {
                securityContext.DemandPermission(PermissionType.ShipmentsCreateEditProcess, shipment.ShipmentID);
            }
            catch (PermissionException ex)
            {
                return new PrepareShipmentResult(null, state, ex);
            }

            IDisposable entityLock = null;

            // Ensure's we are the only one who processes this shipment, if other ShipWorks are running
            try
            {
                entityLock = resourceLockFactory.GetEntityLock(shipment.ShipmentID, "Process Shipment");
            }
            catch (SqlAppResourceLockException ex)
            {
                log.InfoFormat("Could not obtain lock for processing shipment {0}", shipment.ShipmentID);
                return new PrepareShipmentResult(entityLock, state, new ShippingException("The shipment was being processed on another computer.", ex));
            }

            shippingManager.EnsureShipmentIsLoadedWithOrder(shipment);

            if (shipment.Processed)
            {
                return new PrepareShipmentResult(entityLock, state, new ShipmentAlreadyProcessedException("The shipment has already been processed."));
            }

            if (shipment.Order == null)
            {
                return new PrepareShipmentResult(entityLock, state, new ShippingException("The shipment's order could not be loaded."));
            }

            StoreEntity store = storeManager.GetStore(shipment.Order.StoreID);
            if (store == null)
            {
                return new PrepareShipmentResult(entityLock, state, new ShippingException("The store the shipment was in has been deleted."));
            }

            // Validate that the license is valid
            Exception error = shippingManager.ValidateLicense(store, licenseCheckCache);
            if (error != null)
            {
                return new PrepareShipmentResult(entityLock, state, new ShippingException(error.Message, error));
            }

            // Get the ShipmentType instance
            IShipmentPreProcessor preprocessor = shipmentPreProcessorFactory.Create(shipment.ShipmentTypeCode);
            IEnumerable<ShipmentEntity> shipmentsToTryToProcess = preprocessor.Run(shipment, selectedRate, CounterRateCarrierConfiguredWhileProcessing);

            // A null value returned from the preprocess method means the user has opted to not continue
            // processing after a counter rate was selected as the best rate, so the processing of the shipment should be aborted
            if (shipmentsToTryToProcess == null)
            {
                return new PrepareShipmentResult(entityLock, state, new ShippingException("Processing was canceled"));
            }

            return new PrepareShipmentResult(entityLock, state, shipmentsToTryToProcess, store);
        }
    }
}
