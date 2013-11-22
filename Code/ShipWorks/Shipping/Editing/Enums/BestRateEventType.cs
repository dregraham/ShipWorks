using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Shipping.Editing.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    [Flags]
    public enum BestRateEventType
    {
        [Description("None")]
        None = 0,

        [Description("Rates Compared")]
        RatesCompared = 1,

        [Description("Rate Selected")]
        RateSelected = 2,

        [Description("Rate Auto Selected and Processed")]
        RateAutoSelectedAndProcessed  = 4
    }
}
