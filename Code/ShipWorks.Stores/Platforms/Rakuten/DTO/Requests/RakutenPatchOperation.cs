using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO.Requests
{
    /// <summary>
    /// The request sent to Rakuten to mark an order as shipped and upload a tracking number
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenPatchOperation
    {
        [JsonProperty("op")]
        public string Operation { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("value")]
        public RakutenShippingInfo Value { get; set; }
    }

    /// <summary>
    /// The shipping information to send to Rakuten
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenShippingInfo
    {
        [JsonProperty("shippingStatus")]
        public string ShippingStatus { get; set; }

        [JsonProperty("carrierName")]
        public string CarrierName { get; set; }

        [JsonProperty("trackingNumber")]
        public string TrackingNumber { get; set; }
    }
}
