using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.SparkPay.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public class OrdersResponse
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("orders")]
        public List<Order> Orders { get; set; }
    }
}
