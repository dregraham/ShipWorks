using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Filters
{
    /// <summary>
    /// Specifies what objects a filter applies to.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FilterTarget
    {
        [Description("Orders")]
        Orders = 0,

        [Description("Customers")]
        Customers = 1,

        [Description("Shipments")]
        Shipments = 2,

        [Description("Order Items")]
        Items = 3
    }
}
