using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Client that wraps all connectivity to the eBay API
    /// </summary>
    public class EbayWebClient
    {
        EbayToken token;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayWebClient(EbayToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }

            if (string.IsNullOrWhiteSpace(token.Token))
            {
                throw new ArgumentException("token cannot be blank", "token");
            }

            this.token = token;
        }

        /// <summary>
        /// Get eBay official time in UTC
        /// </summary>
        public DateTime GetOfficialTime()
        {
            EbayOfficialTimeRequest request = new EbayOfficialTimeRequest(token);

            return request.Execute();
        }

        /// <summary>
        /// Get the user informatiopn for the user that is represented by the token
        /// </summary>
        public UserType GetUser()
        {
            EbayGetUserRequest request = new EbayGetUserRequest(token);

            return request.Execute();
        }

        /// <summary>
        /// Get a page of orders from eBay
        /// </summary>
        public GetOrdersResponseType GetOrders(DateTime rangeStart, DateTime rangeEnd, int page)
        {
            EbayGetOrdersRequest request = new EbayGetOrdersRequest(token, rangeStart, rangeEnd, page);

            return request.Execute();
        }

        /// <summary>
        /// Get full item details for the given item listing ID
        /// </summary>
        public ItemType GetItem(string itemID)
        {
            EbayGetItemRequest request = new EbayGetItemRequest(token, itemID);

            return request.Execute();
        }

        /// <summary>
        /// Get feedback that has been left by or for this user
        /// </summary>
        public GetFeedbackResponseType GetFeedback(FeedbackTypeCodeType? feedbackType, int page)
        {
            EbayGetFeedbackRequest request = new EbayGetFeedbackRequest(token, feedbackType, page);

            return request.Execute();
        }

        /// <summary>
        /// Leave feedback for the given item and transaction
        /// </summary>
        public void LeaveFeedback(long itemID, long transactionID, string buyerID, CommentTypeCodeType feedbackType, string feedback)
        {
            EbayLeaveFeedbackRequest request = new EbayLeaveFeedbackRequest(token, itemID, transactionID, buyerID, feedbackType, feedback);

            request.Execute();
        }

        /// <summary>
        /// Send a message for the given item to the buyer
        /// </summary>
        [NDependIgnoreTooManyParams]
        public void SendMessage(long itemID, string buyerID, QuestionTypeCodeType messageType, string subject, string message, bool copySender)
        {
            EbaySendMessageRequest request = new EbaySendMessageRequest(token, itemID, buyerID, messageType, subject, message, copySender);

            request.Execute();
        }

        /// <summary>
        /// Add a note into the buyer's my ebay for the given item
        /// </summary>
        public void AddUserNote(long itemID, long transactionID, string notesText)
        {
            EbaySetUserNotesRequest request = new EbaySetUserNotesRequest(token, itemID, transactionID, notesText);

            request.Execute();
        }

        /// <summary>
        /// Marks a transation as paid (or not) and shipped (or not) on my ebay
        /// </summary>
        [NDependIgnoreTooManyParams]
        public void CompleteSale(long itemID, long transactionID, bool? paid, bool? shipped, string trackingNumber, string shippingCarrier)
        {
            EbayCompleteSaleRequest request = new EbayCompleteSaleRequest(token, itemID, transactionID, paid, shipped, trackingNumber, shippingCarrier);

            request.Execute();
        }

        /// <summary>
        /// Combine the given transactions, specifying the additional costs for the combined order
        /// </summary>
        [NDependIgnoreTooManyParams]
        public long CombineOrders(IEnumerable<TransactionType> transactionsToCombine, double orderTotal, IEnumerable<BuyerPaymentMethodCodeType> paymentMethods, decimal shippingCost, string shippingCountryCode, string shippingService, decimal salesTaxPercent, string taxState, bool isShippingTaxed)
        {
            EbayAddOrderRequest request = new EbayAddOrderRequest(token, transactionsToCombine, orderTotal, paymentMethods, shippingCost, shippingCountryCode, shippingService, salesTaxPercent, taxState, isShippingTaxed);

            return request.Execute();
        }
    }
}
