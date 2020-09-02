using System.Collections.Generic;
using ShipWorks.Warehouse.DTO.Configuration.ShippingSettings;

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
        void Configure(List<CarrierConfiguration> configs);
    }
}
