using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExHazardousMaterialsQuantityUnits
    {
        [Description("ML")]
        Milliliters = 0,

        [Description("L")]
        Liters = 1,

        [Description("G")]
        Gram = 2,

        [Description("KG")]
        Kilogram = 3
    }
}