using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ShipWorks.Warehouse.DTO.Orders
{
    /// <summary>
    /// Response from uploading an order
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class WarehouseUploadOrderResponse
    {
        /// <summary>
        /// The sequence number of the order's most recent event log entry
        /// </summary>
        [JsonProperty("sequence")]
        public long HubSequence { get; set; }

        /// <summary>
        /// The assigned guid
        /// </summary>
        [JsonProperty("id")]
        public string HubOrderID { get; set; }
    }
}