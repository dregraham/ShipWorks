using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    /// <summary>
    /// The response from Rakuten when shipping an order
    /// </summary>
    public class RakutenShipmentResponse
    {
        [JsonProperty("errors")]
        public RakutenErrors Errors { get; set; }
    }
}
