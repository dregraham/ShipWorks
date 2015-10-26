using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.LemonStand.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class LemonStandOrderStatuses
    {
        [JsonProperty("data")]
        public List<LemonStandOrderStatus> OrderStatus { get; set; }
    }
}