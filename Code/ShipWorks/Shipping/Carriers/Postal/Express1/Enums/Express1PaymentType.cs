using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Payment type (Credit card or ACH)
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum Express1PaymentType
    {
        [Description("Credit Card")]
        CreditCard = 0,

        [Description("ACH")]
        Ach = 1
    }
}
