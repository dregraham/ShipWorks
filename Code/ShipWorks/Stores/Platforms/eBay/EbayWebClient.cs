using System;
using System.Collections.Generic;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Client that wraps all connectivity to the eBay API
    /// </summary>
    [Component]
    public class EbayWebClient : IEbayWebClient
    {
        /// <summary>
        /// Get eBay official time in UTC
        /// </summary>
        public DateTime GetOfficialTime(EbayToken token)
        {
            EbayOfficialTimeRequest request = new EbayOfficialTimeRequest(token);

            return request.Execute();
        }

        /// <summary>
        /// Get the user information for the user that is represented by the token
        /// </summary>
        public UserType GetUser(EbayToken token)
        {
            EbayGetUserRequest request = new EbayGetUserRequest(token);

            return request.Execute();
        }

        /// <summary>
        /// Get a page of orders from eBay
        /// </summary>
        public GetOrdersResponseType GetOrders(EbayToken token, Range<DateTime> range, int page)
        {
            EbayGetOrdersRequest request = new EbayGetOrdersRequest(token, range.Start, range.End, page);

            return request.Execute();
        }

        /// <summary>
        /// Get full item details for the given item listing ID
        /// </summary>
        public ItemType GetItem(EbayToken token, string itemID)
        {
            EbayGetItemRequest request = new EbayGetItemRequest(token, itemID);

            return request.Execute();
        }

        /// <summary>
        /// Get feedback that has been left by or for this user
        /// </summary>
        public GetFeedbackResponseType GetFeedback(EbayToken token, FeedbackTypeCodeType? feedbackType, int page)
        {
            EbayGetFeedbackRequest request = new EbayGetFeedbackRequest(token, feedbackType, page);

            return request.Execute();
        }

        /// <summary>
        /// Leave feedback for the given item and transaction
        /// </summary>
        [NDependIgnoreTooManyParams]
        public void LeaveFeedback(EbayToken token, long itemID, long transactionID, string buyerID, CommentTypeCodeType feedbackType, string feedback)
        {
            EbayLeaveFeedbackRequest request = new EbayLeaveFeedbackRequest(token, itemID, transactionID, buyerID, feedbackType, feedback);

            request.Execute();
        }

        /// <summary>
        /// Send a message for the given item to the buyer
        /// </summary>
        [NDependIgnoreTooManyParams]
        public void SendMessage(EbayToken token, long itemID, string buyerID, QuestionTypeCodeType messageType, string subject, string message, bool copySender)
        {
            EbaySendMessageRequest request = new EbaySendMessageRequest(token, itemID, buyerID, messageType, subject, message, copySender);

            request.Execute();
        }

        /// <summary>
        /// Add a note into the buyer's my ebay for the given item
        /// </summary>
        public void AddUserNote(EbayToken token, long itemID, long transactionID, string notesText)
        {
            EbaySetUserNotesRequest request = new EbaySetUserNotesRequest(token, itemID, transactionID, notesText);

            request.Execute();
        }

        /// <summary>
        /// Marks a transaction as paid (or not) and shipped (or not) on my ebay
        /// </summary>
        [NDependIgnoreTooManyParams]
        public void CompleteSale(EbayToken token, long itemID, long transactionID, bool? paid, bool? shipped, string trackingNumber, string shippingCarrier)
        {
            EbayCompleteSaleRequest request = new EbayCompleteSaleRequest(token, itemID, transactionID, paid, shipped, trackingNumber, shippingCarrier);

            request.Execute();
        }

        /// <summary>
        /// Combine the given transactions, specifying the additional costs for the combined order
        /// </summary>
        [NDependIgnoreTooManyParams]
        public long CombineOrders(EbayToken token, IEnumerable<TransactionType> transactionsToCombine,
            double orderTotal, IEnumerable<BuyerPaymentMethodCodeType> paymentMethods, decimal shippingCost,
            string shippingCountryCode, string shippingService, decimal salesTaxPercent, string taxState, bool isShippingTaxed)
        {
            EbayAddOrderRequest request = new EbayAddOrderRequest(token, transactionsToCombine, orderTotal, paymentMethods, shippingCost, shippingCountryCode, shippingService, salesTaxPercent, taxState, isShippingTaxed);

            return request.Execute();
        }
    }
}
