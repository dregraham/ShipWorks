using System.Collections.Generic;
using System.Reflection;
using ShipWorks.Warehouse.Configuration.Carriers.DTO;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

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
        /// A list of the stores configured in hub
        /// </summary>
        public List<StoreConfiguration> StoreConfigurations { get; set; }
    }
}
