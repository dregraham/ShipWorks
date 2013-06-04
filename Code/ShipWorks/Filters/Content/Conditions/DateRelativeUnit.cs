using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Units for relative date condition operators
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum DateRelativeUnit
    {
        [Description("Week")]
        Week = 0,

        [Description("Month")]
        Month = 1,

        [Description("Quarter")]
        Quarter = 2,

        [Description("Year")]
        Year = 3
    }
}
