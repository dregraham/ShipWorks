using Interapptive.Shared.Utility;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Licensing
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AssociateShipWorksWithItselfResponseType
    {
        [Description("Success")]
        Success = 0,

        [Description("Unknown")]
        UnknownError = 1,

        [Description("PO Box Not Allowed")]
        [ApiValue("100")]
        POBoxNotAllowed = 2,

        [Description("ShipWorks could not validate this address. This usually means additional information is needed.")]
        AddressValidationFailed = 3,
    }
}