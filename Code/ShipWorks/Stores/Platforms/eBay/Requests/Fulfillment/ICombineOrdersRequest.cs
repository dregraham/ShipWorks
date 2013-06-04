using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Fulfillment
{
    /// <summary>
    /// An interface for submitting requests to combine a group of transactions into one order.
    /// </summary>
    public interface ICombineOrdersRequest
    {
        /// <summary>
        /// Combines the orders in the IEnumerable of transactions provided.
        /// </summary>
        /// <param name="transactionsToCombine">The transactions to combine.</param>
        /// <param name="orderTotal">The order total.</param>
        /// <param name="paymentMethods">The payment methods.</param>
        /// <param name="shippingDetails">The shipping details.</param>
        /// <param name="salesTaxPercent">The sales tax percent.</param>
        /// <param name="taxState">State of the tax.</param>
        /// <param name="isShippingTaxed">if set to <c>true</c> [is shipping taxed].</param>
        /// <returns>An AddOrderResponseType object.</returns>
        AddOrderResponseType CombineOrders(IEnumerable<TransactionType> transactionsToCombine, double orderTotal, IEnumerable<BuyerPaymentMethodCodeType> paymentMethods, decimal shippingCost, string shippingCountryCode, string shippingService, decimal salesTaxPercent, string taxState, bool isShippingTaxed);
    }
}
