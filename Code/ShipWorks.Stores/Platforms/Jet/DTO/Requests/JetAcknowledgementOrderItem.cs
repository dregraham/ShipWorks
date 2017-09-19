using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO.Requests
{
    public class JetAcknowledgementOrderItem
    {
        [JsonProperty("order_item_acknowledgement_status")]
        public string OrderItemAcknowledgementStatus => "fulfillable";

        [JsonProperty("order_item_id")]
        public string OrderItemId { get; set; }

        [JsonProperty("alt_order_item_id")]
        public string AltOrderItemId { get; set; }
    }
}