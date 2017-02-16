using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoRestShipment
{
    public class Comment
    {
        [JsonProperty("comment")]
        public string Text { get; set; }
    }
}