using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.Warehouse
{
    /// <summary>
    /// Shopify Warehouse Order
    /// </summary>
    [Obfuscation]
    public class ShopifyWarehouseOrder
    {
        /// <summary>
        /// The Shopify order ID 
        /// </summary>
        [JsonProperty("shopifyOrderId")]
        public string ShopifyOrderID { get; set; }

        /// <summary>
        /// The fulfilment status code of the order
        /// </summary>
        public string FulfillmentStatus { get; set; }

        /// <summary>
        /// The payment status code of the order
        /// </summary>
        public string PaymentStatus { get; set; }

        /// <summary>
        /// The requested shipping code
        /// </summary>
        public string RequestedShippingCode { get; set; }

        /// <summary>
        /// The requested shipping title
        /// </summary>
        public string RequestedShippingTitle { get; set; }
    }
}
