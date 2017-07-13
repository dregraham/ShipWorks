using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Promotion entity returned by ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorPromotion
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Amount")]
        public int Amount { get; set; }

        [JsonProperty("ShippingAmount")]
        public int ShippingAmount { get; set; }
    }
}