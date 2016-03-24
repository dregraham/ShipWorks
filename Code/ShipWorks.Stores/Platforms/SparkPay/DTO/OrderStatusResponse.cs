using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.SparkPay.DTO
{
    public class OrderStatusResponse
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("order_statuses")]
        public List<OrderStatus> Statuses { get; set; }
    }
}
