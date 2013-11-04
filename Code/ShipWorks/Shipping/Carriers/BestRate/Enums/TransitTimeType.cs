using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Shipping.Carriers.BestRate.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum TransitTimeType
    {
        [Description("Any")] 
        Any = 0,

        [Description("Expected")] 
        Expected = 1
    }
}
