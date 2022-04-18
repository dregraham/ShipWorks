using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Amazon.DTO
{
    [Obfuscation(Exclude = true)]
    public class GetMonauthInitiateUrlResponse
    {
        [JsonProperty("initiateUrl")]
        [Obfuscation(Exclude = true)]
        public string InitiateUrl { get; set; }
    }
}