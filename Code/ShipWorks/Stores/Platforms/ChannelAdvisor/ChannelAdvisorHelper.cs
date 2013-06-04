using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Constants;
using log4net;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Helper/Utility methods for CA
    /// </summary>
    static class ChannelAdvisorHelper
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
    }
}
