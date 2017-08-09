using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO.Requests
{
    public class JetShipmentRequest
    {
        [JsonProperty("alt_order_id")]
        public string AltOrderId{ get; set; }

        [JsonProperty("shipments")]
        public List<JetShipment> Shipments { get; set; }
    }
}