using System.Collections.Generic;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class OrdersResponse : IOrdersResponse
    {
        [JsonProperty("items")]
        public IList<IOrder> Orders { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
    }
}