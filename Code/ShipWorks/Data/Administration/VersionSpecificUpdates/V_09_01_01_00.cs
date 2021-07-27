using System;
using System.Linq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Stores;

namespace ShipWorks.Data.Administration.VersionSpecificUpdates
{
    public class V_09_01_01_00 : IVersionSpecificUpdate
    {
        private readonly IConfigurationData configurationData;
        private readonly ICustomerLicenseWriter customerLicenseWriter;
        private readonly ILicenseService licenseService;
        private readonly IStoreManager storeManager;
        private readonly ITangoWebClient tangoWebClient;

        /// <summary>
        /// Always run just in case it has never been run before.
        /// </summary>
        public bool AlwaysRun => true;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customerLicense"></param>
        public V_09_01_01_00(IConfigurationData configurationData, ICustomerLicenseWriter customerLicenseWriter,
            ILicenseService licenseService, IStoreManager storeManager, ITangoWebClient tangoWebClient)
        {
            this.configurationData = configurationData;
            this.customerLicenseWriter = customerLicenseWriter;
            this.licenseService = licenseService;
            this.storeManager = storeManager;
            this.tangoWebClient = tangoWebClient;
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
            configurationData.CheckForChangesNeeded();
            storeManager.CheckForChanges();
            
            if (licenseService.IsLegacy && string.IsNullOrEmpty(configurationData.FetchReadOnly().LegacyCustomerKey))
            {
                string customerLicenseKey = string.Empty;
                // get first store with a non-trial license key
                var store = storeManager.GetAllStores().FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(x.License) && !new ShipWorksLicense(x.License).IsLegacyTrialKey);

                if (store != null)
                {
                    customerLicenseKey = tangoWebClient.GetCustomerLicenseKey(store.License);
                }

                if (!string.IsNullOrWhiteSpace(customerLicenseKey))
                {
                    customerLicenseWriter.Write(customerLicenseKey, CustomerLicenseKeyType.Legacy);
                }
            }
        }
    }
}