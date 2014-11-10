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
        [Description("Missing required field(s)")]
        MissingRequiredFields = 0,

        [Description("The request to open a new UPS account failed because pickup address is not valid for SMART Pickup")]
        SmartPickupError = 1

    }
}
