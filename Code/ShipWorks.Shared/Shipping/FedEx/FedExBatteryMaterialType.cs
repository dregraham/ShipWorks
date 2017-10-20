using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.FedEx
{
    /// <summary>
    /// Battery material type for FedEx hazardous materials
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExBatteryMaterialType
    {
        [Description("Not specified")]
        NotSpecified = 0,

        [Description("Lithium Ion")]
        LithiumIon = 1,

        [Description("Lithium Metal")]
        LithiumMetal = 2,
    }
}