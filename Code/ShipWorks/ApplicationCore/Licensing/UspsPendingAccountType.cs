using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Licensing
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UspsPendingAccountType
    {
        [Description("None")]
        None = 0,

        [Description("Create")]
        Create = 1,

        [Description("Existing")]
        Existing = 2
    }
}