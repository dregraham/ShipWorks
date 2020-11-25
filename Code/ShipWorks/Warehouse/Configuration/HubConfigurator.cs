using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Carriers.CarrierSetup;
using ShipWorks.Warehouse.Configuration.Customer;
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
        private readonly IHubCustomerConfigurator customerConfigurator;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubConfigurator(IHubCarrierConfigurator carrierConfigurator, IHubCustomerConfigurator customerConfigurator)
        {
            this.carrierConfigurator = carrierConfigurator;
            this.customerConfigurator = customerConfigurator;
        }

        /// <summary>
        /// Perform the given configuration
        /// </summary>
        public async Task Configure(HubConfiguration config)
        {
            await carrierConfigurator.Configure(config.CarrierConfigurations).ConfigureAwait(false);
            await customerConfigurator.Configure(config).ConfigureAwait(false);
        }
    }
}