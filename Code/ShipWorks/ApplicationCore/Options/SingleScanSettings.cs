using System.ComponentModel;

namespace ShipWorks.ApplicationCore.Options
{
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