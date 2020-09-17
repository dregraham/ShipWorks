using System.Reflection;
using ShipWorks.Shipping;

namespace ShipWorks.Warehouse.Configuration.DTO.ShippingSettings
{
    /// <summary>
    /// DTO for importing a carrier confiugration from hub
    /// </summary>
    [Obfuscation]
    public class CarrierConfiguration
    {
        /// <summary>
        /// The type code of the configured carrier
        /// </summary>
        public ShipmentTypeCode TypeCode { get; set; }

        /// <summary>
        /// The payload of the configuration containing configuration data
        /// </summary>
        public CarrierConfigurationPayload Payload { get; set; }
    }
}
