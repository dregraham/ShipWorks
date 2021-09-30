using System.Collections.Generic;
using System.Reflection;
using ShipWorks.Warehouse.Configuration.Carriers.DTO;
using ShipWorks.Warehouse.Configuration.Stores.DTO;
using ShipWorks.Warehouse.Configuration.TaxIdentifiers.DTO;

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

        /// <summary>
        /// The ShipEngine API key associated with this customer in the hub
        /// </summary>
        public string ShipEngineApiKey { get; set; }

        /// <summary>
        /// The ShipEngine Account ID associated with this customer in the hub
        /// </summary>
        public string ShipEngineAccountID { get; set; }

        /// <summary>
        /// The IOSS/OSS Tax Identifiers for the customer in the hub
        /// </summary>
        public List<TaxIdentifierConfiguration> TaxIdentifierConfigurations { get; set; }
    }
}
