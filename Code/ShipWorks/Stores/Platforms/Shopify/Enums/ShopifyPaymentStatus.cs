using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Shopify.Enums
{
    /// <summary>
    /// Represents the Financial status of a shopify order
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShopifyPaymentStatus
    {
        // DO NOT change the ApiValueAttribute values unles the actual Shopify text for the field changes!

        [Description("Authorized")]
        [ApiValueAttribute("authorized")]
        Authorized = 0,

        [Description("Pending")]
        [ApiValueAttribute("pending")]
        Pending = 1,

        [Description("Paid")]
        [ApiValueAttribute("paid")]
        Paid = 2,

        [Description("Abandoned")]
        [ApiValueAttribute("abandoned")]
        Abandoned = 3,

        [Description("Refunded")]
        [ApiValueAttribute("refunded")]
        Refunded = 4,

        [Description("Voided")]
        [ApiValueAttribute("voided")]
        Voided = 5,

        [Description("Partially Refunded")]
        [ApiValueAttribute("partially_refunded")]
        PartiallyRefunded = 6,

        [Description("Partially Paid")]
        [ApiValueAttribute("partially_paid")]
        PartiallyPaid = 7,

        [Description("Unknown")]
        [ApiValueAttribute("unknown")]
        Unknown = 8
    }
}
