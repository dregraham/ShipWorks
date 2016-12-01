using Interapptive.Shared.Utility.Json;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class Shipping : IShipping
    {
        [JsonProperty("address")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IShippingAddress, ShippingAddress>))]
        public IShippingAddress Address { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("total")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<ITotal, Total>))]
        public ITotal Total { get; set; }
    }
}