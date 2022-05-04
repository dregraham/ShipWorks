using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// Request DTO for creating a manifest
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class CreateManifestRequest
    {
        public List<string> LabelIds { get; set; }
    }
}
