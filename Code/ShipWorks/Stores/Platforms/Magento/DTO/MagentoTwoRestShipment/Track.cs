using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoRestShipment
{
    public class Track
    {
        [JsonProperty("trackNumber")]
        public string TrackNumber { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("carrierCode")]
        public string CarrierCode { get; set; }
    }
}