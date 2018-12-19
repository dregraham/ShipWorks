using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Claim
{
    /// <summary>
    /// Response for a get_claim_status request
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class InsureShipGetClaimStatusResponse
    {
        /// <summary>
        /// Status of the response
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}