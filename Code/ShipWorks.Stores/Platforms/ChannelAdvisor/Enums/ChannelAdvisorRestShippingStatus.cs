using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    /// <summary>
    /// Shipping Status for ChannelAdvisor's REST API
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ChannelAdvisorRestShippingStatus
    {
        [Description("Unshipped")]
        Unshipped = 0,

        [Description("Shipped")]
        Shipped = 1,

        [Description("Partially Shipped")]
        PartiallyShipped = 2,

        [Description("Pending Shipment")]
        PendingShipment = 4,

        [Description("Canceled")]
        Canceled = 8,

        [Description("Third Party Managed")]
        ThirdPartyManaged = 16,

        [Description("Unknown")]
        Unknown = 99
    }
}