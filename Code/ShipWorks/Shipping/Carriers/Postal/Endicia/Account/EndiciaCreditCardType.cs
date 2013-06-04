using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Account
{
    /// <summary>
    /// Credit card types for signing
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EndiciaCreditCardType
    {
        [Description("Visa")]
        Visa = 0,

        [Description("MasterCard")]
        MasterCard = 1,

        [Description("American Express")]
        AmericanExpress = 2,

        [Description("Carte Blanche")]
        CarteBlanche = 3,

        [Description("Discover/Novis")]
        Discover = 4,

        [Description("Diners Club")]
        DinersClub = 5
    }
}
