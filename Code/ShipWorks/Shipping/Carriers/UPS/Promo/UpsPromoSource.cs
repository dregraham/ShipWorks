using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    /// <summary>
    /// The source of the UPS Promo
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsPromoSource
    {
        SetupWizard = 0,
        PromoFootnote = 1
    }
}