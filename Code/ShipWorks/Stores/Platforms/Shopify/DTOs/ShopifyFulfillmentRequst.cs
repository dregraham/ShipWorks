using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// A shopify fulfillment request
    /// </summary>
    [Obfuscation]
    public class ShopifyFulfillmentRequest
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyFulfillmentRequest(ShopifyFulfillment fulfillment)
        {
            Fulfillment = fulfillment;
        }

        /// <summary>
        /// The fulfillment details
        /// </summary>
        [JsonProperty("fulfillment", NullValueHandling = NullValueHandling.Ignore)]
        public ShopifyFulfillment Fulfillment { get; }
    }
}