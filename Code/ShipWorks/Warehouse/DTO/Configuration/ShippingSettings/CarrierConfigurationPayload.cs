using System.Reflection;

namespace ShipWorks.Warehouse.DTO.Configuration.ShippingSettings
{
    [Obfuscation]
    public class CarrierConfigurationPayload
    {
        public ConfigurationAddress Address { get; set; }
        public CarrierConfigurationAccount Account { get; set; }
    }
}
