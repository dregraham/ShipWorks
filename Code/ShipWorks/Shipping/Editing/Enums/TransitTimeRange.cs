﻿using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Editing.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    enum TransitTimeRange
    {
        [Description("Anytime")]
        Anytime = 0,

        [Description("One Day")]
        OneDay = 1,

        [Description("Two Days")]
        TwoDays = 2,

        [Description("Three Days")]
        ThreeDays = 3, 

        [Description("4-7 Days")]
        FourToSevenDays = 4
    }
}
