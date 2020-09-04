using System.Collections.Generic;
using System.Reflection;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;

namespace ShipWorks.Warehouse.Configuration.DTO
{
    [Obfuscation]
    public class HubConfiguration
    {
        public List<CarrierConfiguration> CarrierConfigurations { get; set; }
    }
}
