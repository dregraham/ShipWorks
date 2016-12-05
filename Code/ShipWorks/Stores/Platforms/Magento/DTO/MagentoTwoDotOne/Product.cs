using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class CustomAttribute
    {
        [JsonProperty("attribute_code")]
        public string AttributeCode { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }
    }

    public class Product
    {
        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("options")]
        public IEnumerable<ProductOptionDetail> Options { get; set; }

        [JsonProperty("custom_attributes")]
        public IEnumerable<CustomAttribute> CustomAttributes { get; set; }
    }
}