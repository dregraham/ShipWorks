using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Possible UPS delivery confirmations
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsDeliveryConfirmationType
    {
        [Description("None")]
        None = 0,

        [Description("Signature Required")]
        Signature = 2,

        [Description("Adult Signature Required")]
        AdultSignature = 3,

        [Description("USPS Delivery Confirmation")]
        UspsDeliveryConfirmation = 4
    }
}
