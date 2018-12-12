using Newtonsoft.Json;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Claim
{
    /// <summary>
    /// Response for a submit_claim request
    /// </summary>
    public class InsureShipSubmitClaimResponse
    {
        /// <summary>
        /// Status of the response
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// ID of the claim
        /// </summary>
        [JsonProperty("claim_id")]
        public long ClaimID { get; set; }
    }
}