using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// ShipWorks error codes for handling Open Account exceptions
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsOpenAccountErrorCode
    {
        [Description("Unknown Error")]
        UnknownError = 0,

        [Description("Missing required field(s)")]
        MissingRequiredFields = 1,

        [Description("The request to open a new UPS account failed because pickup address is not valid for SMART Pickup")]
        SmartPickupError = 2,

        [Description("Account created but not registered.")]
        NotRegistered = 3

    }
}
