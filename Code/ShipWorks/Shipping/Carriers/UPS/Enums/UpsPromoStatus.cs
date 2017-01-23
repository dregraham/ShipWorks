using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// UPS Promo Status
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsPromoStatus
    {
        [Description("None")]
        None = 0,

        [Description("Applied")]
        Applied = 1,

        [Description("Declined")]
        Declined = 2,
    }
}