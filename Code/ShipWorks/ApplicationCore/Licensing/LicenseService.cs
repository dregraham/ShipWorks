using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
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
        }

        /// <summary>
        /// Customer Key read from the reader.
        /// </summary>
        /// <exception cref="EncryptionException"></exception>
        private string CustomerKey
        {
            get
            {
                string customerKey = reader.Value.Read();

                isLegacy = string.IsNullOrWhiteSpace(customerKey);

                return customerKey;
            }
        }

        /// <summary>
        /// True if Legacy Customer
        /// </summary>
        /// <remarks>True if CustomerKey is null or empty</remarks>
        /// <exception cref="EncryptionException" />
        private bool IsLegacy
        {
            get
            {
                if (!isLegacy.HasValue)
                {
                    // Yes, this is duplicated code, but needed a way to get the initial value.
                    isLegacy = string.IsNullOrWhiteSpace(CustomerKey);
                }

                return isLegacy.Value;
            }
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
                return IsLegacy ? (ILicense) storeLicenseFactory(store) : GetCustomerLicense();
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
        /// Returns the cached customer license or creates a new license if there is no cache
        /// </summary>
        private ICustomerLicense GetCustomerLicense()
        {
            // If the cache is null or the key has changed due to a new license being activated
            if (cachedCustomerLicense == null || CustomerKey != cachedCustomerLicense.Key)
            {
                cachedCustomerLicense = customerLicenseFactory(CustomerKey);
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
