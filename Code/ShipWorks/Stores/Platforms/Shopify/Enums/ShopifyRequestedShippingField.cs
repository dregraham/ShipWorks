using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Shopify.Enums
{
    /// <summary>
    /// Represents the requested shipping field to use from the Shopify API JSON
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShopifyRequestedShippingField
    {
        [Description("Shipping Service Code")]
        [ApiValue("code")]
        Code = 0,

        [Description("Shipping Service Name")]
        [ApiValueAttribute("title")]
        Title = 1
    }
}
