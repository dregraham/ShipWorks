using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    /// <summary>
    /// The UPS account type when activating the UPS Promo
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsPromoAccountType
    {
        [Description("New UPS Account")]
        NewAccount = 0,

        [Description("Existing UPS Account")]
        ExistingAccount = 1
    }
}