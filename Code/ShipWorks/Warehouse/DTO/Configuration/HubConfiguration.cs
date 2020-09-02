using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Warehouse.DTO.Configuration.ShippingSettings;

namespace ShipWorks.Warehouse.DTO.Configuration
{
    [Obfuscation]
    public class HubConfiguration
    {
        public List<CarrierConfiguration> CarrierConfigurations { get; set; }
    }
}
