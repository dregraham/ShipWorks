using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{
    /// <summary>
    /// Enum for the EnforcerPriority
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EnforcerPriority
    {
        [Description("Medium priority and should be performed last.")]
        Low = 0,

        [Description("Medium priority and should be performed before low priority.")]
        Medium = 1,

        [Description("Highest priority and should be performed first.")]
        High = 2,
    }
}