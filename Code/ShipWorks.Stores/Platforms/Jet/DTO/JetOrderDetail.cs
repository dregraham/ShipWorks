using System;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO
{
    public class JetOrderDetail
    {
        [JsonProperty("request_shipping_carrier")]
        public string RequestShippingCarrier { get; set; }

        [JsonProperty("request_shipping_method")]
        public string RequestShippingMethod { get; set; }

        [JsonProperty("request_service_level")]
        public string RequestServiceLevel { get; set; }

        [JsonProperty("request_ship_by")]
        public DateTime RequestShipBy { get; set; }

        [JsonProperty("request_delivery_by")]
        public DateTime RequestDeliveryBy { get; set; }
    }
}