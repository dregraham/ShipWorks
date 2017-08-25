using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO.Requests
{
    public class JetAcknowledgementRequest
    {
        [JsonProperty("acknowledgement_status")]
        public string AcknowledgementStatus => "accepted";

        [JsonProperty("alt_order_id")]
        public string AltOrderId { get; set; }

        [JsonProperty("order_items")]
        public IList<JetAcknowledgementOrderItem> OrderItems { get; set; }
    }
}