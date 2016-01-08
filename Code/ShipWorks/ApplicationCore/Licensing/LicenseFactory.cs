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
        private readonly CustomerLicense customerLicense;
        private readonly IStoreManager storeManager;
        private readonly bool isLegacy;

        /// <summary>
        /// Constructor
        /// </summary>
        public LicenseFactory(ICustomerLicenseReader reader, CustomerLicense customerLicense, IStoreManager storeManager)
        {
            this.customerLicense = customerLicense;
            this.storeManager = storeManager;

            string customerKey = reader.Read();
            customerLicense.Key = customerKey;
            isLegacy = string.IsNullOrEmpty(customerKey);
        }

        /// <summary>
        /// Returns a store license if legacy else returns the customer license
        /// </summary>
        public ILicense GetLicense(StoreEntity store)
        {
            return isLegacy ? 
                (ILicense) new StoreLicense(store) : 
                customerLicense;
        }

        /// <summary>
        /// Gets all Licenses. If customer license, only 1 is returned, else all stores are returned.
        /// </summary>
        public IEnumerable<ILicense> GetLicenses()
        {
            return isLegacy ? 
                storeManager.GetEnabledStores().Select(GetLicense) : 
                Enumerable.Repeat(customerLicense, 1);
        }
    }
}
