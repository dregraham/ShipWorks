using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// An interface for issuing eBay related web requests.
    /// </summary>
    public interface IEbayWebClient
    {
        /// <summary>
        /// Gets authorization from Tango.
        /// </summary>
        /// <returns></returns>
        XmlDocument GetTangoAuthorization();

        /// <summary>
        /// Gets the user info.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <returns>A GetUserResponseType object.</returns>
        GetUserResponseType GetUserInfo(string tokenKey);

        /// <summary>
        /// Gets the server time in UTC.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <returns>The UTC time according to eBay.</returns>
        DateTime GetServerTimeInUtc(string tokenKey);

        /// <summary>
        /// Gets the transaction and page counts for the given date range.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An EbayDownloadSummary object</returns>
        EbayDownloadSummary GetTransactionSummary(string tokenKey, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Downloads the transactions.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>An GetSellerTransactionsResponseType containing detailed order/transaction information.</returns>
        GetSellerTransactionsResponseType DownloadTransactions(string tokenKey, DateTime startDate, DateTime endDate, int pageNumber);

        /// <summary>
        /// Gets the payment and page counts for the given date range and order status.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="orderStatus">The order status.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An EbayDownloadSummary object.</returns>
        EbayDownloadSummary GetPaymentSummary(string tokenKey, OrderStatusCodeType orderStatus, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets the payment and page counts for the given date range for all order statuses.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An EbayDownloadSummary object.</returns>
        EbayDownloadSummary GetPaymentSummary(string tokenKey, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets the active payments.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A List of OrderType objects.</returns>
        List<OrderType> GetActivePayments(string tokenKey, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets the completed payments.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A List of OrderType objects.</returns>
        List<OrderType> GetCompletedPayments(string tokenKey, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all payments.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="pageToDownload">The page to download.</param>
        /// <returns>A List of OrderType objects.</returns>
        List<OrderType> GetAllPayments(string tokenKey, DateTime startDate, DateTime endDate, int pageToDownload);

        /// <summary>
        /// Gets the sold items summary.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="durationInDays">The duration in days.</param>
        /// <returns>An EbayDownloadSummary object</returns>
        EbayDownloadSummary GetSoldItemsSummary(string tokenKey, int durationInDays);

        /// <summary>
        /// Gets the sold items.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="durationInDays">The duration in days.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>A list of OrderTransactionType objects.</returns>
        List<OrderTransactionType> GetSoldItems(string tokenKey, int durationInDays, int pageNumber);

        /// <summary>
        /// Gets the feedback summary.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <returns>An EbayDownloadSummary object.</returns>
        EbayDownloadSummary GetFeedbackSummary(string tokenKey);

        /// <summary>
        /// Gets the feedback.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>A list of FeedbackDetailType objects.</returns>
        List<FeedbackDetailType> GetFeedbackDetails(string tokenKey, int pageNumber);

        /// <summary>
        /// Gets the item details.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="itemId">The item ID.</param>
        /// <returns>An ItemType object.</returns>
        ItemType GetItemDetails(string tokenKey, string itemId);

        /// <summary>
        /// Gets the transaction details. A null value is returned if the
        /// transaction is not found.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="itemId">The item ID.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <returns>An TransactionType object.</returns>
        TransactionType GetTransactionDetails(string tokenKey, string itemId, string transactionId);

        /// <summary>
        /// Leaves feedback for the given transaction/item.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="itemId">The item ID.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="targetUserId">The target user ID.</param>
        /// <param name="commentType">Type of the comment.</param>
        /// <param name="comment">The comment.</param>
        void LeaveFeedback(string tokenKey, string itemId, string transactionId, string targetUserId, CommentTypeCodeType commentType, string comment);

        /// <summary>
        /// Sends a message to partner through eBay.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="itemId">The item ID.</param>
        /// <param name="userId">The user ID.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        /// <param name="copySender">if set to <c>true</c> [copy sender].</param>
        void SendMessageToPartner(string tokenKey, string itemId, string userId, QuestionTypeCodeType messageType, string subject, string message, bool copySender);

        /// <summary>
        /// Sends a request to eBay to completes the sale for the given item/transaction.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="itemId">The item ID.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="isPaid">if set to <c>true</c> [is paid].</param>
        /// <param name="isShipped">if set to <c>true</c> [is shipped].</param>
        /// <param name="trackingNumber">The tracking number.</param>
        /// <param name="shippingCarrier">The shipping carrier.</param>
        void CompleteSale(string tokenKey, string itemId, string transactionId, bool? isPaid, bool? isShipped, string trackingNumber, string shippingCarrier);

        /// <summary>
        /// Saves the notefor the specified item/transaction.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="itemId">The item ID.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="noteText">The note text.</param>
        void SaveNote(string tokenKey, string itemId, string transactionId, string noteText);

        /// <summary>
        /// Combines the given orders/transactions into one order within the eBay system.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="transactionsToCombine">The transactions to combine.</param>
        /// <param name="orderTotal">The order total.</param>
        /// <param name="paymentMethods">The payment methods.</param>
        /// <param name="shippingCost">The shipping cost.</param>
        /// <param name="shippingCountryCode">The shipping country code.</param>
        /// <param name="shippingService">The shipping service.</param>
        /// <param name="salesTaxPercent">The sales tax percent.</param>
        /// <param name="taxState">State of the tax.</param>
        /// <param name="isShippingTaxed">if set to <c>true</c> [is shipping taxed].</param>
        /// <returns>A string representing the new Order ID from eBay for the combined order.</returns>
        string CombineOrders(string tokenKey, IEnumerable<TransactionType> transactionsToCombine, double orderTotal, IEnumerable<BuyerPaymentMethodCodeType> paymentMethods, decimal shippingCost, string shippingCountryCode, string shippingService, decimal salesTaxPercent, string taxState, bool isShippingTaxed);
    }
}
