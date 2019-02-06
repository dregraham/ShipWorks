using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Insure
{
    /// <summary>
    /// Response for a new_policy request
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class InsureShipNewPolicyResponse
    {
        /// <summary>
        /// Status of the response
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// ID of the new policy
        /// </summary>
        [JsonProperty("policy_id")]
        public long PolicyID { get; set; }

        /// <summary>
        /// Timestamp of the response
        /// </summary>
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}