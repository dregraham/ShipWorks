using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class ProductOptionDetail
    {
        [JsonProperty("product_sku")]
        public string ProductSku { get; set; }

        [JsonProperty("option_id")]
        public long OptionID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}