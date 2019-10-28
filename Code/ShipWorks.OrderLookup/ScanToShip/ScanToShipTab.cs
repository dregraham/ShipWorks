using System.Reflection;

namespace ShipWorks.OrderLookup.ScanToShip
{
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum ScanToShipTab
    {
        PackTab = 0,

        ShipTab = 1
    }
}
