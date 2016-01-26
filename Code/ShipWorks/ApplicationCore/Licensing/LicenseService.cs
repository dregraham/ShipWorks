using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Service for license related tasks 
    /// </summary>
    public class LicenseService : ILicenseService
    {
        private readonly Func<string, ICustomerLicense> customerLicenseFactory;
        private readonly Func<StoreEntity, StoreLicense> storeLicenseFactory;
        private readonly IStoreManager storeManager;
        private readonly bool isLegacy;
        private readonly string customerKey;

        /// <summary>
        /// Constructor
        /// </summary>
        public LicenseService(ICustomerLicenseReader reader, Func<string, ICustomerLicense> customerLicenseFactory, Func<StoreEntity, StoreLicense> storeLicenseFactory,  IStoreManager storeManager)
        {
            this.customerLicenseFactory = customerLicenseFactory;
            this.storeLicenseFactory = storeLicenseFactory;
            this.storeManager = storeManager;

            customerKey = reader.Read();
            isLegacy = string.IsNullOrEmpty(customerKey);
        }

        /// <summary>
        /// Returns the correct ILicense for the store
        /// </summary>
        public ILicense GetLicense(StoreEntity store)
        {
            // If Legacy, return store license, else return customer license
            return isLegacy ?
                (ILicense) storeLicenseFactory(store) :
                 customerLicenseFactory(customerKey);
        }

        /// <summary>
        /// Gets all Licenses.
        /// </summary>
        public IEnumerable<ILicense> GetLicenses()
        {
            // If Legacy, return store licenses for each store, else return a single customer license
            return isLegacy ?
                storeManager.GetEnabledStores().Select(GetLicense) :
                new[] {customerLicenseFactory(customerKey)};
        }

        /// <summary>
        /// Can the customer Logon?
        /// </summary>
        public EnumResult<LogOnRestrictionLevel> AllowsLogOn()
        {
            // Legacy users are always allowed to log on, only new pricing restricts logon
            if (isLegacy)
            {
                return new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.None);
            }
            
            ILicense customerLicense = customerLicenseFactory(customerKey);

            // Customer licenses that are disabled cannot logon, refresh the 
            //license info with tango before checking if the license is disabled
            customerLicense.Refresh();

            return customerLicense.IsDisabled ?
                new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.Forbidden, customerLicense.DisabledReason) :
                new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.None);
        }
    }
}
