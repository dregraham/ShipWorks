using System;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Ship Request to send to ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorShipment
    {
        [JsonProperty("ShippedDateUtc")]
        public DateTime ShippedDateUtc { get; set; }

        [JsonProperty("TrackingNumber")]
        public string TrackingNumber { get; set; }

        [JsonProperty("SellerFulfillmentID")]
        public string SellerFulfillmentID { get; set; }

        [JsonProperty("DistributionCenterID")]
        public int DistributionCenterID { get; set; }

        [JsonProperty("DeliveryStatus")]
        public string DeliveryStatus { get; set; }

        [JsonProperty("ShippingCarrier")]
        public string ShippingCarrier { get; set; }

        [JsonProperty("ShippingClass")]
        public string ShippingClass { get; set; }
    }
}