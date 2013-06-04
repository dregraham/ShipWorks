using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

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
