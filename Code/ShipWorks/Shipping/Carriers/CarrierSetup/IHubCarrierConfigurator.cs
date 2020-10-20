using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;

namespace ShipWorks.Shipping.Carriers.CarrierSetup
{
    /// <summary>
    /// Configures carriers downloaded from the Hub
    /// </summary>
    public interface IHubCarrierConfigurator
    {
        /// <summary>
        /// Configure carriers
        /// </summary>
        Task Configure(List<CarrierConfiguration> carrierConfigurations);
    }
}
