using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class CustomOption : ICustomOption
    {
        [JsonProperty("optionId")]
        public string OptionId { get; set; }

        [JsonProperty("optionValue")]
        public string OptionValue { get; set; }

        [JsonProperty("extensionAttributes")]
        public IExtensionAttributes ExtensionAttributes { get; set; }
    }
}