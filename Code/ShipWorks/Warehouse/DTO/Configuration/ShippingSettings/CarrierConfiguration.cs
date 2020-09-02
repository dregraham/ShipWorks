using System.Reflection;

namespace ShipWorks.Warehouse.DTO.Configuration.ShippingSettings
{
    [Obfuscation]
    public class CarrierConfiguration
    {
        public int TypeCode { get; set; }

        public CarrierConfigurationPayload Payload { get; set; }
    }
}
