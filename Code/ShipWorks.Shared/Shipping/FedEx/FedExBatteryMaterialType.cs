using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.FedEx
{
    /// <summary>
    /// Battery material type for FedEx hazardous materials
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum FedExBatteryMaterialType
    {
        [Description("Not specified")]
        [ApiValue("None")]
        NotSpecified = 0,

        [Description("Lithium Ion")]
        [ApiValue("LITHIUM_ION")]
        LithiumIon = 1,

        [Description("Lithium Metal")]
        [ApiValue("LITHIUM_METAL")]
        LithiumMetal = 2,
    }
}