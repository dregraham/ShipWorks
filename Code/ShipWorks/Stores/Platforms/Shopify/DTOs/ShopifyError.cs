using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Error when communicating with Shopify
    /// </summary>
    [Obfuscation]
    public class ShopifyError
    {
        /// <summary>
        /// Error with the order
        /// </summary>
        [JsonProperty("order")]
        public IEnumerable<string> Order { get; set; } = Enumerable.Empty<string>();

        /// <summary>
        /// Error with the line items
        /// </summary>
        [JsonProperty("line_items")]
        public IEnumerable<string> LineItems { get; set; } = Enumerable.Empty<string>();
    }
}