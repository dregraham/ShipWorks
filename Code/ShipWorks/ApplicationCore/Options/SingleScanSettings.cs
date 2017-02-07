using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Options
{
    /// <summary>
    /// Single Scan settings
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum SingleScanSettings
    {
        [Description("Single scan disabled")]
        [ApiValue("Disabled")]
        Disabled = 0,

        [Description("Single scan enabled")]
        [ApiValue("Scan")]
        Scan = 1,

        [Description("Single scan with auto print enabled")]
        [ApiValue("AutoPrint")]
        AutoPrint = 2
    }
}