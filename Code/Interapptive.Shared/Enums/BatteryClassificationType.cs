using System.Reflection;
using System.ComponentModel;

namespace Interapptive.Shared.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum BatteryClassificationType
    {
        [Description("Lithium Ion")]
        LithiumIon = 0,

        [Description("Lithium Metal")]
        LithiumMetal = 1,
    }
}