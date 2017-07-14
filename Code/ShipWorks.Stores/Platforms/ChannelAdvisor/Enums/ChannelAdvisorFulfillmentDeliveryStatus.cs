using System.Reflection;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    /// <summary>
    /// Fulfillment delivery status enum for ChannelAdvisor
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ChannelAdvisorFulfillmentDeliveryStatus
    {
        NoChange = 1,

        InTransit = 2,

        ReadyForPickup = 4,

        Complete = 8,

        Canceled = 13,

        ThirdPartyManaged = 26,

        Failed = -1
    }
}