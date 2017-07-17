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
        public decimal Amount { get; set; }

        [JsonProperty("ShippingAmount")]
        public decimal ShippingAmount { get; set; }
    }
}