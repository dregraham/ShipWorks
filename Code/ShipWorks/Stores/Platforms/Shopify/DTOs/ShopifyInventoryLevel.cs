using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Inventory level for a Shopify product
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class ShopifyInventoryLevel
    {
        /// <summary>
        /// The Available
        /// </summary>
        [JsonProperty("available", DefaultValueHandling = DefaultValueHandling.Populate)]
        public long? Available { get; set; }

        /// <summary>
        /// The LocationID
        /// </summary>
        [JsonProperty("location_id", DefaultValueHandling = DefaultValueHandling.Populate)]
        public long LocationID { get; set; }

        /// <summary>
        /// The InventoryItemID
        /// </summary>
        [JsonProperty("inventory_item_id", DefaultValueHandling = DefaultValueHandling.Populate)]
        public long InventoryItemID { get; set; }
    }
}
