using System.Collections.Generic;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class ShippingAssignment : IShippingAssignment
    {
        public ShippingAssignment(Shipping shipping, IEnumerable<Item> items)
        {
            Shipping = shipping;
            Items = items;
        }
        [JsonProperty("shipping")]
        public IShipping Shipping { get; set; }

        [JsonProperty("items")]
        public IEnumerable<IItem> Items { get; set; }
    }
}