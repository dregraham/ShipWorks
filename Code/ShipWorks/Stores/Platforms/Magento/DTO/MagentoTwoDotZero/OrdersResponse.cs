using System.Collections.Generic;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class OrdersResponse : IOrdersResponse
    {
        [JsonProperty("items")]
        public IList<IOrder> Orders { get; set; }

        [JsonProperty("searchCriteria")]
        public SearchCriteria SearchCriteria { get; set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}