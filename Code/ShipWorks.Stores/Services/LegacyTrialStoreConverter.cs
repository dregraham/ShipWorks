using System;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.Stores.Services
{
    [Component]
    public class LegacyTrialStoreConverter : ILegacyTrialStoreConverter
    {
        private readonly ILicenseService licenseService;
        private readonly ITangoWebClient tangoWebClient;
        private readonly IStoreManager storeManager;
        private readonly ILog log;

        public LegacyTrialStoreConverter(ILicenseService licenseService, ITangoWebClient tangoWebClient, IStoreManager storeManager, Func<Type, ILog> logFactory)
        {
            this.licenseService = licenseService;
            this.tangoWebClient = tangoWebClient;
            this.storeManager = storeManager;
            log = logFactory(GetType());
        }

        public void ConvertTrials()
        {
            try
            {
                // store trials only exist for legacy customers
                if (licenseService.IsLegacy)
                {
                    // grab all trial stores
                    var trialStores = storeManager.GetAllStores().Where(x => licenseService.GetLicense(x).IsInTrial);

                    foreach (var trialStore in trialStores)
                    {
                        // stash trial license for later because we're about to override it
                        string trialLicenseKey = trialStore.License;
                        
                        // add store to account and save new key
                        var addStoreResponse = tangoWebClient.AddStore(trialStore.License, trialStore);
                        trialStore.License = addStoreResponse.Key;
                        
                        storeManager.SaveStore(trialStore);

                        // make call to convert trial
                        tangoWebClient.ConvertLegacyTrialStore(trialLicenseKey);
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