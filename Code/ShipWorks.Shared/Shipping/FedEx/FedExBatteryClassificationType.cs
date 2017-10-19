using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.FedEx
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExBatteryClassificationType
    {
        [Description("Lithium Ion")]
        LithiumIon = 0,

        [Description("Lithium Metal")]
        LithiumMetal = 1,
    }
}