using System;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Groupon.DTO
{
    public class GrouponTracking
    {
        public GrouponTracking(string Carrier, Int64 CILineItemID, string Tracking)
        {
            this.Carrier = Carrier;
            this.GrouponLineitemId = CILineItemID;
            this.Tracking = Tracking;

        }

        [JsonProperty("carrier")]
        public string Carrier { get; set; }

        [JsonProperty("ci_lineitem_id")]
        public Int64 GrouponLineitemId { get; set; }

        [JsonProperty("tracking")]
        public string Tracking { get; set; }

    }
}
