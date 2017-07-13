using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Fulfillment entity returned by ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorFulfillment
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("ProfileID")]
        public int ProfileID { get; set; }

        [JsonProperty("OrderID")]
        public int OrderID { get; set; }

        [JsonProperty("CreatedDateUtc")]
        public DateTime CreatedDateUtc { get; set; }

        [JsonProperty("UpdatedDateUtc")]
        public DateTime UpdatedDateUtc { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("DeliveryStatus")]
        public string DeliveryStatus { get; set; }

        [JsonProperty("TrackingNumber")]
        public object TrackingNumber { get; set; }

        [JsonProperty("ShippingCarrier")]
        public object ShippingCarrier { get; set; }

        [JsonProperty("ShippingClass")]
        public object ShippingClass { get; set; }

        [JsonProperty("DistributionCenterID")]
        public int DistributionCenterID { get; set; }

        [JsonProperty("ExternalFulfillmentCenterCode")]
        public object ExternalFulfillmentCenterCode { get; set; }

        [JsonProperty("ShippedDateUtc")]
        public object ShippedDateUtc { get; set; }

        [JsonProperty("SellerFulfillmentID")]
        public object SellerFulfillmentID { get; set; }

        [JsonProperty("Items")]
        public IList<ChannelAdvisorFulfillmentItem> Items { get; set; }
    }
}