using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.SparkPay.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class AddressesResponse
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("addresses")]
        public IList<Address> Addresses { get; set; }
    }
}
