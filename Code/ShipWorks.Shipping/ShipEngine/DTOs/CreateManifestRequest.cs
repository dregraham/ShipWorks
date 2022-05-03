using System.Collections.Generic;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// Request DTO for creating a manifest
    /// </summary>
    public class CreateManifestRequest
    {
        public List<string> LabelIds { get; set; }
    }
}
