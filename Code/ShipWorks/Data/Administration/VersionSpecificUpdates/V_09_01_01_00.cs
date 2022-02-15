using System;
using System.Linq;
using log4net;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Stores;

namespace ShipWorks.Data.Administration.VersionSpecificUpdates
{
    /// <summary>
    /// Populates Tango License Key for legacy customers
    /// </summary>
    public class V_09_01_01_00 : IVersionSpecificUpdate
    {
        private readonly IConfigurationData configurationData;
        private readonly ICustomerLicenseWriter customerLicenseWriter;
        private readonly ILicenseService licenseService;
        private readonly IStoreManager storeManager;
        private readonly ITangoWebClient tangoWebClient;
        private readonly ILog log;

        /// <summary>
        /// Always run just in case it has never been run before.
        /// </summary>
        public bool AlwaysRun => true;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customerLicense"></param>
        public V_09_01_01_00(IConfigurationData configurationData, ICustomerLicenseWriter customerLicenseWriter,
            ILicenseService licenseService, IStoreManager storeManager, ITangoWebClient tangoWebClient, Func<Type, ILog> logFactory)
        {
            this.configurationData = configurationData;
            this.customerLicenseWriter = customerLicenseWriter;
            this.licenseService = licenseService;
            this.storeManager = storeManager;
            this.tangoWebClient = tangoWebClient;
            log = logFactory(typeof(V_09_01_01_00));
        }

        /// <summary>
        /// To which version does this update apply
        /// </summary>
        public Version AppliesTo => new Version(9, 1, 1, 0);

        /// <summary>
        /// Run the update
        /// </summary>
        public void Update()
        {
            log.Info("Applying programmatic update V_09_01_01_00");
            configurationData.CheckForChangesNeeded();
            storeManager.InitializeForCurrentSession();
            storeManager.CheckForChanges();
            
            log.Info("Checking if legacy.");
            if (licenseService.IsLegacy && string.IsNullOrEmpty(configurationData.FetchReadOnly().LegacyCustomerKey))
            {
                log.Info("Indeed legacy, updating. Get all stores");
                string customerLicenseKey = string.Empty;
                // get first store with a non-trial license key
                var store = storeManager.GetAllStores().FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(x.License) && !new ShipWorksLicense(x.License).IsLegacyTrialKey);

                if (store != null)
                {
                    log.Info("Store is not null. setting customer license key");
                    customerLicenseKey = tangoWebClient.GetCustomerLicenseKey(store.License);
                }

                if (!string.IsNullOrWhiteSpace(customerLicenseKey))
                {
                    log.Info($"Setting Customer License Key.");
                    customerLicenseWriter.Write(customerLicenseKey, CustomerLicenseKeyType.Legacy);
                }
            }
            log.Info("Update Complete!");
        }
    }
}