using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Product distribution center quantity entity returned by ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorProductDistributionCenterQuantity
    {
        [JsonProperty("ProductID")]
        public int ProductID { get; set; }

        [JsonProperty("ProfileID")]
        public int ProfileID { get; set; }

        [JsonProperty("DistributionCenterID")]
        public int DistributionCenterID { get; set; }

        [JsonProperty("AvailableQuantity")]
        public int AvailableQuantity { get; set; }
    }
}