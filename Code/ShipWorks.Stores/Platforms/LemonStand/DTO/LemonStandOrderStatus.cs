using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.LemonStand.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class LemonStandOrderStatus
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}