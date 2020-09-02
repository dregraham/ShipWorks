using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Warehouse.DTO.Configuration.ShippingSettings
{
    [Obfuscation]
    public class CarrierConfiguration
    {
        public int TypeCode { get; set; }
        public CarrierConfigurationPayload Payload { get; set; }
    }
}
