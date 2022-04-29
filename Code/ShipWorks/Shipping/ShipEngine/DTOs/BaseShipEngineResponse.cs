using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// The base class for responses from ShipEngine
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class BaseShipEngineResponse
    {
        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        [JsonProperty("errors")]
        public List<ShipEngineError> Errors { get; set; }
    }

    public class ShipEngineError
    {
        [JsonProperty("error_source")]
        public string ErrorSource { get; set; }

        [JsonProperty("error_type")]
        public string ErrorType { get; set; }

        [JsonProperty("error_code")]
        public string ErrorCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
