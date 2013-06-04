using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Filters.Content.Conditions
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum PrintedQueryType
    {
        [Description("Has Been Printed With")]
        HasBeen = 1,

        [Description("Has Not Been Printed With")]
        HasNotBeen = 0
    }
}
