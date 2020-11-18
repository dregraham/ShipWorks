using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Warehouse.Configuration.Carriers;
using ShipWorks.Warehouse.Configuration.DTO;
using ShipWorks.Warehouse.Configuration.Stores;

namespace ShipWorks.Warehouse.Configuration
{
    /// <summary>
    /// Class to configure data downloaded from the Hub
    /// </summary>
    [Component]
    public class HubConfigurator : IHubConfigurator
    {
        private readonly IHubCarrierConfigurator carrierConfigurator;
        private readonly IHubStoreConfigurator storeConfigurator;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubConfigurator(IHubCarrierConfigurator carrierConfigurator, IHubStoreConfigurator storeConfigurator)
        {
            this.carrierConfigurator = carrierConfigurator;
            this.storeConfigurator = storeConfigurator;
        }

        /// <summary>
        /// Perform the given configuration
        /// </summary>
        public async Task Configure(HubConfiguration config)
        {
            await carrierConfigurator.Configure(config.CarrierConfigurations);
            await storeConfigurator.Configure(config.StoreConfigurations);
        }
    }
}
