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