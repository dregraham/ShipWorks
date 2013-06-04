using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ThermalDocTabType
    {
        [Description("Before the label")]
        Leading = 0,

        [Description("After the label")]
        Trailing = 1
    }
}
