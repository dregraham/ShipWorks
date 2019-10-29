using System.Reflection;

namespace ShipWorks.OrderLookup.ScanToShip
{
    /// <summary>
    /// Tab in the ScanToShipControl
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum ScanToShipTab
    {
        PackTab = 0,

        ShipTab = 1
    }
}
