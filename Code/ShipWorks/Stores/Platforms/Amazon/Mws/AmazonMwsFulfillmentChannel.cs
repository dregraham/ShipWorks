using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// The fulfillment channel an order was placed
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AmazonMwsFulfillmentChannel
    {
        [Description("Unavailable")]
        Unknown = 0,

        [Description("Amazon")]
        AFN = 1,

        [Description("Merchant")]
        MFN = 2
    }
}
