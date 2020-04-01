using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Settings
{
    /// <summary>
    /// Enums representing each single scan confirmation mode.
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false)]
    public enum SingleScanConfirmationMode
    {
        [Description("Ask Every Time")]
        Select = 0,

        [Description("Create New Label")]
        CreateNew = 1,

        [Description("Print Existing Label")]
        PrintExisting = 2
    }
}