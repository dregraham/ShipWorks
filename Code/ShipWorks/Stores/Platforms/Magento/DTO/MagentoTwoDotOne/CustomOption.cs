using Interapptive.Shared.Utility.Json;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class CustomOption : ICustomOption
    {
        [JsonProperty("option_id")]
        public long OptionID { get; set; }

        [JsonProperty("option_value")]
        public string OptionValue { get; set; }

        [JsonProperty("extension_attributes")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IExtensionAttributes, ExtensionAttributes>))]
        public IExtensionAttributes ExtensionAttributes { get; set; }
    }
}