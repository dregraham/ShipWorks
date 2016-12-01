using System.Collections.Generic;
using Interapptive.Shared.Utility.Json;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class ShippingAssignment : IShippingAssignment
    {
        [JsonProperty("shipping")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IShipping, Shipping>))]
        public IShipping Shipping { get; set; }

        [JsonProperty("items")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IEnumerable<IItem>, List<Item>>))]
        public IEnumerable<IItem> Items { get; set; }
    }
}