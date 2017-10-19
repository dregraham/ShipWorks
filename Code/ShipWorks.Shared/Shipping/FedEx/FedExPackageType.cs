using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.FedEx
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExPackageType
    {
        [Description("Contained in equipment")]
        ContainsInEquipement = 0,

        [Description("Packed with equipment")]
        PackedWithEquipment = 1,
    }
}