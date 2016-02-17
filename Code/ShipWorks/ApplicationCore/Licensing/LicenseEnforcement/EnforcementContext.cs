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
        [Description("No specific context")]
        NotSpecified = 0,

        [Description("Logging into ShipWorks")]
        Login = 1,

        [Description("Creating a label")]
        CreateLabel = 2,

        [Description("Downloading orders")]
        Download = 3
    }
}