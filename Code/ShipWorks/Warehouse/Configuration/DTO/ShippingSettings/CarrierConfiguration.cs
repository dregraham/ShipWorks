using System.Reflection;
using ShipWorks.Shipping;

namespace ShipWorks.Warehouse.Configuration.DTO.ShippingSettings
{
    [Obfuscation]
    public class CarrierConfiguration
    {
        public ShipmentTypeCode TypeCode { get; set; }

        public CarrierConfigurationPayload Payload { get; set; }
    }
}
