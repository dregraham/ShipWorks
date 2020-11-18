using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.Stores;

namespace ShipWorks.Warehouse.Configuration.Stores.DTO
{
    /// <summary>
    /// A store configuration downloaded from the Hub
    /// </summary>
    public class StoreConfiguration
    {
        /// <summary>
        /// The type code of this store
        /// </summary>
        public StoreTypeCode StoreType { get; set; }

        /// <summary>
        /// The name of this store
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The unique identifier of this store
        /// </summary>
        public string UniqueIdentifier { get; set; }

        /// <summary>
        /// Additional store-specific configuration data
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
