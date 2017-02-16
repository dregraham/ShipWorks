using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class Product
    {
        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("options")]
        public IEnumerable<ProductOptionDetail> Options { get; set; }
    }
}