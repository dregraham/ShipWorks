using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Void
{
    /// <summary>
    /// Response for a void_policy request
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class InsureShipVoidPolicyResponse
    {
        /// <summary>
        /// Status of the response
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}