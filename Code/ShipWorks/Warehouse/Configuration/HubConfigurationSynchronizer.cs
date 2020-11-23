using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration.Ordering;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Warehouse.Configuration.Stores;

namespace ShipWorks.Warehouse.Configuration
{
    /// <summary>
    /// Class to import and apply the configuration from Hub
    /// </summary>
    [Order(typeof(IInitializeForCurrentSession), 2)]
    public class HubConfigurationSynchronizer : IInitializeForCurrentSession
    {
        private readonly ILicenseService licenseService;
        private readonly IHubConfigurator configurator;
        private readonly IHubConfigurationWebClient webClient;
        private readonly IConfigurationData configurationData;
        private readonly IHubStoreSynchronizer hubStoreSynchronizer;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public HubConfigurationSynchronizer(ILicenseService licenseService,
            IHubConfigurator configurator,
            IHubConfigurationWebClient webClient,
            IConfigurationData configurationData,
            IHubStoreSynchronizer hubStoreSynchronizer,
            Func<Type, ILog> logFactory)
        {
            this.licenseService = licenseService;
            this.configurator = configurator;
            this.webClient = webClient;
            this.configurationData = configurationData;
            this.hubStoreSynchronizer = hubStoreSynchronizer;
            log = logFactory(typeof(HubConfigurationSynchronizer));
        }

        /// <summary>
        /// Initialize for the currently logged on user
        /// </summary>
        public void InitializeForCurrentSession()
        {
            if (licenseService.IsHub)
            {
                IConfigurationEntity configuration = configurationData.FetchReadOnly();
                if (!string.IsNullOrEmpty(configuration.WarehouseID))
                {
                    Task.Run(async () =>
                    {
                        try
                        {
                            var hubConfig = await webClient.GetConfig(configuration.WarehouseID).ConfigureAwait(false);
                            await configurator.Configure(hubConfig).ConfigureAwait(false);
                            await hubStoreSynchronizer.SynchronizeStoresIfNeeded(hubConfig.StoreConfigurations).ConfigureAwait(false);
                        }
                        catch (AggregateException ex) when (ex.InnerExceptions.FirstOrDefault() is WebException)
                        {
                            log.Error("Error getting configuration", ex);
                        }
                        catch (WebException ex)
                        {
                            log.Error("Error getting configuration", ex);
                        }
                    });
                }
            }
        }
    }
}
