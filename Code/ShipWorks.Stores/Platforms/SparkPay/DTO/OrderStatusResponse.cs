using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.SparkPay.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class OrderStatusResponse
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("order_statuses")]
        public List<OrderStatus> Statuses { get; set; }
    }
}
