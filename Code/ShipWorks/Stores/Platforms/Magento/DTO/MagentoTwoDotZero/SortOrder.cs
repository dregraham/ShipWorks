using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class SortOrder
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }
    }
}