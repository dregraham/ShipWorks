using System;
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
        public new List<CreateManifestError> Errors { get; set; }

        [JsonProperty("manifests")]
        public List<CreateManifestManifest> Manifests { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class CreateManifestError : ShipEngineError
    {
        [JsonProperty("label_id")]
        public string LabelId { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class CreateManifestManifest
    {
        [JsonProperty("manifest_id")]
        public string ManifestId { get; set; }

        [JsonProperty("form_id")]
        public string FormId { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("ship_date")]
        public DateTime ShipDate { get; set; }

        [JsonProperty("shipments")]
        public int ShipmentCount { get; set; }

        [JsonProperty("warehouse_id")]
        public string PlatformWarehouseId { get; set; }

        [JsonProperty("submission_id")]
        public string SubmissionId { get; set; }

        [JsonProperty("carrier_id")]
        public string CarrierId { get; set; }

        [JsonProperty("manifest_download")]
        public CreateManifestManifestDownload ManifestDownload { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class CreateManifestManifestDownload
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }
}