using System.Collections.Generic;
using System.Reflection;
using ShipWorks.Warehouse.DTO.Configuration.ShippingSettings;

namespace ShipWorks.Warehouse.DTO.Configuration
{
    [Obfuscation]
    public class HubConfiguration
    {
        public List<CarrierConfiguration> CarrierConfigurations { get; set; }
    }
}
