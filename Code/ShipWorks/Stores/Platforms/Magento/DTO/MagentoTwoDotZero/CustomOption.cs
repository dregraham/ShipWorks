using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class CustomOption
    {
        [JsonProperty("optionId")]
        public string OptionId { get; set; }

        [JsonProperty("optionValue")]
        public string OptionValue { get; set; }

        [JsonProperty("extensionAttributes")]
        public ExtensionAttributes ExtensionAttributes { get; set; }
    }
}