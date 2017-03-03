using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class Shipping
    {
        [JsonProperty("address")]
        public ShippingAddress Address { get; set; }
    }
}