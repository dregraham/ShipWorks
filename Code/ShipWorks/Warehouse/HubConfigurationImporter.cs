using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.CarrierSetup;

namespace ShipWorks.Warehouse
{
    /// <summary>
    /// Class to import the configuration from Hub
    /// </summary>
    [Component]
    public class HubConfigurationImporter : IInitializeForCurrentDatabase
    {
        private readonly ILicenseService licenseService;
        private readonly IHubCarrierConfigurator carrierConfigurator;
        private readonly IHubConfigurationWebClient webClient;
        private readonly IConfigurationEntity configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubConfigurationImporter(ILicenseService licenseService,
            IHubCarrierConfigurator carrierConfigurator,
            IHubConfigurationWebClient webClient,
            IConfigurationEntity configuration)
        {
            this.licenseService = licenseService;
            this.carrierConfigurator = carrierConfigurator;
            this.webClient = webClient;
            this.configuration = configuration;
        }

        /// <summary>
        /// Initialize for the currently logged on user
        /// </summary>
        public void InitializeForCurrentDatabase(ExecutionMode executionMode)
        {
            if (licenseService.IsHub)
            {
                var task = Task.Run(async () => await webClient.GetConfig(configuration.WarehouseID).ConfigureAwait(false));
                var hubConfig = task.Result;

                carrierConfigurator.Configure(hubConfig.CarrierConfigurations);
            }
        }
    }
}
