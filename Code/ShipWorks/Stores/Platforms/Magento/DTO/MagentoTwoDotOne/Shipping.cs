using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class Shipping : IShipping
    {
        public Shipping(ShippingAddress address, Total total)
        {
            Address = address;
            Total = total;
        }

        [JsonProperty("address")]
        public IShippingAddress Address { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("total")]
        public ITotal Total { get; set; }
    }
}