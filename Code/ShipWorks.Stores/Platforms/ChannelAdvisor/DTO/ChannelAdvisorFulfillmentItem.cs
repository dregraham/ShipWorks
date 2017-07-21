using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Fulfillment Item entity returned by ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorFulfillmentItem
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("ProfileID")]
        public int ProfileID { get; set; }

        [JsonProperty("FulfillmentID")]
        public int FulfillmentID { get; set; }

        [JsonProperty("OrderID")]
        public int OrderID { get; set; }

        [JsonProperty("OrderItemID")]
        public int OrderItemID { get; set; }

        [JsonProperty("Quantity")]
        public int Quantity { get; set; }

        [JsonProperty("ProductID")]
        public int ProductID { get; set; }
    }
}