using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Variant of a ShopifyProduct
    /// </summary>
    [Obfuscation]
    public class ShopifyProductVariant
    {
        /// <summary>
        /// ID of the product variant
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        /// ID of the inventory item
        /// </summary>
        [JsonProperty("inventory_item_id")]
        public long InventoryItemID { get; set; }

        /// <summary>
        /// Barcode
        /// </summary>
        [JsonProperty("barcode")]
        public string Barcode { get; set; } = string.Empty;
    }
}