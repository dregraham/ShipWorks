using System.Collections.Generic;
using Interapptive.Shared.Utility.Json;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class CustomAttribute : ICustomAttribute
    {
        [JsonProperty("attribute_code")]
        public string AttributeCode { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }
    }

    public class Product : IProduct
    {
        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("options")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IEnumerable<IProductOptionDetail>, List<ProductOptionDetail>>))]
        public IEnumerable<ProductOptionDetail> Options { get; set; }

        [JsonProperty("custom_attributes")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IEnumerable<ICustomAttribute>, List<CustomAttribute>>))]
        public IEnumerable<CustomAttribute> CustomAttributes { get; set; }
    }

}