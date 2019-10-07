using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// DTO for an error from the Hub API
    /// </summary>
    public class HubApiError
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }
    }
}