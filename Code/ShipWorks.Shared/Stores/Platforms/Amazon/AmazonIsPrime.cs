using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// The fulfillment channel an order was placed
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AmazonIsPrime
    {
        [Description("Unavailable")]
        Unknown = 0,

        [Description("Yes")]
        Yes = 1,

        [Description("No")]
        No = 2
    }
}
