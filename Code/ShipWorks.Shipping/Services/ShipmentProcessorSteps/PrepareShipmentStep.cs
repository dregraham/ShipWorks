using System;
using System.Collections.Generic;
using Interapptive.Shared;
using log4net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Stores;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps
{
    /// <summary>
    /// Prepare the shipment for processing
    /// </summary>
    [Component(RegistrationType.Self)]
    public class PrepareShipmentStep
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
        [NDependIgnoreTooManyParams]
        public PrepareShipmentStep(IStoreManager storeManager, ISecurityContext securityContext,
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

            if (!state.Success)
            {
                return new PrepareShipmentResult(null, state, state.Exception);
            }

            log.InfoFormat("Shipment {0} - Process Start", shipment.ShipmentID);

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
                return new PrepareShipmentResult(null, state, new ShippingException("The shipment was being processed on another computer.", ex));
            }

            shippingManager.EnsureShipmentIsLoadedWithOrder(shipment);
            StoreEntity store = shipment.Order == null ? null : storeManager.GetStore(shipment.Order.StoreID);

            ShippingException exception = ValidateShipmentCanBeProcessed(shipment, store, state.LicenseCheckCache);
            if (exception != null)
            {
                return new PrepareShipmentResult(entityLock, state, exception);
            }

            return RunPreProcessor(entityLock, state, shipment, store);
        }

        /// <summary>
        /// Run the preprocessor
        /// </summary>
        private PrepareShipmentResult RunPreProcessor(IDisposable entityLock, ProcessShipmentState state,
            ShipmentEntity shipment, StoreEntity store)
        {
            IShipmentPreProcessor preprocessor = shipmentPreProcessorFactory.Create(shipment.ShipmentTypeCode);
            IEnumerable<ShipmentEntity> shipmentsToTryToProcess =
                preprocessor.Run(shipment, state.ChosenRate, CounterRateCarrierConfiguredWhileProcessing);

            // A null value returned from the preprocess method means the user has opted to not continue
            // processing after a counter rate was selected as the best rate, so the processing of the shipment should be aborted
            if (shipmentsToTryToProcess == null)
            {
                return new PrepareShipmentResult(entityLock, state, new ShippingException("Processing was canceled"));
            }

            return new PrepareShipmentResult(entityLock, state, shipmentsToTryToProcess, store);
        }

        /// <summary>
        /// Validate that the shipment can be processed
        /// </summary>
        private ShippingException ValidateShipmentCanBeProcessed(ShipmentEntity shipment, StoreEntity store,
            IDictionary<long, Exception> licenseCheckCache)
        {
            if (shipment.Processed)
            {
                return new ShipmentAlreadyProcessedException("The shipment has already been processed.");
            }

            if (shipment.Order == null)
            {
                return new ShippingException("The shipment's order could not be loaded.");
            }

            if (store == null)
            {
                return new ShippingException("The store the shipment was in has been deleted.");
            }

            // Validate that the license is valid
            Exception error = shippingManager.ValidateLicense(store, licenseCheckCache);
            if (error != null)
            {
                return new ShippingException(error.Message, error);
            }

            return null;
        }
    }
}
