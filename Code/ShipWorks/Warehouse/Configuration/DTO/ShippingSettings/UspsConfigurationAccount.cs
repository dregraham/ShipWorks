using System.Reflection;

namespace ShipWorks.Warehouse.Configuration.DTO.ShippingSettings
{
    [Obfuscation]
    public class UspsConfigurationAccount
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public long AccountId { get; set; }

        public int ContractType { get; set; }

        public int GlobalPost { get; set; }

        public string ShipEngineId { get; set; }
    }
}
