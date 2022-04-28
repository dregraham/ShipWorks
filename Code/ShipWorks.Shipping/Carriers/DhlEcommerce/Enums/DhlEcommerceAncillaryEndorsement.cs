using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce.Enums
{
    /// <summary>
    /// Ancillary endorsement setting for DHL eCommerce accounts
    /// </summary>
    [Obfuscation(ApplyToMembers = true, Exclude = true, StripAfterObfuscation = false)]
    public enum DhlEcommerceAncillaryEndorsement
    {
        [ApiValue("none")]
        [Description("None")]
        None = 0,

        [ApiValue("address_service_requested")]
        [Description("Address Service Requested")]
        AddressServiceRequested = 1,

        [ApiValue("forwarding_service_requested")]
        [Description("Forwarding Service Requested")]
        ForwardingServiceRequested = 2,

        [ApiValue("change_service_requested")]
        [Description("Change Service Requested")]
        ChangeServiceRequested = 3,

        [ApiValue("return_service_requested")]
        [Description("Return Service Requested")]
        ReturnServiceRequested = 4,
    }
}
