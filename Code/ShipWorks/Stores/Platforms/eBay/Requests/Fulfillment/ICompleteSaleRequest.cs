using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Fulfillment
{
    /// <summary>
    /// An interface for submitting requests to update an item/transaction as paid or shipped.
    /// </summary>
    public interface ICompleteSaleRequest
    {
        /// <summary>
        /// Completes the sale for the given item/transaction.
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="isPaid">if set to <c>true</c> [is paid].</param>
        /// <param name="isShipped">if set to <c>true</c> [is shipped].</param>
        /// <param name="trackingNumber">The tracking number.</param>
        /// <param name="shippingCarrier">The carrier.</param>
        /// <returns>An CompleteSaleResponseType object.</returns>
        CompleteSaleResponseType CompleteSale(string itemId, string transactionId, bool? isPaid, bool? isShipped, string trackingNumber, string shippingCarrier);
    }
}
