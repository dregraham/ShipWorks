using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.GenericModule.Enums
{
    /// <summary>
    /// The fulfillment channel an order was placed
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]

    public enum GenericModuleIsAmazonPrime
    {
        [Description("Unavailable")]
        Unknown = 0,

        [Description("Yes")]
        Yes = 1,

        [Description("No")]
        No = 2
    }
}
