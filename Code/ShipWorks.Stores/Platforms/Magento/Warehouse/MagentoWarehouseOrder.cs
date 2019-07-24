using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.Warehouse
{
    /// <summary>
    /// Magento warehouse order
    /// </summary>
    [Obfuscation]
    public class MagentoWarehouseOrder
    {
        /// <summary>
        /// The orders Magento order ID
        /// </summary>
        [JsonProperty("magentoOrderID")]
        public long MagentoOrderID { get; set; }
    }
}
