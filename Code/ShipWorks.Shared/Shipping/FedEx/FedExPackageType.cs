using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.FedEx
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExPackageType
    {
        [Description("None")]
        None = 0,

        [Description("Contained in equipment")]
        ContainsInEquipement = 1,

        [Description("Packed with equipment")]
        PackedWithEquipment = 2,
    }
}