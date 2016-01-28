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

        /// <summary>
        /// Customer Key read from the reader. 
        /// </summary>
        /// <exception cref="EncryptionException"></exception>
        private string CustomerKey => reader.Read();

        /// <summary>
        /// True if Legacy Customer
        /// </summary>
        /// <remarks>
        /// True if CustomerKey is null or empty
        /// </remarks>
        /// <exception cref="EncryptionException" />
        private bool IsLegacy => string.IsNullOrEmpty(CustomerKey);

        /// <summary>
        /// Returns the correct ILicense for the store
        /// </summary>
        /// <exception cref="EncryptionException" />
        public ILicense GetLicense(StoreEntity store)
        {
            try
            {
                // If Legacy, return store license, else return customer license
                return IsLegacy
                    ? (ILicense) storeLicenseFactory(store)
                    : customerLicenseFactory(CustomerKey);
            }
            catch (ShipWorksLicenseException ex)
            {
                return new DisabledLicense(ex.Message);
            }
        }

        /// <summary>
        /// If License is over the channel limit prompt user to delete channels
        /// </summary>
        public void EnforceChannelLimit()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all Licenses.
        /// </summary>
        /// <exception cref="EncryptionException" />
        public IEnumerable<ILicense> GetLicenses()
        {
            try
            {
                // If Legacy, return store licenses for each store, else return a single customer license
                return IsLegacy
                    ? storeManager.GetEnabledStores().Select(GetLicense)
                    : new[] {customerLicenseFactory(CustomerKey)};
            }
            catch (ShipWorksLicenseException ex)
            {
                return new[] {new DisabledLicense(ex.Message)};
            }
        }

        /// <summary>
        /// Can the customer Logon?
        /// </summary>
        /// <exception cref="EncryptionException" />
        public EnumResult<LogOnRestrictionLevel> AllowsLogOn()
        {
            try
            {
                // Legacy users are always allowed to log on, only new pricing restricts logon
                if (IsLegacy)
                {
                    return new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.None);
                }
            }
            catch (ShipWorksLicenseException ex)
            {
                return new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.Forbidden, ex.Message);
            }

            ILicense customerLicense = customerLicenseFactory(CustomerKey);

            // Customer licenses that are disabled cannot logon, refresh the 
            // license info with tango before checking if the license is disabled
            customerLicense.Refresh();

            return customerLicense.IsDisabled ?
                new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.Forbidden, customerLicense.DisabledReason) :
                new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.None);
        }
    }
}
