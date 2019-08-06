using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ShipWorks.Warehouse.DTO.Orders
{
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