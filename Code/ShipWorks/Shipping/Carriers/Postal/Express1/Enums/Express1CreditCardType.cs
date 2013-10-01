using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Enums
{
    /// <summary>
    /// Credit card type
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum Express1CreditCardType
    {
        [Description("Visa")]
        Visa = 0,

        [Description("MasterCard")]
        MasterCard = 1,

        [Description("American Express")]
        AmericanExpress = 2,

        [Description("Discover")]
        Discover = 3
    }
}
