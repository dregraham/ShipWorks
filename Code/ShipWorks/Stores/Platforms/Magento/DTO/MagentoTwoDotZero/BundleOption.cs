using System.Collections.Generic;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class BundleOption : IBundleOption
    {
        [JsonProperty("optionId")]
        public int OptionId { get; set; }

        [JsonProperty("optionQty")]
        public int OptionQty { get; set; }

        [JsonProperty("optionSelections")]
        public IEnumerable<int> OptionSelections { get; set; }

        [JsonProperty("extensionAttributes")]
        public IExtensionAttributes ExtensionAttributes { get; set; }
    }
}