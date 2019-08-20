using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.Warehouse
{
    /// <summary>
    /// Shopify Warehouse Item
    /// </summary>
    [Obfuscation]
    public class ShopifyWarehouseItem
    {
        /// <summary>
        /// The Shopify order item ID
        /// </summary>
        [JsonProperty("shopifyOrderItemId")]
        public long ShopifyOrderItemID { get; set; }

        /// <summary>
        /// The Shopify product ID
        /// </summary>
        [JsonProperty("shopifyProductId")]
        public long ShopifyProductID { get; set; }

        /// <summary>
        /// The Shopify inventory item ID
        /// </summary>
        [JsonProperty("shopifyInventoryItemId")]
        public long ShopifyInventoryItemID { get; set; }
    }
}
