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
        None = 0x00,

        [Description("Rates Compared")]
        RatesCompared = 0x01,

        [Description("Rate Selected")]
        RateSelected = 0x02,

        [Description("Rate Auto Selected and Processed")]
        RateAutoSelectedAndProcessed  = 0x04
    }
}
