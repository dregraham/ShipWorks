using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Warehouse.DTO.Configuration.ShippingSettings
{
    [Obfuscation]
    public class CarrierConfigurationPayload
    {
        public ConfigurationAddress Address { get; set; }
        public CarrierConfigurationAccount Account { get; set; }
    }
}
