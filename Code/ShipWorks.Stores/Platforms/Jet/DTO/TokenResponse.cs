using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO
{
    public class TokenResponse
    {
        [JsonProperty("id_token")]
        public string Token { get; set; }
    }
}
