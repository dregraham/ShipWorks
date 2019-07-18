using System.Reflection;

namespace ShipWorks.OrderLookup.ScanPack
{
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum ScanPackState
    {
        ListeningForOrderScan = 0,

        ListeningForItemScan = 1,

        OrderVerified = 2,
    }
}
