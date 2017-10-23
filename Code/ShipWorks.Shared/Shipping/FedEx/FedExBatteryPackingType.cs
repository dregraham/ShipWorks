using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.FedEx
{
    /// <summary>
    /// Battery packing type for FedEx hazardous materials
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum FedExBatteryPackingType
    {
        [Description("Not specified")]
        NotSpecified = 0,

        [Description("Contained in equipment")]
        ContainsInEquipement = 1,

        [Description("Packed with equipment")]
        PackedWithEquipment = 2,
    }
}