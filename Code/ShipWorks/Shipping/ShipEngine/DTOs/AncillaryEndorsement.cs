using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// Ancillary endorsement setting for ShipEngine
    /// </summary>
    [Obfuscation(ApplyToMembers = true, Exclude = true, StripAfterObfuscation = false)]
    public enum AncillaryEndorsement
    {
        [ApiValue("None")]
        [Description("None")]
        None = 0,

        [ApiValue("AddressServiceRequested")]
        [Description("Address Service Requested")]
        AddressServiceRequested = 1,

        [ApiValue("ForwardingServiceRequested")]
        [Description("Forwarding Service Requested")]
        ForwardingServiceRequested = 2,

        [ApiValue("ChangeServiceRequested")]
        [Description("Change Service Requested")]
        ChangeServiceRequested = 3,

        [ApiValue("ReturnServiceRequested")]
        [Description("Return Service Requested")]
        ReturnServiceRequested = 4,
    }
}
