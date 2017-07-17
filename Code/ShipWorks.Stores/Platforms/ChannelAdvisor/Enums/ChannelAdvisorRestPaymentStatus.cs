using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    /// <summary>
    /// Payment status for ChannelAdvisor's REST API
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ChannelAdvisorRestPaymentStatus
    {
        [Description("Not Yet Submitted")]
        NotYetSubmitted = 0,

        [Description("Cleared")]
        Cleared = 1,

        [Description("Submitted")]
        Submitted = 2,

        [Description("Failed")]
        Failed = 4,

        [Description("Deposited")]
        Deposited = 8,

        [Description("Unknown")]
        Unknown = 99
    }
}