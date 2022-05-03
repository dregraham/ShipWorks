using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// DTO for the response from a CreateManifest request
    /// </summary>
    public class CreateManifestResponse : BaseShipEngineResponse
    {
        [JsonProperty("errors")]
        public List<CreateManifestError> Errors { get; set; }
    }

    public class CreateManifestError : ShipEngineError
    {
        [JsonProperty("label_id")]
        public string LabelId { get; set; }
    }
}
