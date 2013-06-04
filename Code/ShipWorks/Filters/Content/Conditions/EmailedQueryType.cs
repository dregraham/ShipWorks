using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Filters.Content.Conditions
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EmailedQueryType
    {
        [Description("Has Been Sent With")]
        HasBeen = 1,

        [Description("Has Not Been Sent With")]
        HasNotBeen = 0
    }
}
