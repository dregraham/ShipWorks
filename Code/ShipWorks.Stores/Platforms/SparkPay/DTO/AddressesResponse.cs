using Newtonsoft.Json;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.SparkPay.DTO
{
    public class AddressesResponse
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
        [JsonProperty("addresses")]
        public IList<Address> Addresses { get; set; }
    }
}
