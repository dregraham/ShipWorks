using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Rakuten.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum RakutenOrderStatus
    {
        [Description("")]
        NoChange = 0,

        [Description("Processing Payment")]
        ProcessingPayment = 1,

        [Description("Not Shipped")]
        NotShipped = 2,

        [Description("Awaiting Completion")]
        AwaitingCompletion = 3,

        [Description("Complete")]
        Complete = 4,

        [Description("Cancelled")]
        Cancelled = 5,

        [Description("Unknown")]
        Unknown = 99
    }
}
