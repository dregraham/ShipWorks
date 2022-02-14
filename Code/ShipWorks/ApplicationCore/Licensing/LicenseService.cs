using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Stores;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Service for license related tasks
    /// </summary>
    [Component]
    public class LicenseService : ILicenseService, IInitializeForCurrentDatabase
    {
        private readonly Lazy<ICustomerLicenseReader> reader;
        private readonly Func<string, ICustomerLicense> customerLicenseFactory;
        private readonly Func<StoreEntity, StoreLicense> storeLicenseFactory;
        private readonly IStoreManager storeManager;
        private readonly Func<Owned<IUserSession>> getUserSession;
        private readonly IList<ILicense> cachedStoreLicenses;
        private ICustomerLicense cachedCustomerLicense;
        private bool? isLegacy = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public LicenseService(Lazy<ICustomerLicenseReader> reader, Func<string, ICustomerLicense> customerLicenseFactory,
            Func<StoreEntity, StoreLicense> storeLicenseFactory, IStoreManager storeManager, Func<Owned<IUserSession>> getUserSession)
        {
            this.reader = reader;
            this.customerLicenseFactory = customerLicenseFactory;
            this.storeLicenseFactory = storeLicenseFactory;
            this.storeManager = storeManager;
            this.getUserSession = getUserSession;

            cachedStoreLicenses = new List<ILicense>();
        }

        /// <summary>
        /// True if Legacy Customer
        /// </summary>
        /// <remarks>True if CustomerKey is null or empty</remarks>
        /// <exception cref="EncryptionException" />
        public bool IsLegacy
        {
            get
            {
                if (!isLegacy.HasValue)
                {
                    // Yes, this is duplicated code, but needed a way to get the initial value.
                    isLegacy = string.IsNullOrWhiteSpace(GetCustomerLicenseKey(CustomerLicenseKeyType.WebReg));
                }

                return isLegacy.Value;
            }
        }

        /// <summary>
        /// True if Hub customer
        /// </summary>
        public bool IsHub => CheckRestriction(EditionFeature.Warehouse, null) == EditionRestrictionLevel.None;

        /// <summary>
        /// Gets the customer's license type
        /// </summary>
        private CustomerLicenseKeyType GetCustomerLicenseKeyType()
        {
            return IsLegacy ? CustomerLicenseKeyType.Legacy : CustomerLicenseKeyType.WebReg;
        }

        /// <summary>
        /// Get the customers license key
        /// </summary>
        public string GetCustomerLicenseKey()
        {
            var customerLicenseKeyType = GetCustomerLicenseKeyType();
            return reader.Value.Read(customerLicenseKeyType);
        }
        
        /// <summary>
        /// Get the customers license key
        /// </summary>
        public string GetCustomerLicenseKey(CustomerLicenseKeyType licenseKeyType)
        {
            string customerKey = reader.Value.Read(licenseKeyType);
            
            if (licenseKeyType == CustomerLicenseKeyType.WebReg)
            {
                // if getting webreg key and it is blank, then customer is legacy
                isLegacy = string.IsNullOrWhiteSpace(customerKey);
            }
            else
            {
                // if getting legacy key and it is not blank, then customer is legacy
                isLegacy = !string.IsNullOrWhiteSpace(customerKey);
            }
            
            return customerKey;
        }
        
        /// <summary>
        /// Returns the correct ILicense for the store
        /// </summary>
        /// <exception cref="EncryptionException" />
        public ILicense GetLicense(StoreEntity store)
        {
            try
            {
                // If Legacy, return store license, else return customer license
                return IsLegacy ? GetStoreLicense(store) : GetCustomerLicense();
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
            return CheckRestrictionWithReason(feature, data).Value;
        }

        /// <summary>
        /// Checks the restriction for a specific feature
        /// </summary>
        public EnumResult<EditionRestrictionLevel> CheckRestrictionWithReason(EditionFeature feature, object data)
        {
            if (SqlSession.Current == null || !getUserSession().Value.IsLoggedOn)
            {
                return EditionRestrictionLevel.Forbidden.AsEnumResult("You must be logged in");
            }

            return IsLegacy ?
                EditionManager.ActiveRestrictions.CheckRestriction(feature, data).Level.AsEnumResult() :
                GetCustomerLicense().CheckRestriction(feature, data);
        }

        /// <summary>
        /// Handles the restriction for a specific feature
        /// </summary>
        public bool HandleRestriction(EditionFeature feature, object data, IWin32Window owner)
        {
            if (SqlSession.Current == null || !getUserSession().Value.IsLoggedOn)
            {
                return false;
            }

            return IsLegacy ?
                EditionManager.HandleRestrictionIssue(owner, EditionManager.ActiveRestrictions.CheckRestriction(feature, data)) :
                GetCustomerLicense().HandleRestriction(feature, data, owner);
        }

        /// <summary>
        /// Gets all Licenses.
        /// </summary>
        /// <exception cref="EncryptionException" />
        public IEnumerable<ILicense> GetLicenses()
        {
            try
            {
                return IsLegacy ?
                    storeManager.GetEnabledStores().Select(GetLicense) :
                    new[] { GetCustomerLicense() };
            }
            catch (ShipWorksLicenseException ex)
            {
                return new[] { new DisabledLicense(ex.Message) };
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
                    // Ensures the legacy customer license key exists. If not, this will throw and trigger the 
                    // activation flow, which will set it.
                    GetCustomerLicenseKey(CustomerLicenseKeyType.Legacy);
                    
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
            customerLicense.ForceRefresh();

            return customerLicense.IsDisabled ?
                new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.Forbidden, customerLicense.DisabledReason) :
                new EnumResult<LogOnRestrictionLevel>(LogOnRestrictionLevel.None);
        }

        /// <summary>
        /// Try to get store license from cache. If it's not in the cache, create one, add it to the cache, and return it
        /// </summary>
        private ILicense GetStoreLicense(StoreEntity store)
        {
            var cachedStoreLicense = cachedStoreLicenses.SingleOrDefault(x =>
                x.Key.Equals(store.License, StringComparison.InvariantCultureIgnoreCase));

            // If cached license is null, add it
            if (cachedStoreLicense == null)
            {
                cachedStoreLicense = storeLicenseFactory(store);
                cachedStoreLicenses.Add(cachedStoreLicense);
            }

            return cachedStoreLicense;
        }
        
        /// <summary>
        /// Returns the cached customer license or creates a new license if there is no cache
        /// </summary>
        private ICustomerLicense GetCustomerLicense()
        {
            var customerLicenseKey = GetCustomerLicenseKey(CustomerLicenseKeyType.WebReg);
            // If the cache is null or the key has changed due to a new license being activated
            if (cachedCustomerLicense == null || customerLicenseKey != cachedCustomerLicense.Key)
            {
                cachedCustomerLicense = customerLicenseFactory(customerLicenseKey);
            }

            return cachedCustomerLicense;
        }

        /// <summary>
        /// Initializes this class for the current database.
        /// </summary>
        public void InitializeForCurrentDatabase(ExecutionMode.ExecutionMode executionMode)
        {
            // Set isLegacy to null so that we will go to the db to get the new CustomerKey
            isLegacy = null;
        }
    }
}
