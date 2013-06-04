using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Filters.Content.Conditions
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum DateWithinRangeType
    {
        [Description("Whole {0}s, and including this {0}")]
        WholeInclusive = 0,

        [Description("Whole {0}s, which excludes this {0}")]
        WholeExclusive = 1,

        [Description("From today")]
        FromToday = 2
    }
}
