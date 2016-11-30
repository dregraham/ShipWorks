using System.Collections.Generic;
using Interapptive.Shared.Utility.Json;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class OrdersResponse : IOrdersResponse
    {
        [JsonProperty("items")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IEnumerable<IOrder>, List<Order>>))]
        public IEnumerable<IOrder> Orders { get; set; }

        [JsonProperty("searchCriteria")]
        public SearchCriteria SearchCriteria { get; set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}