using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Line item for a Shopify fulfillment
    /// </summary>
    [Obfuscation]
    public class ShopifyFulfillmentLineItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyFulfillmentLineItem(long id)
        {
            ID = id;
        }

        /// <summary>
        /// ID of the line item
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; }
    }
}