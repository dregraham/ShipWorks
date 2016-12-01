using Interapptive.Shared.Utility.Json;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class ProductOption : IProductOption
    {
        [JsonProperty("extension_attributes")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IExtensionAttributes, ExtensionAttributes>))]
        public IExtensionAttributes ExtensionAttributes { get; set; }
    }
}