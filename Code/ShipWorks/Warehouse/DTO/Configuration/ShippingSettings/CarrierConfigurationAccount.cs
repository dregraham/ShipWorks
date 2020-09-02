using System.Reflection;

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
