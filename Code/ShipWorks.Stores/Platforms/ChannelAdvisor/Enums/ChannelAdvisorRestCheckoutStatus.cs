using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    /// <summary>
    /// Checkout status for ChannelAdvisor's REST API
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ChannelAdvisorRestCheckoutStatus
    {
        [Description("Not Visited")]
        NotVisited = 0,

        [Description("Completed")]
        Completed = 1,

        [Description("Visited")]
        Visited = 2,

        [Description("Completed and Visited")]
        CompletedAndVisited = 3,

        [Description("Disabled")]
        Disabled = 4,

        [Description("Completed Offline")]
        CompletedOffline = 8,

        [Description("On Hold")]
        OnHold = 16,

        [Description("Unknown")]
        Unknown = 99
    }
}