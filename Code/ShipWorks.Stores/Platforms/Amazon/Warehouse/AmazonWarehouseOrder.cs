using System;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Amazon.Warehouse
{
    /// <summary>
    /// Amazon warehouse order
    /// </summary>
    public class AmazonWarehouseOrder
    {
        /// <summary>
        /// The orders Amazon order ID
        /// </summary>
        [JsonProperty("amazonOrderId")]
        public string AmazonOrderID { get; set; }

        /// <summary>
        /// The fulfillment channel of the order
        /// </summary>
        public int FulfillmentChannel { get; set; }

        /// <summary>
        /// The orders Amazon Prime status
        /// </summary>
        public int IsPrime { get; set; }

        /// <summary>
        /// The Earliest Expected Delivery Date of the order
        /// </summary>
        public DateTime? EarliestExpectedDeliveryDate { get; set; }

        /// <summary>
        /// The Latest Expected Delivery Date of the order
        /// </summary>
        public DateTime? LatestExpectedDeliveryDate { get; set; }

        /// <summary>
        /// The Purchase Order Number of the order
        /// </summary>
        public string PurchaseOrderNumber { get; set; }
    }
}
