using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Options
{
    /// <summary>
    /// Single Scan settings
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum SingleScanSettings
    {
        [Description("Single scan disabled")]
        Disabled = 0,

        [Description("Single scan enabled")]
        Scan = 1,

        [Description("Single scan with auto print enabled")]
        AutoPrint = 2
    }
}