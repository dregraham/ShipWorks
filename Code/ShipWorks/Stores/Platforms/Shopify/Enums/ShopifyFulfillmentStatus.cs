using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Shopify.Enums
{
    /// <summary>
    /// Represents the Fulfillment status of a shopify order
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShopifyFulfillmentStatus
    {
        // DO NOT change the ApiValueAttribute values unles the actual Shopify text for the field changes!

        [Description("Unshipped")]
        [ApiValueAttribute("unshipped")]
        Unshipped = 0,

        [Description("Partial")]
        [ApiValueAttribute("partial")]
        Partial = 1,

        [Description("Fulfilled")]
        [ApiValueAttribute("fulfilled")]
        Fulfilled = 2,

        [Description("Restocked")]
        [ApiValueAttribute("restocked")]
        Restocked = 3,

        [Description("Unknown")]
        [ApiValueAttribute("unknown")]
        Unknown = 4
    }
}
