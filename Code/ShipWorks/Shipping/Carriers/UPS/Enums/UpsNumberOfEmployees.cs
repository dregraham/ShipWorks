﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// UPS Business Number of employees
    /// 01= 1
    /// 02 = 2-5
    /// 03 = 6-10
    /// 04 = 11-19
    /// 05 = 20 - 49
    /// 06= 50 - 99
    /// 07 = 100+
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsNumberOfEmployees
    {
        [Description("Select your number of employees.")]
        [ApiValue("0")]
        Unselected = 0,

        [Description("1")]
        [ApiValue("01")]
        One = 01,

        [Description("2-5")]
        [ApiValue("02")]
        TwoToFive = 02,

        [Description("6-10")]
        [ApiValue("03")]
        SixToTen = 03,

        [Description("11-19")]
        [ApiValue("04")]
        ElevenToNineteen = 04,

        [Description("20 - 49")]
        [ApiValue("05")]
        TwentyToFortyNine = 05,

        [Description("50 - 99")]
        [ApiValue("06")]
        FiftyToNinetyNine = 06,

        [Description("100+")]
        [ApiValue("07")]
        OneHundredAndOver = 07
    }
}
