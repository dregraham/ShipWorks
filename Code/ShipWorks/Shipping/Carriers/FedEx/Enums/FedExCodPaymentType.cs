using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// COD payment method
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExCodPaymentType
    {
        [Description("Any")]
        Any = 0,

        [Description("Secured")]
        Secured = 1,

        [Description("Unsecured")]
        Unsecured = 2
    }
}
