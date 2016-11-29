using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class ConfigurableItemOption
    {
        [JsonProperty("optionId")]
        public string OptionId { get; set; }

        [JsonProperty("optionValue")]
        public int OptionValue { get; set; }

        [JsonProperty("extensionAttributes")]
        public ExtensionAttributes ExtensionAttributes { get; set; }
    }
}