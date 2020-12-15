using System.Collections.Generic;
using System.Reflection;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;

namespace ShipWorks.Warehouse.Configuration.DTO
{
    /// <summary>
    /// DTO for importing hub configuration data
    /// </summary>
    [Obfuscation]
    public class HubConfiguration
    {
        /// <summary>
        /// A list of the carriers configured in hub
        /// </summary>
        public List<CarrierConfiguration> CarrierConfigurations { get; set; }

        /// <summary>
        /// The ShipEngine API key associated with this customer in the hub
        /// </summary>
        public string ShipEngineApiKey { get; set; }

        /// <summary>
        /// The ShipEngine Account ID associated with this customer in the hub
        /// </summary>
        public string ShipEngineAccountID { get; set; }
    }
}
