using System.Reflection;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    /// <summary>
    /// Adjustment type enum for ChannelAdvisor
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ChannelAdvisorAdjustmentType
    {
        Refund = 0,

        Cancellation = 1,

        Dispute = 2
    }
}