using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// State of the Scan Pack view
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum ScanPackState
    {
        [Description("ListeningForOrderScan")]
        ListeningForOrderScan = 0,

        [Description("OrderLoaded")]
        OrderLoaded = 1,

        [Description("ScanningItems")]
        ScanningItems = 2,

        [Description("OrderVerified")]
        OrderVerified = 3,
    }
}
