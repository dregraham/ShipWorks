using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    /// <summary>
    /// The fulfillment channel an order was placed
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ChannelAdvisorIsAmazonPrime
    {
        [Description("Unavailable")]
        Unknown = 0,

        [Description("Yes")]
        Yes = 1,

        [Description("No")]
        No = 2
    }
}
