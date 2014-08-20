using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum InternationalLabelType
    {
        [Description("Standard")]
        Standard = 0,

        [Description("Thermal")]
        Thermal = 1,

        [Description("Same As Domestic")]
        SameAsDomestic = 2
    }
}
