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

        [Description("ListeningForItemScan")]
        ListeningForItemScan = 1,

        [Description("OrderVerified")]
        OrderVerified = 2,
    }
}
