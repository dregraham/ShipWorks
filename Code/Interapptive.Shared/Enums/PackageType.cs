using System.Reflection;
using System.ComponentModel;

namespace Interapptive.Shared.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum PackageType
    {
        [Description("Contained in equipment")]
        ContainsInEquipement = 0,

        [Description("Packed with equipment")]
        PackedWithEquipment = 1,
    }
}