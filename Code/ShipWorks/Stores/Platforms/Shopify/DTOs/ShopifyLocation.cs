using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Location of a shopify fulfillment center
    /// </summary>
    [Obfuscation]
    public class ShopifyLocation
    {
        /// <summary>
        /// ID of the location
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }
    }
}
