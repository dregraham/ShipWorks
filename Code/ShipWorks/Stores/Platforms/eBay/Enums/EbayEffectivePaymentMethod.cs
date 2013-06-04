using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Ebay.Enums
{
    /// <summary>
    /// Our own type for representing the payment method of an order
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EbayEffectivePaymentMethod
    {
        [Description("")]
        NotChosen = 0,

        [Description("PayPal")]
        PayPal = 1,

        [Description("Money Order\\Cashiers Check")]
        MoneyOrderOrCashiersCheck = 2,

        [Description("Personal Check")]
        PersonalCheck = 3,

        [Description("COD")]
        Cod = 4,

        [Description("Credit Card")]
        CreditCard = 5,
        
        [Description("Other")]
        Other = 6,

        [Description("Not Specified")]
        NotSpecified = 7,

        [Description("Unknown")]
        Unknown = 8,

        [Description("Money Transfer")]
        MoneyTransferCip = 9,

        [Description("Money Transfer")]
        MoneyTransferCipPlus = 10,

        [Description("Cash on pick-up")]
        CashOnPickup = 11,

        [Description("Visa\\Mastercard")]
        VisaMastercard = 12,

        [Description("American Express")]
        AmericanExpress = 13,

        [Description("Discover Card")]
        DiscoverCard = 14,

        [Description("See Description")]
        SeeDescription = 15,

        [Description("Online Payment")]
        OnlinePayment = 16
    }
}
