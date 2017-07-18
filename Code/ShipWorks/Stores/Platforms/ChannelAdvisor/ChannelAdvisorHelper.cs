using System;
using Interapptive.Shared.Business.Geography;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Constants;
using log4net;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Helper/Utility methods for CA
    /// </summary>
    public static class ChannelAdvisorHelper
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ChannelAdvisorHelper));

        /// <summary>
        /// Get the ShipWorks value for the given CA payment status
        /// </summary>
        public static ChannelAdvisorPaymentStatus GetShipWorksPaymentStatus(string caPaymentStatus)
        {
            switch (caPaymentStatus)
            {
                case PaymentStatusCodes.NoChange: return ChannelAdvisorPaymentStatus.NoChange;
                case PaymentStatusCodes.NotSubmitted: return ChannelAdvisorPaymentStatus.NotSubmitted;
                case PaymentStatusCodes.Cleared: return ChannelAdvisorPaymentStatus.Cleared;
                case PaymentStatusCodes.Submitted: return ChannelAdvisorPaymentStatus.Submitted;
                case PaymentStatusCodes.Failed: return ChannelAdvisorPaymentStatus.Failed;
                case PaymentStatusCodes.Deposited: return ChannelAdvisorPaymentStatus.Deposited;
                default:
                    // ChannelAdvisor passes payment status around as a string, so they can add additional
                    // functionality without having applications update their WSDLs. If we get here, CA
                    // has added a new status string, so return an unknown status rather than crashing the
                    // system not allowing a user to download any orders until we have updated the integration
                    log.Warn(string.Format("An invalid/unknown value ({0}) was encountered for CA payment status", caPaymentStatus));
                    return ChannelAdvisorPaymentStatus.Unknown;
            }
        }

        /// <summary>
        /// Get the ShipWorks value for the given CA checkout status
        /// </summary>
        public static ChannelAdvisorCheckoutStatus GetShipWorksCheckoutStatus(string caCheckoutStatus)
        {
            switch (caCheckoutStatus)
            {
                case CheckoutStatusCodes.NoChange: return ChannelAdvisorCheckoutStatus.NoChange;
                case CheckoutStatusCodes.Cancelled: return ChannelAdvisorCheckoutStatus.Cancelled;
                case CheckoutStatusCodes.Completed: return ChannelAdvisorCheckoutStatus.Completed;
                case CheckoutStatusCodes.CompletedOffline: return ChannelAdvisorCheckoutStatus.CompletedOffline;
                case CheckoutStatusCodes.NotVisited: return ChannelAdvisorCheckoutStatus.NotVisited;
                case CheckoutStatusCodes.OnHold: return ChannelAdvisorCheckoutStatus.OnHold;
                case CheckoutStatusCodes.Visited: return ChannelAdvisorCheckoutStatus.Visited;
                default:
                    // ChannelAdvisor passes checkout status around as a string, so they can add additional
                    // functionality without having applications update their WSDLs. If we get here, CA
                    // has added a new status string, so return an unknown status rather than crashing the
                    // system and not allowing a user to download any orders until we have updated the integration
                    log.Warn(string.Format("An invalid/unknown value ({0}) was encountered for CA checkout status", caCheckoutStatus));
                    return ChannelAdvisorCheckoutStatus.Unknown;
            }
        }

        /// <summary>
        /// Get the ShipWorks value for the given CA shipping status
        /// </summary>
        public static ChannelAdvisorShippingStatus GetShipWorksShippingStatus(string caShippingStatus)
        {
            switch (caShippingStatus)
            {
                case ShippingStatusCodes.NoChange: return ChannelAdvisorShippingStatus.NoChange;
                case ShippingStatusCodes.Shipped: return ChannelAdvisorShippingStatus.Shipped;
                case ShippingStatusCodes.Unshipped: return ChannelAdvisorShippingStatus.Unshipped;
                case ShippingStatusCodes.PartiallyShipped: return ChannelAdvisorShippingStatus.PartiallyShipped;
                case ShippingStatusCodes.PendingShipment: return ChannelAdvisorShippingStatus.PendingShipment;
                default:
                    // ChannelAdvisor passes shipping status around as a string, so they can add additional
                    // functionality without having applications update their WSDLs. If we get here, CA
                    // has added a new status string, so return an unknown status rather than crashing the
                    // system and not allowing a user to download any orders until we have updated the integration
                    log.Warn(string.Format("An invalid/unknown value ({0}) was encountered for CA shipping status", caShippingStatus));
                    return ChannelAdvisorShippingStatus.Unknown;
            }
        }

        /// <summary>
        /// Gets the prime status based on the shippingClass
        /// </summary>
        public static ChannelAdvisorIsAmazonPrime GetIsPrime(string shippingClass, string carrier)
        {
            if (string.IsNullOrEmpty(shippingClass) || string.IsNullOrEmpty(carrier))
            {
                return ChannelAdvisorIsAmazonPrime.Unknown;
            }

            return shippingClass.IndexOf("Prime", StringComparison.OrdinalIgnoreCase) >= 0 &&
                   carrier.IndexOf("Amazon", StringComparison.OrdinalIgnoreCase) >= 0 ?
                ChannelAdvisorIsAmazonPrime.Yes : ChannelAdvisorIsAmazonPrime.No;
        }

        /// <summary>
        /// Determine the state/province based on the region from CA.
        /// </summary>
        public static string GetStateProvCode(string region)
        {
            // CA will send 001 if they don't know what to do with the region.
            // So we'll just return ""
            return region == "001" ? string.Empty : Geography.GetStateProvCode(region);
        }

        /// <summary>
        /// Gets the country code from ChannelAdvisor
        /// </summary>
        public static string GetCountryCode(string country)
        {
            // in some cases, we've seen CA provide invalid data here
            return string.CompareOrdinal(country, "-1") == 0 ? string.Empty : country;
        }
    }
}
