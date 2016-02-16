using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Compliance state of a license
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public enum ComplianceLevel
    {
        [Description("Not Compliant")]
        NotCompliant = 0,

        [Description("Not Compliant")]
        Compliant = 1
    }
}