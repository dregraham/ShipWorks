using System.Collections.Generic;
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
        /// <summary>
        /// Constructor
        /// </summary>
        public HubApiError()
        {
            Errors = new List<string>();
        }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("errors")]
        public List<string> Errors { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
