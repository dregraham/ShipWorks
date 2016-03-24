using Newtonsoft.Json;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.SparkPay.DTO
{
    public class OrdersResponse
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("orders")]
        public List<Order> Orders { get; set; }
    }
}
