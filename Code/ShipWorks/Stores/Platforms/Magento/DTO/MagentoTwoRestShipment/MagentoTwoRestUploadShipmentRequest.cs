using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoRestShipment
{
    public class MagentoTwoRestUploadShipmentRequest
    {
        [JsonProperty("items")]
        public IList<ShipmentItem> Items { get; set; }

        [JsonProperty("notify")]
        public bool Notify { get; set; }

        [JsonProperty("appendComment")]
        public bool AppendComment { get; set; }

        [JsonProperty("comment")]
        public Comment Comment { get; set; }

        [JsonProperty("tracks")]
        public IList<Track> Tracks { get; set; }
    }
}