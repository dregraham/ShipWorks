using System.Collections.Generic;
using ShipWorks.Warehouse.DTO.Configuration.ShippingSettings;

namespace ShipWorks.Shipping.Carriers.CarrierSetup
{
    interface IHubCarrierConfigurator
    {
        void Configure(List<CarrierConfiguration> configs);
    }
}
