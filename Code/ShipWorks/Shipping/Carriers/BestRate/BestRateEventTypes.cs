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
        None = 0x00000000,

        [Description("Rates Compared")]
        RatesCompared = 0x00000001,

        [Description("Rate Selected")]
        RateSelected = 0x00000002,

        [Description("Rate Auto Selected and Processed")]
        RateAutoSelectedAndProcessed = 0x00000004
    }
}
