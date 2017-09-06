using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    /// <summary>
    /// The source of the UPS Promo
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsPromoSource
    {
        [Description("Setup Wizard")]
        SetupWizard = 0,

        [Description("Promo Footnote")]
        PromoFootnote = 1
    }
}