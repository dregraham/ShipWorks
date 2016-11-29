using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class ConfigurableItemOption
    {
        public ConfigurableItemOption(ExtensionAttributes extensionAttributes)
        {
            ExtensionAttributes = extensionAttributes;
        }

        [JsonProperty("optionId")]
        public string OptionId { get; set; }

        [JsonProperty("optionValue")]
        public int OptionValue { get; set; }

        [JsonProperty("extensionAttributes")]
        public IExtensionAttributes ExtensionAttributes { get; set; }
    }
}