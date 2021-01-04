using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Warehouse.Configuration.Carriers;
using ShipWorks.Warehouse.Configuration.DTO;

namespace ShipWorks.Warehouse.Configuration
{
    /// <summary>
    /// Class to configure data downloaded from the Hub
    /// </summary>
    [Component]
    public class HubConfigurator : IHubConfigurator
    {
        private readonly IHubCarrierConfigurator carrierConfigurator;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubConfigurator(IHubCarrierConfigurator carrierConfigurator)
        {
            this.carrierConfigurator = carrierConfigurator;
        }

        /// <summary>
        /// Perform the given configuration
        /// </summary>
        public async Task Configure(HubConfiguration config)
        {
            await carrierConfigurator.Configure(config.CarrierConfigurations).ConfigureAwait(false);
        }
    }
}
