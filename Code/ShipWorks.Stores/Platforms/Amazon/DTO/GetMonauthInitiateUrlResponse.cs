using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Amazon.DTO
{
    [Obfuscation]
    public class GetMonauthInitiateUrlResponse
    {
        [JsonProperty("initiateUrl")]
        public string InitiateUrl { get; set; }
    }
}