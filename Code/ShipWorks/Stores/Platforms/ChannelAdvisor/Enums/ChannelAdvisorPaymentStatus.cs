using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ChannelAdvisorPaymentStatus
    {
        [Description("")]
        NoChange = 0,

        [Description("Not Submitted")]
        NotSubmitted = 1,

        [Description("Cleared")]
        Cleared = 2,

        [Description("Submitted")]
        Submitted = 3,

        [Description("Failed")]
        Failed = 4,

        [Description("Deposited")]
        Deposited = 5,

        [Description("Unknown")]
        Unknown = 99
    }
}
