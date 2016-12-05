using System.Collections.Generic;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
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