using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Shopify get orders result DTO
    /// </summary>
    public class ShopifyWebClientGetOrdersResult
    {
        /// <summary>
        /// List of JToken Shopify orders
        /// </summary>
        public IEnumerable<JToken> Orders { get; set; }

        /// <summary>
        /// URL of the next page of orders to download
        /// </summary>
        public string NextPageUrl { get; set; }
    }
}
