using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ChannelAdvisorCheckoutStatus
    {
        [Description("")]
        NoChange = 0,

        [Description("Not Visited")]
        NotVisited = 1,

        [Description("Completed")]
        Completed = 2,

        [Description("Visited")]
        Visited = 3,

        [Description("Canceled")]
        Cancelled = 4,

        [Description("Completed Offline")]
        CompletedOffline = 5,

        [Description("On Hold")]
        OnHold = 6,

        [Description("Unknown")]
        Unknown = 99
    }
}
