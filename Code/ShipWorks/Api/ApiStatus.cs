using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Api
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ApiStatus
    {
        [Description("Stopped")]
        Stopped,

        [Description("Running")]
        Running,

        [Description("Updating")]
        Updating
    }
}
