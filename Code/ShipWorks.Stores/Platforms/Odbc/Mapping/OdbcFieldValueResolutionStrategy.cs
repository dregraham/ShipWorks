using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    [Obfuscation(Exclude = true)]
    public enum OdbcFieldValueResolutionStrategy
    {
        [Description("Default")]
        Default,

        [Description("Shipping Carrier")]
        ShippingCarrier,

        [Description("Shipping Provider")]
        ShippingService
    }
}
