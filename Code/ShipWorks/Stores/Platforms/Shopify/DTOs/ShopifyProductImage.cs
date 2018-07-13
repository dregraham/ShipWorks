using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Image for a Shopify product
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class ShopifyProductImage
    {
        /// <summary>
        /// IDs of the variants to which the image applies
        /// </summary>
        [JsonProperty("variant_ids")]
        public long[] VariantIDs { get; set; } = new long[0];

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("src")]
        public string Src { get; set; } = string.Empty;
    }
}