using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO
{
    public class ShipmentItem
    {
        [JsonProperty("orderItemId")]
        public long OrderItemId { get; set; }

        [JsonProperty("qty")]
        public double Qty { get; set; }
    }

    public class Comment
    {
        [JsonProperty("comment")]
        public string Text { get; set; }

        [JsonProperty("isVisibleOnFront")]
        public int IsVisibleOnFront { get; set; }
    }

    public class Track
    {
        [JsonProperty("trackNumber")]
        public string TrackNumber { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("carrierCode")]
        public string CarrierCode { get; set; }
    }

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