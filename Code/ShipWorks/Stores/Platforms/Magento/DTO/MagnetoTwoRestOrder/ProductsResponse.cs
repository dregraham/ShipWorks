using Newtonsoft.Json;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class ProductsResponse
    {
        [JsonProperty("items")]
        public IEnumerable<Product> Products { get; set; }
    }
}
