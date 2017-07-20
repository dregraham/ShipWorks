using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Product image entity returned by ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorProductImage
    {
        [JsonProperty("ProductID")]
        public int ProductID { get; set; }

        [JsonProperty("ProfileID")]
        public int ProfileID { get; set; }

        [JsonProperty("PlacementName")]
        public string PlacementName { get; set; }

        [JsonProperty("Abbreviation")]
        public string Abbreviation { get; set; }

        [JsonProperty("Url")]
        public string Url { get; set; }
    }
}