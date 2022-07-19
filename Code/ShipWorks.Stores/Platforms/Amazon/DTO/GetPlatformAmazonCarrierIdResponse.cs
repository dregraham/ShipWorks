using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Amazon.DTO
{
    [Obfuscation]
    public class GetPlatformAmazonCarrierIdResponse
    {
        [JsonProperty("carrierId")]
        public string CarrierId { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}