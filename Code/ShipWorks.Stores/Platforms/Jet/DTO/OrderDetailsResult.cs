using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO
{
    public class OrderDetailsResult
    {
        [JsonProperty("merchant_order_id")]
        public string MerchantOrderId { get; set; }

        [JsonProperty("reference_order_id")]
        public string ReferenceOrderId { get; set; }

        [JsonProperty("order_placed_date")]
        public DateTime OrderPlacedDate { get; set; }

        [JsonProperty("order_ready_date")]
        public DateTime OrderReadyDate { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("hash_email")]
        public string HashEmail { get; set; }

        [JsonProperty("buyer")]
        public JetBuyer Buyer { get; set; }

        [JsonProperty("shipping_to")]
        public JetShippingTo ShippingTo { get; set; }

        [JsonProperty("order_totals")]
        public JetOrderTotals OrderTotals { get; set; }

        [JsonProperty("order_items")]
        public IList<JetOrderItem> OrderItems { get; set; }

        [JsonProperty("order_detail")]
        public JetOrderDetail OrderDetail { get; set; }
    }
}