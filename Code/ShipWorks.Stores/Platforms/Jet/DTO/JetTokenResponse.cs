using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO
{
    public class JetTokenResponse
    {
        [JsonProperty("id_token")]
        public string Token { get; set; }
    }
}
