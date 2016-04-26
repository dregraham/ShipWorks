using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// An enum representing the boolean value of compliance.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ComplianceLevel
    {
        [Description("Not Compliant")]
        NotCompliant = 0,

        [Description("Compliant")]
        Compliant = 1
    }
}