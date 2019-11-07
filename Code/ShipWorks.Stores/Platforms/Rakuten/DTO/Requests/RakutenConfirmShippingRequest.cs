using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO.Requests
{
    /// <summary>
    /// The request sent to Rakuten to mark an order as shipped and upload a tracking number
    /// </summary>
    public class RakutenConfirmShippingRequest
    {
        public IList<RakutenPatchOperation> Operations { get; set; }
    }

    /// <summary>
    /// The operation to perform
    /// </summary>
    public class RakutenPatchOperation
    {
        [JsonProperty("op")]
        public string OP { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("value")]
        public RakutenShippingInfo Value { get; set; }
    }

    /// <summary>
    /// The shipping information to send to Rakuten
    /// </summary>
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
