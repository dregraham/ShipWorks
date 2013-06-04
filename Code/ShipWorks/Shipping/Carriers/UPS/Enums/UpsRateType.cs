using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsRateType
    {
        [Description("Daily Pickup")]
        DailyPickup = 0,

        [Description("Occasional")]
        Occasional = 1,

        [Description("Suggested Retail")]
        Retail = 2,

        [Description("Negotiated (Account Based Rates)")]
        Negotiated = 3
    }
}
