using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class ConfigurableItemOption
    {
        [JsonProperty("option_id")]
        public string OptionId { get; set; }

        [JsonProperty("option_value")]
        public int OptionValue { get; set; }

        [JsonProperty("extension_attributes")]
        public ExtensionAttributes ExtensionAttributes { get; set; }
    }
}