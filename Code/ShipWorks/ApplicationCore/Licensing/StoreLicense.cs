using System;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;

namespace ShipWorks.ApplicationCore.Licensing
{
    public class StoreLicense : ILicense
    {
        private readonly StoreEntity store;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public StoreLicense(StoreEntity store, Func<Type, ILog> logFactory)
        {
            this.store = store;
            log = logFactory(GetType());
            Key = store.License;
        }

        /// <summary>
        /// The reason the store is disabled.
        /// </summary>
        public string DisabledReason { get; set; }

        /// <summary>
        /// Is the store disabled?
        /// </summary>
        public bool IsDisabled => !string.IsNullOrEmpty(DisabledReason);

        /// <summary>
        /// License key
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Is the license legacy
        /// </summary>
        public bool IsLegacy => true;

        /// <summary>
        /// Store licenses do not have channel limits
        /// </summary>
        /// <remarks>
        /// Always returns false
        /// </remarks>
        public bool IsOverChannelLimit => false;

        /// <summary>
        /// Store licenses do not have shipment limits
        /// </summary>
        /// <remarks>
        /// Always returns false
        /// </remarks>
        public bool IsShipmentLimitReached => false;

        /// <summary>
        /// Store license do not have channel limits
        /// </summary>
        /// <remarks>
        /// Always return 0
        /// </remarks>
        public int NumberOfChannelsOverLimit => 0;

        /// <summary>
        /// Activate a new store
        /// </summary>
        /// <remarks>
        /// Make sure store is populated with the appropriate license key.
        /// </remarks>
        public EnumResult<LicenseActivationState> Activate(StoreEntity newStore)
        {
            ShipWorksLicense license = new ShipWorksLicense(store.License);

            return license.IsTrial ?
                new EnumResult<LicenseActivationState>(LicenseActivationState.Active) :
                LicenseActivationHelper.ActivateAndSetLicense(newStore, newStore.License);
        }

        /// <summary>
        /// Nothing to enforce
        /// </summary>
        public void EnforceChannelLimit()
        {
        }

        /// <summary>
        /// Nothing to enforce
        /// </summary>
        public void EnforceShipmentLimit()
        {
        }

        /// <summary>
        /// Refreshes store license data.
        /// </summary>
        public void Refresh()
        {
            try
            {
                LicenseActivationHelper.EnsureActive(store);
                DisabledReason = string.Empty;
            }
            catch (Exception ex)
                when (ex is ShipWorksLicenseException || ex is TangoException)
            {
                log.Warn(ex.Message, ex);
                DisabledReason = ex.Message;
            }
        }

        /// <summary>
        /// Deletes the store
        /// </summary>
        /// <param name="store"></param>
        public void DeleteStore(StoreEntity store)
        {
            DeletionService.DeleteStore(store);
        }
    }
}