using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// DTO for an error from the Hub API
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class HubApiError
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }
    }
}
