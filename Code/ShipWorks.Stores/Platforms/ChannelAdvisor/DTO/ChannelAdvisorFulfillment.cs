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
        public DateTimeOffset CreatedDateUtc { get; set; }

        [JsonProperty("UpdatedDateUtc")]
        public DateTimeOffset UpdatedDateUtc { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("DeliveryStatus")]
        public string DeliveryStatus { get; set; }

        [JsonProperty("TrackingNumber")]
        public string TrackingNumber { get; set; }

        [JsonProperty("ShippingCarrier")]
        public string ShippingCarrier { get; set; }

        [JsonProperty("ShippingClass")]
        public string ShippingClass { get; set; }

        [JsonProperty("DistributionCenterID")]
        public int DistributionCenterID { get; set; }

        [JsonProperty("ExternalFulfillmentCenterCode")]
        public string ExternalFulfillmentCenterCode { get; set; }

        [JsonProperty("ShippedDateUtc")]
        public DateTimeOffset ShippedDateUtc { get; set; }

        [JsonProperty("SellerFulfillmentID")]
        public string SellerFulfillmentID { get; set; }

        [JsonProperty("Items")]
        public IList<ChannelAdvisorFulfillmentItem> Items { get; set; }
    }
}