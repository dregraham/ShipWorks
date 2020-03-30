using System.ComponentModel;

namespace ShipWorks.SingleScan.Enums
{
    /// <summary>
    /// Enums representing each single scan confirmation mode.
    /// </summary>
    public enum SingleScanConfirmationMode
    {
        [Description("Select")]
        Select = 0,

        [Description("Create New")]
        CreateNew = 1,

        [Description("Print Existing")]
        PrintExisting = 2
    }
}