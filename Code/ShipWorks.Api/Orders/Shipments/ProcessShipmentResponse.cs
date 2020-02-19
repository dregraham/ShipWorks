using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Api.Orders.Shipments
{
    /// <summary>
    /// Response containing the shipment and supporting metadata
    /// </summary>
    public class ProcessShipmentResponse
    {
        /// <summary>
        /// A collection of label names and Base64 encoded labels
        /// </summary>
        [JsonProperty("labels")]
        public IEnumerable<LabelData> Labels { get; set; }

        /// <summary>
        /// The cost of the shipment
        /// </summary>
        [JsonProperty("cost")]
        public decimal Cost { get; set; }

        /// <summary>
        /// The carrier for the Shipment
        /// </summary>
        [JsonProperty("carrier")]
        public string Carrier { get; set; }

        /// <summary>
        /// The service for the Shipment
        /// </summary>
        [JsonProperty("service")]
        public string Service { get; set; }

        /// <summary>
        /// The tracking number for the Shipment
        /// </summary>
        [JsonProperty("tracking")]
        public string Tracking { get; set; }
    }
}
