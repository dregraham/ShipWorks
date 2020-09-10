using System;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration.Ordering;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.CarrierSetup;
using System.Linq;

namespace ShipWorks.Warehouse.Configuration
{
    /// <summary>
    /// Class to import and apply the configuration from Hub
    /// </summary>
    [Order(typeof(IInitializeForCurrentSession), 2)]
    public class HubConfigurationImporter : IInitializeForCurrentSession
    {
        private readonly ILicenseService licenseService;
        private readonly IHubCarrierConfigurator carrierConfigurator;
        private readonly IHubConfigurationWebClient webClient;
        private readonly IConfigurationData configurationData;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubConfigurationImporter(ILicenseService licenseService,
            IHubCarrierConfigurator carrierConfigurator,
            IHubConfigurationWebClient webClient,
            IConfigurationData configurationData,
             Func<Type, ILog> logFactory)
        {
            this.licenseService = licenseService;
            this.carrierConfigurator = carrierConfigurator;
            this.webClient = webClient;
            this.configurationData = configurationData;
            log = logFactory(typeof(HubConfigurationImporter));
        }

        /// <summary>
        /// Initialize for the currently logged on user
        /// </summary>
        public void InitializeForCurrentSession()
        {
            if (licenseService.IsHub)
            {
                try
                {
                    IConfigurationEntity configuration = configurationData.FetchReadOnly();
                    var task = Task.Run(async () => await webClient.GetConfig(configuration.WarehouseID).ConfigureAwait(false));
                    var hubConfig = task.Result;

                    carrierConfigurator.Configure(hubConfig.CarrierConfigurations);
                } 
                catch (AggregateException ex) when (ex.InnerExceptions.FirstOrDefault() is WebException)
                {
                    log.Error("Error getting configuration", ex);
                }
                catch(WebException ex)
                {
                    log.Error("Error getting configuration", ex);
                }
            }
        }
    }
}
