using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Response from Shopify for shop information
    /// </summary>
    [Obfuscation]
    public class ShopifyShopResponse
    {
        /// <summary>
        /// Shop information
        /// </summary>
        [JsonProperty("shop")]
        public ShopifyShop Shop { get; set; }
    }
}
