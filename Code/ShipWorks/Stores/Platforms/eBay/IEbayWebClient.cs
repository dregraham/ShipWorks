using System;
using System.Collections.Generic;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Client that wraps all connectivity to the eBay API
    /// </summary>
    public interface IEbayWebClient
    {
        /// <summary>
        /// Get eBay official time in UTC
        /// </summary>
        DateTime GetOfficialTime(EbayToken token);

        /// <summary>
        /// Get the user information for the user that is represented by the token
        /// </summary>
        UserType GetUser(EbayToken token);

        /// <summary>
        /// Get a page of orders from eBay
        /// </summary>
        GetOrdersResponseType GetOrders(EbayToken token, Range<DateTime> range, int page);

        /// <summary>
        /// Get full item details for the given item listing ID
        /// </summary>
        ItemType GetItem(EbayToken token, string itemID);

        /// <summary>
        /// Get feedback that has been left by or for this user
        /// </summary>
        GetFeedbackResponseType GetFeedback(EbayToken token, FeedbackTypeCodeType? feedbackType, int page);

        /// <summary>
        /// Leave feedback for the given item and transaction
        /// </summary>
        [NDependIgnoreTooManyParams]
        void LeaveFeedback(EbayToken token, long itemID, long transactionID, string buyerID, CommentTypeCodeType feedbackType, string feedback);

        /// <summary>
        /// Send a message for the given item to the buyer
        /// </summary>
        [NDependIgnoreTooManyParams]
        void SendMessage(EbayToken token, long itemID, string buyerID, QuestionTypeCodeType messageType, string subject, string message, bool copySender);

        /// <summary>
        /// Add a note into the buyer's my ebay for the given item
        /// </summary>
        void AddUserNote(EbayToken token, long itemID, long transactionID, string notesText);

        /// <summary>
        /// Marks a transaction as paid (or not) and shipped (or not) on my ebay
        /// </summary>
        [NDependIgnoreTooManyParams]
        void CompleteSale(EbayToken token, long itemID, long transactionID, bool? paid, bool? shipped, string trackingNumber, string shippingCarrier);

        /// <summary>
        /// Combine the given transactions, specifying the additional costs for the combined order
        /// </summary>
        [NDependIgnoreTooManyParams]
        long CombineOrders(EbayToken token, IEnumerable<TransactionType> transactionsToCombine,
            double orderTotal, IEnumerable<BuyerPaymentMethodCodeType> paymentMethods, decimal shippingCost,
            string shippingCountryCode, string shippingService, decimal salesTaxPercent, string taxState, bool isShippingTaxed);
    }
}