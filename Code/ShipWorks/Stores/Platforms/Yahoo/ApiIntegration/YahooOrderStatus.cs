using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    public enum YahooOrderStatus
    {
        [Description("OK")]
        OK = 0,

        [Description("Fraudulent")]
        Fraudulent = 1,

        [Description("Cancelled")]
        Cancelled = 2,

        [Description("Returned")]
        Returned = 3,

        [Description("On Hold")]
        OnHold = 4,

        [Description("Pending Review")]
        PendingReview = 5,

        [Description("Partially Shipped")]
        PartiallyShipped = 6,

        [Description("Fully Shipped")]
        FullyShipped = 7,

        [Description("Tracked")]
        Tracked = 8
    }
}