using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.PayPal.WebServices;
using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.PayPal
{
    /// <summary>
    /// Utility class for working with paypal
    /// </summary>
    public static class PayPalUtility
    {
        /// <summary>
        /// Gets our representation of the paypal api payment status enum
        /// </summary>
        public static PayPalPaymentStatus GetPaymentStatus(PaymentStatusCodeType paypalStatus)
        {
            switch (paypalStatus)
            {
                case PaymentStatusCodeType.None:                return PayPalPaymentStatus.None;
                case PaymentStatusCodeType.Completed:           return PayPalPaymentStatus.Completed;
                case PaymentStatusCodeType.Failed:              return PayPalPaymentStatus.Failed;
                case PaymentStatusCodeType.Pending:             return PayPalPaymentStatus.Pending;
                case PaymentStatusCodeType.Denied:              return PayPalPaymentStatus.Denied;
                case PaymentStatusCodeType.Refunded:            return PayPalPaymentStatus.Refunded;
                case PaymentStatusCodeType.Reversed:            return PayPalPaymentStatus.Reversed;
                case PaymentStatusCodeType.CanceledReversal:    return PayPalPaymentStatus.CanceledReversal;
                case PaymentStatusCodeType.Processed:           return PayPalPaymentStatus.Processed;
                case PaymentStatusCodeType.PartiallyRefunded:   return PayPalPaymentStatus.PartiallyRefunded;
                case PaymentStatusCodeType.Voided:              return PayPalPaymentStatus.Voided;
                case PaymentStatusCodeType.Expired:             return PayPalPaymentStatus.Expired;
                case PaymentStatusCodeType.InProgress:          return PayPalPaymentStatus.InProgress;

                default:
                    return PayPalPaymentStatus.None;
            }
        }

        /// <summary>
        /// Gets our representation of the paypal api address status enum
        /// </summary>
        public static PayPalAddressStatus GetAddressStatus(AddressStatusCodeType addressStatusCodeType)
        {
            switch (addressStatusCodeType)
            {
                case AddressStatusCodeType.None: return PayPalAddressStatus.None;
                case AddressStatusCodeType.Confirmed: return PayPalAddressStatus.Confirmed;
                case AddressStatusCodeType.Unconfirmed: return PayPalAddressStatus.Unconfirmed;

                default:
                    return PayPalAddressStatus.None;
            }
        }
    }
}
