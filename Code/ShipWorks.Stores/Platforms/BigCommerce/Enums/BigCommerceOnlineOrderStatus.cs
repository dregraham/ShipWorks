using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.BigCommerce.Enums
{
    // Enum that represents the status of orders online
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum BigCommerceOnlineOrderStatus
    {
        [Description("Incomplete")]
        Incomplete = 0,

        [Description("Pending")]
        Pending = 1,

        [Description("Shipped")]
        Shipped = 2,

        [Description("Partially Shipped")]
        PartiallyShipped = 3,

        [Description("Refunded")]
        Refunded = 4,

        [Description("Cancelled")]
        Cancelled = 5,

        [Description("Declined")]
        Declined = 6,

        [Description("Awaiting Payment")]
        AwaitingPayment = 7,

        [Description("Awaiting Pickup")]
        AwaitingPickup = 8,

        [Description("Awaiting Shipment")]
        AwaitingShipment = 9,

        [Description("Completed")]
        Completed = 10,

        [Description("Awaiting Fulfillment")]
        AwaitingFulfillment = 11,

        [Description("Manual Verification Required")]
        ManualVerificationRequired = 12
    }
}
