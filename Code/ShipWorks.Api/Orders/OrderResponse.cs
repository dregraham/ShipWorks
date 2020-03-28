using System;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Api.Orders
{
    /// <summary>
    /// A minimal representation of an order in ShipWorks
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class OrderResponse
    {
        /// <summary>
        /// The ShipWorks order ID of the order
        /// </summary>
        [JsonProperty("orderId")]
        public long OrderId { get; set; }

        /// <summary>
        /// The order number of the order
        /// </summary>
        [JsonProperty("orderNumber")]
        public string OrderNumber { get; set; }

        /// <summary>
        /// The date and time that the order was placed
        /// </summary>
        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// The date and time that the order was last modified
        /// </summary>
        [JsonProperty("lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        /// <summary>
        /// The total of the order in USD
        /// </summary>
        [JsonProperty("orderTotal")]
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// The current status of the order
        /// </summary>
        [JsonProperty("storeStatus")]
        public string StoreStatus { get; set; }

        /// <summary>
        /// The shipping address of the order
        /// </summary>
        [JsonProperty("shipAddress")]
        public Address ShipAddress { get; set; }

        /// <summary>
        /// The billing address of the order
        /// </summary>
        [JsonProperty("billAddress")]
        public Address BillAddress { get; set; }
    }
}
