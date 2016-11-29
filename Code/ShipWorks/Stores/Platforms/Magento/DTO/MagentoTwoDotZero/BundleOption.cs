using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class BundleOption
    {
        [JsonProperty("optionId")]
        public int OptionId { get; set; }

        [JsonProperty("optionQty")]
        public int OptionQty { get; set; }

        [JsonProperty("optionSelections")]
        public IList<int> OptionSelections { get; set; }

        [JsonProperty("extensionAttributes")]
        public ExtensionAttributes ExtensionAttributes { get; set; }
    }
}