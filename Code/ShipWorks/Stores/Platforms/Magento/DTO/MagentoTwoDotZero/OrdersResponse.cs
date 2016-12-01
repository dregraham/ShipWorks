using System.Collections.Generic;
using Interapptive.Shared.Utility.Json;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    [Component]
    public class OrdersResponse : IOrdersResponse
    {
        [JsonProperty("items")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IEnumerable<IOrder>, List<Order>>))]
        public IEnumerable<IOrder> Orders { get; set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}