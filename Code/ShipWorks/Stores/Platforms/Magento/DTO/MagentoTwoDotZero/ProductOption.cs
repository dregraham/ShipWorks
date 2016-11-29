using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class ProductOption
    {

        [JsonProperty("extensionAttributes")]
        public ExtensionAttributes ExtensionAttributes { get; set; }
    }
}