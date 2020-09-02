using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Warehouse.DTO.Configuration.ShippingSettings
{
    [Obfuscation]
    public class CarrierConfigurationAccount
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
    }
}
