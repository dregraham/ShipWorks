using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO
{
    public class JetShipResponse
    {
        [JsonProperty("errors")]
        public string[] Errors { get; set; }
    }
}