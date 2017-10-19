using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.FedEx
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExBatteryClassificationType
    {
        [Description("None")]
        None = 0,

        [Description("Lithium Ion")]
        LithiumIon = 1,

        [Description("Lithium Metal")]
        LithiumMetal = 2,
    }
}