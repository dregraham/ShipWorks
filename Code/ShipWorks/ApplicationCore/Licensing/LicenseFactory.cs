using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Factory to create a License
    /// </summary>
    public class LicenseFactory
    {
        private readonly Func<string, CustomerLicense> customerLicenseFactory;
        private readonly Func<StoreEntity, StoreLicense> storeLicenseFactory;
        private readonly IStoreManager storeManager;
        private readonly bool isLegacy;
        private readonly string customerKey;

        /// <summary>
        /// Constructor
        /// </summary>
        public LicenseFactory(ICustomerLicenseReader reader, Func<string, CustomerLicense> customerLicenseFactory, Func<StoreEntity, StoreLicense> storeLicenseFactory,  IStoreManager storeManager)
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
            return isLegacy
                ? storeManager.GetEnabledStores().Select(GetLicense)
                : new[] {customerLicenseFactory(customerKey)};
        }
    }
}
