using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{ 
    /// <summary>
    /// Enum for license enforcement context
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EnforcementContext
    {
        [Description("Logging into ShipWorks")]
        Login = 0,

        [Description("Creating a label")]
        CreateLabel = 1,

        [Description("Downloading orders")]
        Download = 2
    }
}