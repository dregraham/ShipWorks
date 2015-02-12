using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Valid Pickup Options
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsPickupOption
    {
        [Description("No Scheduled Pickup")]
        [ApiValue("08")]
        NoScheduledPickup = 0,

        [Description("Regular Daily Pickup")]
        [ApiValue("01")]
        RegularDailyPickup = 1,

        [Description("Daily-On Route")]
        [ApiValue("07")]
        DailyOnRoute = 2,

        [Description("Day Specific Pickup")]
        [ApiValue("99")]
        DaySpecificPickup = 3,

        [Description("SMART Pickup")]
        [ApiValue("02")]
        SmartPickup = 4
    }
}