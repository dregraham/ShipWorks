using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.ThreeDCart.Enums
{
    /// <summary>
    /// Hard coded 3dcart order statuses as of 3/21/16.
    /// </summary>
    /// <remarks>
    /// The 3dcart REST API does not currently support
    /// getting a list of order statuses. They also have no documentation confirming these statuses
    /// and their codes, I had to change an order to each status manually and confirm them.
    /// 3dcart users also have the ability to change the label that gets displayed with these
    /// statuses, but not the actual status. So the status name may not match what the user sees
    /// on the 3dcart backend.
    /// </remarks>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ThreeDCartOrderStatus
    {
        [ApiValue("New")]
        [Description("New")]
        New = 1,

        [ApiValue("Processing")]
        [Description("Processing")]
        Processing = 2,

        [ApiValue("Partial")]
        [Description("Partial")]
        Partial = 3,

        [ApiValue("Shipped")]
        [Description("Shipped")]
        Shipped = 4,

        [ApiValue("Cancel")]
        [Description("Cancel")]
        Cancel = 5,

        [ApiValue("Hold")]
        [Description("Hold")]
        Hold = 6,

        [ApiValue("Not Completed")]
        [Description("Not Completed")]
        NotCompleted = 7,

        [ApiValue("Custom 1")]
        [Description("Custom 1")]
        Custom1 = 8,

        [ApiValue("Custom 2")]
        [Description("Custom 2")]
        Custom2 = 9,

        [ApiValue("Custom 3")]
        [Description("Custom 3")]
        Custom3 = 10,

        [ApiValue("Unpaid")]
        [Description("Unpaid")]
        Unpaid = 11,

        [ApiValue("Review")]
        [Description("Review")]
        Review = 13
    }
}