using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.OnTrac.Enums
{
    /// <summary>
    /// Valid OnTrac package type values
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum OnTracPackagingType
    {
        [Description("Package")]
        [ApiValue("P")]
        Package = 0,

        [Description("Letter")]
        [ApiValue("L")]
        Letter = 1
    }
}