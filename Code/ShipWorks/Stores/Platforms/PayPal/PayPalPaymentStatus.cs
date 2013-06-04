using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.PayPal
{
    /// <summary>
    /// Our controlled representation of PayPal Payment Status
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum PayPalPaymentStatus
    {
        [Description("None")]
        None = 0,

        [Description("Completed")]
        Completed = 1,

        [Description("Failed")]
        Failed = 2,

        [Description("Pending")]
        Pending = 3,

        [Description("Denied")]
        Denied = 4,

        [Description("Refunded")]
        Refunded = 5,

        [Description("Reversed")]
        Reversed = 6,

        [Description("Cancelled Reversed")]
        CanceledReversal = 7,

        [Description("Processed")]
        Processed = 8,

        [Description("Partially Refunded")]
        PartiallyRefunded = 9,

        [Description("Voided")]
        Voided = 10,

        [Description("Expired")]
        Expired = 11,

        [Description("In Progress")]
        InProgress = 12,

        [Description("Under Review")]
        UnderReview = 13
    }
}
