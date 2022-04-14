using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Amazon.DTO
{
    public class GetMonauthInitiateUrlResponse
    {
        [JsonProperty("initiateUrl")]
        public string InitiateUrl { get; set; }
    }
}