using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// UPS OpenAccount response status codes
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsOpenAccountResponseStatusCode
    {
        [Description("Failed")]
        [ApiValueAttribute("0")]
        Failed = 0,

        [Description("Success")]
        [ApiValueAttribute("1")]
        Success = 1
    }
}
