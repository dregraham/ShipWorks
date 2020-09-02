using System.Reflection;
using ShipWorks.Shipping;

namespace ShipWorks.Warehouse.DTO.Configuration.ShippingSettings
{
    [Obfuscation]
    public class CarrierConfiguration
    {
        public ShipmentTypeCode TypeCode { get; set; }

        public CarrierConfigurationPayload Payload { get; set; }
    }
}
