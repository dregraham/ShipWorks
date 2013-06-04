using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.OnTrac.Enums
{
    /// <summary>
    /// Valid OnTrac cod type values
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum OnTracCodType
    {
        [Description("None")]
        [ApiValue("NONE")]
        None = 0,

        [Description("Unsecured funds")]
        [ApiValue("UNSECURED")]
        UnsecuredFunds = 1,

        [Description("Secured")]
        [ApiValue("SECURED")]
        SecuredFunds = 2
    }
}