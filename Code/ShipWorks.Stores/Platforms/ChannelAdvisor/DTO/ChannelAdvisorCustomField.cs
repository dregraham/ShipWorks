using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Custom Field entity returned by ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorCustomField
    {
        [JsonProperty("FieldID")]
        public int FieldID { get; set; }

        [JsonProperty("OrderID")]
        public int OrderID { get; set; }

        [JsonProperty("ProfileID")]
        public int ProfileID { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}