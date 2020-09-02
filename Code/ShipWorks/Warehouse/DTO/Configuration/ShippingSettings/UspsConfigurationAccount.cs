using System.Reflection;

namespace ShipWorks.Warehouse.DTO.Configuration.ShippingSettings
{
    [Obfuscation]
    public class UspsConfigurationAccount : CarrierConfigurationAccount
    {
        public long AccountId { get; set; }

        public int ContractType { get; set; }

        public int GlobalPost { get; set; }

        public int ShipEngineId { get; set; }
    }
}
