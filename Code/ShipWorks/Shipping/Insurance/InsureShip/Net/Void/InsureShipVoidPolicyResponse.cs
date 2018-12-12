﻿using Newtonsoft.Json;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Void
{
    /// <summary>
    /// Response for a void_policy request
    /// </summary>
    public class InsureShipVoidPolicyResponse
    {
        /// <summary>
        /// Status of the response
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}