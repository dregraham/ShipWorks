using Newtonsoft.Json;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class ProductOptionDetail
    {
        [JsonProperty("product_sku")]
        public string ProductSku { get; set; }

        [JsonProperty("option_id")]
        public string OptionID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("values")]
        public IEnumerable<ProductOptionValue> Values {get;set;}
    }
}