using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Licensing
{
    public class StoreLicense : ILicense
    {
        private readonly StoreEntity store;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public StoreLicense(StoreEntity store)
        {
            this.store = store;
        }

        public void Activate(string email, string password)
        {
            // no need to activate for legacy
        }

        /// <summary>
        /// The store license key
        /// </summary>
        public string Key
        {
            get { return store.License; }
            set { store.License = value; }
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
            catch (ShipWorksLicenseException ex)
            {
                DisabledReason = ex.Message;
            }
        }

        /// <summary>
        /// The reason the store is disabled.
        /// </summary>
        public string DisabledReason { get; set; }

        /// <summary>
        /// Is the store disabled?
        /// </summary>
        public bool IsDisabled => !string.IsNullOrEmpty(DisabledReason);
    }
}