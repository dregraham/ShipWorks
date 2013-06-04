using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.OnTrac.Enums
{
    /// <summary>
    /// Valid OnTrac service type values
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum OnTracServiceType
    {
        [Description("Not Available")]
        None = 0,

        [Description("Sunrise")]
        [ApiValue("S")]
        Sunrise = 1,

        [Description("Sunrise Gold")]
        [ApiValue("G")]
        SunriseGold = 2,

        [Description("OnTrac Ground")]
        [ApiValue("C")]
        Ground = 3,

        [Description("Palletized Freight")]
        [ApiValue("H")]
        PalletizedFreight = 4
    }
}