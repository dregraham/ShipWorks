using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class BundleOption
    {
        [JsonProperty("option_id")]
        public int OptionId { get; set; }

        [JsonProperty("option_qty")]
        public int OptionQty { get; set; }

        [JsonProperty("option_selections")]
        public IEnumerable<int> OptionSelections { get; set; }

        [JsonProperty("extension_attributes")]
        public ExtensionAttributes ExtensionAttributes { get; set; }
    }
}