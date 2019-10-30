using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.OrderLookup.ScanToShip
{
    /// <summary>
    /// Tab in the ScanToShipControl
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum ScanToShipTab
    {
        [Description("The pack tab")]
        PackTab = 0,

        [Description("The scan tab")]
        ShipTab = 1
    }
}
