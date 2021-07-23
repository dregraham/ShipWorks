using System;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.Stores.Services
{
    /// <summary>
    /// Service to convert legacy trial store keys
    /// </summary>
    [Component]
    public class LegacyStoreTrialKeyConverter : ILegacyStoreTrialKeyConverter
    {
        private readonly ILicenseService licenseService;
        private readonly ITangoWebClient tangoWebClient;
        private readonly IStoreManager storeManager;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public LegacyStoreTrialKeyConverter(ILicenseService licenseService, ITangoWebClient tangoWebClient, IStoreManager storeManager, Func<Type, ILog> logFactory)
        {
            this.licenseService = licenseService;
            this.tangoWebClient = tangoWebClient;
            this.storeManager = storeManager;
            log = logFactory(GetType());
        }

        /// <summary>
        /// Convert legacy store trial keys into real keys
        /// </summary>
        public void ConvertTrials()
        {
            try
            {
                // store trials only exist for legacy customers
                if (licenseService.IsLegacy)
                {
                    // grab all trial stores
                    var trialStores = storeManager.GetAllStores()
                        .Where(x => new ShipWorksLicense(x.License).IsLegacyTrialKey);

                    foreach (var trialStore in trialStores)
                    {
                        // stash trial license for later because we're about to override it
                        string trialLicenseKey = trialStore.License;
                        
                        // add store to account and save new key
                        var addStoreResponse = tangoWebClient.AddStore(licenseService.LegacyCustomerKey, trialStore);

                        if (!string.IsNullOrWhiteSpace(addStoreResponse.Key))
                        {
                            trialStore.License = addStoreResponse.Key;
                        
                            storeManager.SaveStore(trialStore);

                            // make call to convert trial
                            tangoWebClient.ConvertLegacyTrialStore(trialLicenseKey);
                        }
                        else
                        {
                            log.Error($"Add store did not return license key for store id {trialStore.StoreID}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to convert legacy trial stores", ex);
            }
        }
    }
}