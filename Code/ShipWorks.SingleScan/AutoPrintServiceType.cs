using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Type of auto print service
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AutoPrintServiceType
    {
        [Description("Auto Print")]
        Default = 0,

        [Description("Auto Print with Logging")]
        Loggable = 1
    }
}