using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Product for a shopify order item
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class ShopifyProduct
    {
        /// <summary>
        /// Product variants
        /// </summary>
        [JsonProperty("variants")]
        public ShopifyProductVariant[] Variants { get; set; } = new ShopifyProductVariant[0];

        /// <summary>
        /// Default product image
        /// </summary>
        [JsonProperty("image")]
        public ShopifyProductImage Image { get; set; }

        /// <summary>
        /// Product images
        /// </summary>
        [JsonProperty("images")]
        public ShopifyProductImage[] Images { get; set; } = new ShopifyProductImage[0];
    }
}
