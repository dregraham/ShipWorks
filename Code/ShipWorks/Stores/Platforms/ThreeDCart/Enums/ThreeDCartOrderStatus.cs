using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.ThreeDCart.Enums
{
    /// <summary>
    /// Hard coded 3DCart order statuses as of 3/21/16.
    /// </summary>
    /// <remarks>
    /// The 3DCart REST API does not currently support
    /// getting a list of order statuses. They also have no documentation confirming these statuses
    /// and their codes, I had to change an order to each status manually and confirm them.
    /// 3DCart users also have the ability to change the label that gets displayed with these
    /// statuses, but not the actuall status. So the status name may not match what the user sees
    /// on the 3DCart backend.
    /// </remarks>
    public enum ThreeDCartOrderStatus
    {
        [Description("New")]
        New = 1,

        [Description("Processing")]
        Processing = 2,

        [Description("Partial")]
        Partial = 3,

        [Description("Shipped")]
        Shipped = 4,

        [Description("Cancel")]
        Cancel = 5,

        [Description("Hold")]
        Hold = 6,

        [Description("Not Completed")]
        NotCompleted = 7,

        [Description("Custom 1")]
        Custom1 = 8,

        [Description("Custom 2")]
        Custom2 = 9,

        [Description("Custom 3")]
        Custom3 = 10,

        [Description("Unpaid")]
        Unpaid = 11,

        [Description("Review")]
        Review = 13
    }
}