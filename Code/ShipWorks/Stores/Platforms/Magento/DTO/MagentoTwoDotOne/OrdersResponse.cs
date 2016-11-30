using System.Collections.Generic;
using Interapptive.Shared.Utility.Json;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class OrdersResponse : IOrdersResponse
    {
        [JsonProperty("items")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IEnumerable<IOrder>, List<Order>>))]
        public IEnumerable<IOrder> Orders { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
    }
}