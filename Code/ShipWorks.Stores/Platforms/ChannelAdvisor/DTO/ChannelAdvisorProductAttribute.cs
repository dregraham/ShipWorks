using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Product attribute entity returned by ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorProductAttribute
    {
        [JsonProperty("ProductID")]
        public int ProductID { get; set; }

        [JsonProperty("ProfileID")]
        public int ProfileID { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}