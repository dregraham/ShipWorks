using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Warehouse.DTO.Configuration.ShippingSettings
{
    [Obfuscation]
    public class UspsConfigurationAccount : CarrierConfigurationAccount
    {
        public int ContractType { get; set; }

        public int GlobalPost { get; set; }

        public int ShipEngineId { get; set; }
    }
}
