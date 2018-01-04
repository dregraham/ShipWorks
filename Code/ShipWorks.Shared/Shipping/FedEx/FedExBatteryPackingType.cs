using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.FedEx
{
    /// <summary>
    /// Battery packing type for FedEx hazardous materials
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum FedExBatteryPackingType
    {
        [Description("Not specified")]
        [ApiValue("None")]
        NotSpecified = 0,

        [Description("Contained in equipment")]
        [ApiValue("CONTAINED_IN_EQUIPMENT")]
        ContainsInEquipement = 1,

        [Description("Packed with equipment")]
        [ApiValue("PACKED_WITH_EQUIPMENT")]
        PackedWithEquipment = 2,
    }
}