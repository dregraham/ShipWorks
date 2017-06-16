using System.Collections.Generic;
using Newtonsoft.Json;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    [Component]
    public class OrdersResponse
    {
        [JsonProperty("items")]
        public IEnumerable<Order> Orders { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
    }
}