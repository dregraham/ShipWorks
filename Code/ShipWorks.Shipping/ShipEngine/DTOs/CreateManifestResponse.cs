using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// DTO for the response from a CreateManifest request
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class CreateManifestResponse : BaseShipEngineResponse
    {
        [JsonProperty("errors")]
        public List<CreateManifestError> Errors { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class CreateManifestError : ShipEngineError
    {
        [JsonProperty("label_id")]
        public string LabelId { get; set; }
    }
}
