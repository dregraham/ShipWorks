using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Line item for a Shopify order
    /// </summary>
    [Obfuscation]
    public class ShopifyLineItem
    {
        /// <summary>
        /// ID of the line item
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        /// ID of the product variant
        /// </summary>
        [JsonProperty("variant_id")]
        public long VariantID { get; set; }
    }
}