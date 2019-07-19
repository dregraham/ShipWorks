using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.OrderLookup.ScanPack
{
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
