using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Shopify.Enums
{
    /// <summary>
    /// Represents the status of a shopify order
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShopifyStatus
    {
        // DO NOT change the ApiValueAttribute values unles the actual Shopify text for the field changes!

        [Description("Open")]
        [ApiValueAttribute("open")]
        Open = 0,

        [Description("Closed")]
        [ApiValueAttribute("closed")]
        Closed = 1,

        [Description("Canceled")]
        [ApiValueAttribute("cancelled")]
        Canceled = 2,

        [Description("Shipped")]
        Shipped = 3,

        [Description("Unknown")]
        [ApiValueAttribute("unknown")]
        Unknown = 4
    }
}
