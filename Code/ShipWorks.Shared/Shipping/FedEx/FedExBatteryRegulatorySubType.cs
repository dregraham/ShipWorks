using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.FedEx
{
    /// <summary>
    /// Battery regulatory sub type for FedEx hazardous materials
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExBatteryRegulatorySubType
    {
        [Description("Not specified")]
        NotSpecified = 0,

        [Description("IATA section II")]
        IATASectionII = 1,
    }
}
