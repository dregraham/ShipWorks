using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsLogoType
    {
        [Description("UPS")]
        Ups = 1,

        [Description("UPS from ShipWorks")]
        UpsFromShipWorks = 2
    }
}
