using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
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
        private ICustomerLicense licenseCache;

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
                    : GetCustomerLicense();
            }
            catch (ShipWorksLicenseException ex)
            {
                return new DisabledLicense(ex.Message);
            }
        }

        /// <summary>
        /// Checks the restriction for a specific feature
        /// </summary>
        public EditionRestrictionLevel CheckRestriction(EditionFeature feature, object data)
        {
            return SqlSession.Current == null || IsLegacy
                ? EditionManager.ActiveRestrictions.CheckRestriction(feature, data).Level
                : GetCustomerLicense().CheckRestriction(feature, data);
        }

        /// <summary>
        /// Handles the restriction for a specific feature
        /// </summary>
        public bool HandleRestriction(EditionFeature feature, object data, IWin32Window owner)
        {
            return SqlSession.Current == null || IsLegacy
                ? EditionManager.HandleRestrictionIssue(owner,
                    EditionManager.ActiveRestrictions.CheckRestriction(feature, data))
                : GetCustomerLicense().HandleRestriction(feature, data, owner);
        }

        /// <summary>
        /// Gets all Licenses.
        /// </summary>
        /// <exception cref="EncryptionException" />
        public IEnumerable<ILicense> GetLicenses()
        {
            try
            {
                if (IsLegacy)
                {
                    return storeManager.GetEnabledStores().Select(GetLicense);
                }

                return new[] { GetCustomerLicense() };
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
            
            ILicense customerLicense = GetCustomerLicense();

            // Customer licenses that are disabled cannot logon, refresh the
            // license info with tango before checking if the license is disabled
            customerLicense.Refresh();

            return customerLicense.IsDisabled ?
                new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.Forbidden, customerLicense.DisabledReason) :
                new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.None);
        }

        /// <summary>
        /// Returns the cached customer license or creates a new license if there is no cache
        /// </summary>
        private ICustomerLicense GetCustomerLicense()
        {
            return licenseCache ?? (licenseCache = customerLicenseFactory(CustomerKey));
        }
    }
}
