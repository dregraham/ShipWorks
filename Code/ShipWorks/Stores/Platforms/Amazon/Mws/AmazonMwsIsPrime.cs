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
    public enum AmazonMwsIsPrime
    {
        [Description("Unavailable")]
        Unknown = 0,

        [Description("True")]
        True = 1,

        [Description("False")]
        False = 2
    }
}
