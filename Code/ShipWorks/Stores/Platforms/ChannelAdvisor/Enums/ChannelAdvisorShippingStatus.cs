using System.ComponentModel;
using System.Reflection;

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

        [Description("Third Party Managed")]
        ThirdPartyManaged = 16,

        [Description("Unknown")]
        Unknown = 99
    }
}
