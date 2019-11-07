using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    /// <summary>
    /// Order entity returned by Rakuten
    /// </summary>
    public class RakutenOrder
    {
        /// <summary>
        /// The Rakuten order number
        /// </summary>
        [JsonProperty("orderNumber")]
        public string OrderNumber { get; set; }

        /// <summary>
        /// The Rakuten order status
        /// </summary>
        [JsonProperty("orderStatus")]
        public string OrderStatus { get; set; }

        /// <summary>
        /// The order total
        /// </summary>
        [JsonProperty("orderTotal")]
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// The last modified date
        /// </summary>
        [JsonProperty("lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        /// <summary>
        /// The date the order was created
        /// </summary>
        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Shopper comment or memo entered during checkout before the order was placed
        /// </summary>
        [JsonProperty("shopperComment")]
        public string ShopperComment { get; set; }

        /// <summary>
        /// Items contained in the order
        /// </summary>
        [JsonProperty("orderItems")]
        public IList<RakutenOrderItem> OrderItems { get; set; }

        /// <summary>
        /// The shipping information of the order
        /// </summary>
        [JsonProperty("shipping")]
        public RakutenShipment Shipping { get; set; }


    }
}