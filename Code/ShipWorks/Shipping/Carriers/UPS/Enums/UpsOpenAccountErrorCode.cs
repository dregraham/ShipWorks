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
        MissingRequiredFields = 0
    }
}
