using System.Collections.Generic;
using Interapptive.Shared.Utility.Json;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class BundleOption : IBundleOption
    {
        [JsonProperty("option_id")]
        public int OptionId { get; set; }

        [JsonProperty("option_qty")]
        public int OptionQty { get; set; }

        [JsonProperty("option_selections")]
        public IEnumerable<int> OptionSelections { get; set; }

        [JsonProperty("extension_attributes")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IExtensionAttributes, ExtensionAttributes>))]
        public IExtensionAttributes ExtensionAttributes { get; set; }
    }
}