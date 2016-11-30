using Interapptive.Shared.Utility.Json;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class ConfigurableItemOption : IConfigurableItemOption
    {
        [JsonProperty("option_id")]
        public string OptionId { get; set; }

        [JsonProperty("option_value")]
        public int OptionValue { get; set; }

        [JsonProperty("extension_attributes")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IExtensionAttributes, ExtensionAttributes>))]
        public IExtensionAttributes ExtensionAttributes { get; set; }
}
}