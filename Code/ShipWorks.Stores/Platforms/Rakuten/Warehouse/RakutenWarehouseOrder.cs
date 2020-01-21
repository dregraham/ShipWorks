using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.Warehouse
{
    /// <summary>
    /// Rakuten warehouse order
    /// </summary>
    [Obfuscation]
    public class RakutenWarehouseOrder
    {
        /// <summary>
        /// Rakuten Package ID for shipping
        /// </summary>
        [JsonProperty("packageId")]
        public string PackageID { get; set; }
    }
}
