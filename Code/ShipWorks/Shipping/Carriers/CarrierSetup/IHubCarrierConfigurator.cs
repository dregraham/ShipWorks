using System.Collections.Generic;
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
        void Configure(List<CarrierConfiguration> carrierConfigurations);
    }
}
