using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Ebay.WizardPages;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Service for license related tasks 
    /// </summary>
    public class LicenseService : ILicenseService
    {
        private readonly ICustomerLicenseReader reader;
        private readonly Func<string, ICustomerLicense> customerLicenseFactory;
        private readonly Func<StoreEntity, StoreLicense> storeLicenseFactory;
        private readonly IStoreManager storeManager;


        /// <summary>
        /// Constructor
        /// </summary>
        public LicenseService(ICustomerLicenseReader reader, Func<string, ICustomerLicense> customerLicenseFactory, Func<StoreEntity, StoreLicense> storeLicenseFactory,  IStoreManager storeManager)
        {
            this.reader = reader;
            this.customerLicenseFactory = customerLicenseFactory;
            this.storeLicenseFactory = storeLicenseFactory;
            this.storeManager = storeManager;
        }

        private string CustomerKey => reader.Read();

        private bool IsLegacy => string.IsNullOrEmpty(CustomerKey);

        /// <summary>
        /// Returns the correct ILicense for the store
        /// </summary>
        public ILicense GetLicense(StoreEntity store)
        {
            // If Legacy, return store license, else return customer license
            return IsLegacy ?
                (ILicense) storeLicenseFactory(store) :
                 customerLicenseFactory(CustomerKey);
        }

        /// <summary>
        /// Gets all Licenses.
        /// </summary>
        public IEnumerable<ILicense> GetLicenses()
        {
            // If Legacy, return store licenses for each store, else return a single customer license
            return IsLegacy ?
                storeManager.GetEnabledStores().Select(GetLicense) :
                new[] {customerLicenseFactory(CustomerKey)};
        }

        /// <summary>
        /// Can the customer Logon?
        /// </summary>
        public EnumResult<LogOnRestrictionLevel> AllowsLogOn()
        {
            // Legacy users are always allowed to log on, only new pricing restricts logon
            if (IsLegacy)
            {
                return new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.None);
            }
            
            ILicense customerLicense = customerLicenseFactory(CustomerKey);

            // Customer licenses that are disabled cannot logon, refresh the 
            //license info with tango before checking if the license is disabled
            customerLicense.Refresh();

            return customerLicense.IsDisabled ?
                new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.Forbidden, customerLicense.DisabledReason) :
                new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.None);
        }
    }
}
