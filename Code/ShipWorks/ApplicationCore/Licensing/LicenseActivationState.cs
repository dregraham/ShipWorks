using Interapptive.Shared.Utility;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// The various states of activation a license can be in
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum LicenseActivationState
    {
        [Description("An unknown error has occurred while comunicating with ShipWorks.")]
        [ApiValue("-1")]
        UnknownError = -1,

        [Description("Active")]
        [ApiValue("0")]
        Active = 0,

        [Description("ShipWorks could not find your customer account.")]
        [ApiValue("1")]
        CustIdNotFound = 1,

        [Description("Your account has been closed.")]
        [ApiValue("2")]
        CustIdClosed = 2,

        [Description("Your account has expired.")]
        [ApiValue("3")]
        CustIdExpired = 3,

        [Description("Your account has been disabled.")]
        [ApiValue("3")]
        CustIdDisabled = 4,

        [Description("You have exceeded your channel limit.")]
        [ApiValue("5")]
        MaxChannelsExceeded = 5,

        // The rest of the enums where already defined and used for store licenses
        ActiveElsewhere = 6,
        ActiveNowhere = 7,
        Deactivated = 8,
        Canceled = 9,
        Invalid = 10
    }
}


