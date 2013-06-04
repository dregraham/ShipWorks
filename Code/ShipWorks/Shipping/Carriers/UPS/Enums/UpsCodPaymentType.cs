using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// Accepted payment methods for UPS COD
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsCodPaymentType
    {
        [Description("Any (No Cash)")]
        Cash = 0,

        [Description("Cashier's Check or Money Order Only")]
        CheckOrMoneyOrder = 1
    }
}
