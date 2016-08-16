using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.LemonStand.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class LemonStandCustomer
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}