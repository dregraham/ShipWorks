using System;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;

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
        /// Make sure the 
        /// </summary>
        public EnumResult<LicenseActivationState> Activate(StoreEntity newStore)
        {
            ShipWorksLicense license = new ShipWorksLicense(store.License);

            return license.IsTrial ? 
                new EnumResult<LicenseActivationState>(LicenseActivationState.Active) : 
                LicenseActivationHelper.ActivateAndSetLicense(newStore, newStore.License);
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
    }
}