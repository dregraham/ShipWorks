using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class ProductOptionValue
    {
        [JsonProperty("option_type_id")]
        public int? OptionTypeID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
