using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Ebay.Enums
{
    /// <summary>
    /// Our own type for representing an order's payment status
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EbayEffectivePaymentStatus
    {
        [Description("Not Completed")]
        Incomplete = 0, 

        [Description("PayPal Processing")]
        PaymentPendingPayPal = 1,

        [Description("Buyer Requests Total")]
        BuyerRequestsTotal = 2,

        [Description("Awaiting Payment")]
        AwaitingPayment = 3,

        [Description("Paid")]
        Paid = 4,

        [Description("Failed")]
        Failed = 5,

        [Description("Responded to Buyer")]
        SellerResponded = 6,

        [Description("Payment Pending")]
        PaymentPending = 7,

        [Description("Payment Pending (Escrow)")]
        PaymentPendingEscrow = 8,

        [Description("Escrow Cancelled")]
        EscrowCanceled = 9
    }
}
