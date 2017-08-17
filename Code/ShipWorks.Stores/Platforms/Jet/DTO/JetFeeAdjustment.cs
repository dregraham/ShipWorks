using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO
{
    public class JetFeeAdjustment
    {
        [JsonProperty("adjustment_name")]
        public string AdjustmentName { get; set; }

        [JsonProperty("adjustment_type")]
        public string AdjustmentType { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }
}