using System.Reflection;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// UPS Promo Status
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsPromoStatus
    {
        None = 0,
        Applied = 1
    }
}