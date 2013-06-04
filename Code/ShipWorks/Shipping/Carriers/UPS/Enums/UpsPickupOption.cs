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
        [Description("Regular Daily Pickup")]
        [ApiValue("01")]
        RegularDailyPickup = 0,

        [Description("Daily-On Route")]
        [ApiValue("07")]
        DailyOnRoute = 1,

        [Description("Day Specific Pickup")]
        [ApiValue("99")]
        DaySpecificPickup = 2,

        [Description("SMART Pickup")]
        [ApiValue("02")]
        SmartPickup = 3,

        [Description("No Scheduled Pickup")]
        [ApiValue("08")]
        NoScheduledPickup = 4
    }
}