using System;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Describes BestRate features used by user when creating and processing a shipment.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    [Flags]
    public enum BestRateEventTypes
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
