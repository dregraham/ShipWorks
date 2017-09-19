using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO
{
    public class JetOrderResponse
    {
        [JsonProperty("order_urls")]
        public string[] OrderUrls { get; set; }
    }
}
