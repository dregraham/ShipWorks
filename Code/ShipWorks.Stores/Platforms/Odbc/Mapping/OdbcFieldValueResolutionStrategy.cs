using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Strategies for reading OdbcFields
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum OdbcFieldValueResolutionStrategy
    {
        [Description("Default")]
        Default = 0,

        [Description("Shipping Provider")]
        ShippingCarrier = 1,

        [Description("Shipping Service")]
        ShippingService = 2
    }
}