using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class ProductOptionDetail : IProductOptionDetail
    {
        [JsonProperty("productsku")]
        public string ProductSku { get; set; }

        [JsonProperty("optionid")]
        public long OptionID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}