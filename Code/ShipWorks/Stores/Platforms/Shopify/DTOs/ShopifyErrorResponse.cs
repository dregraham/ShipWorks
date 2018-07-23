using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Response from Shopify for errors
    /// </summary>
    [Obfuscation]
    public class ShopifyErrorResponse
    {
        /// <summary>
        /// Shop information
        /// </summary>
        [JsonProperty("errors")]
        public ShopifyError Errors { get; set; }
    }
}
