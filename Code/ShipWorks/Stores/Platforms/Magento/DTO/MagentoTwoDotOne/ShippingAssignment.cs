using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class ShippingAssignment
    {
        [JsonProperty("shipping")]
        public Shipping Shipping { get; set; }

        [JsonProperty("items")]
        public IEnumerable<Item> Items { get; set; }
    }
}