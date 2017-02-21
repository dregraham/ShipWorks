using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class ParentItem
    {
        [JsonProperty("price")]
        public double Price { get; set; }
    }
}