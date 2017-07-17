using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ChannelAdvisorShippingStatus
    {
        [Description("")]
        NoChange = 0,

        [Description("Unshipped")]
        Unshipped = 1,

        [Description("Shipped")]
        Shipped = 2,

        [Description("Partially Shipped")]
        PartiallyShipped = 3,

        [Description("Pending Shipment")]
        PendingShipment = 4,

        [Description("Unknown")]
        Unknown = 99
    }
}
