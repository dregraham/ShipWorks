using System.Collections.Generic;
using Interapptive.Shared.Utility.Json;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class Shipping : IShipping
    {
        [JsonProperty("address")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IEnumerable<IExtensionAttributes>, List<ExtensionAttributes>>))]
        public IShippingAddress Address { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("total")]
        public ITotal Total { get; set; }
    }
}