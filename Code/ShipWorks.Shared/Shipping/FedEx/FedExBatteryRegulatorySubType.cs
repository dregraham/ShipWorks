using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.FedEx
{
    /// <summary>
    /// Battery regulatory sub type for FedEx hazardous materials
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum FedExBatteryRegulatorySubType
    {
        [Description("Not specified")]
        [ApiValue("None")]
        NotSpecified = 0,

        [Description("IATA section II")]
        [ApiValue("IATA_SECTION_II")]
        IATASectionII = 1,
    }
}
