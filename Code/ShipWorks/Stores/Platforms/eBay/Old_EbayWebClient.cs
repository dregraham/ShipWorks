using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using System.Web;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore;
using System.Xml;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.ApplicationCore.Logging;
using System.Net;
using Interapptive.Shared.Win32;
using System.Web.Services.Protocols;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using ShipWorks.Stores.Platforms.Ebay.Requests.Download;
using ShipWorks.Stores.Platforms.Ebay.Requests.Fulfillment;
using ShipWorks.Stores.Platforms.Ebay.Requests.Authorization;
using ShipWorks.Stores.Platforms.Ebay.Requests.System;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// An implementation of the IEbayWebClient interface.
    /// </summary>
    public class Old_EbayWebClient : IEbayWebClient
    {
        private IRequestFactory requestFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Old_EbayWebClient"/> class.
        /// </summary>
        public Old_EbayWebClient()
            : this(new EbayRequestFactory())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Old_EbayWebClient"/> class.
        /// </summary>
        /// <param name="requestFactory">The request factory.</param>
        public Old_EbayWebClient(IRequestFactory requestFactory)
        {
            this.requestFactory = requestFactory;
        }

        
        /// <summary>
        /// Gets authorization from Tango.
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetTangoAuthorization()
        {
            ITangoAuthorizationRequest tangoRequest = requestFactory.CreateTangoAuthorizationRequest(ShipWorksSession.InstanceID.ToString());
            return tangoRequest.Authorize();
        }

        /// <summary>
        /// Gets the user info.
        /// </summary>
        /// <returns>A GetUserResponseType object.</returns>
        public GetUserResponseType GetUserInfo(string tokenKey)
        {
            // To obtain user information from eBay we just need the token key. In most cases,
            // this will be called during the initial setup/authorization process, so the full
            // TokenData information will not be available yet
            TokenData tokenData = CreateTokenData(tokenKey);
            IUserInfoRequest request = requestFactory.CreateUserInfoRequest(tokenData);

            return request.GetUserInfo();
        }

        /// <summary>
        /// Gets the server time in UTC.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <returns>The UTC time according to eBay.</returns>
        public DateTime GetServerTimeInUtc(string tokenKey)
        {
            TokenData tokenData = CreateTokenData(tokenKey);
            ITimeRequest request = requestFactory.CreateTimeRequest(tokenData);

            return request.GetServerTimeInUtc();
        }

        /// <summary>
        /// Gets the transaction count page.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A GetSellerTransactionsResponseType object.</returns>
        public EbayDownloadSummary GetTransactionSummary(string tokenKey, DateTime startDate, DateTime endDate)
        {
            ValidateDateRange(startDate, endDate);

            TokenData tokenData = CreateTokenData(tokenKey);
            ITransactionRequest request = requestFactory.CreateTransactionRequest(tokenData);

            return request.GetTransactionSummary(startDate, endDate);
        }

        /// <summary>
        /// Downloads the transactions.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An GetSellerTransactionsResponseType containing detailed order/transaction information.</returns>
        public GetSellerTransactionsResponseType DownloadTransactions(string tokenKey, DateTime startDate, DateTime endDate, int pageNumber)
        {
            ValidateDateRange(startDate, endDate);
            ValidatePageNumber(pageNumber);

            TokenData tokenData = CreateTokenData(tokenKey);
            ITransactionRequest request = requestFactory.CreateTransactionRequest(tokenData);

            return request.GetTransactions(startDate, endDate, pageNumber);
        }

        /// <summary>
        /// Gets the payment and page counts for the given date range and order status.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="orderStatus">The order status.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An EbayDownloadSummary object.</returns>
        public EbayDownloadSummary GetPaymentSummary(string tokenKey, OrderStatusCodeType orderStatus, DateTime startDate, DateTime endDate)
        {
            ValidateDateRange(startDate, endDate);
            TokenData tokenData = CreateTokenData(tokenKey);

            ICombinedPaymentRequest request = requestFactory.CreateCombinedPaymentRequest(tokenData);
            EbayDownloadSummary summary = request.GetPaymentSummary(orderStatus, startDate, endDate);

            return summary;
        }

        /// <summary>
        /// Gets the payment and page counts for the given date range for all order statuses.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An EbayDownloadSummary object.</returns>
        public EbayDownloadSummary GetPaymentSummary(string tokenKey, DateTime startDate, DateTime endDate)
        {
            ValidateDateRange(startDate, endDate);
            TokenData tokenData = CreateTokenData(tokenKey);

            ICombinedPaymentRequest request = requestFactory.CreateCombinedPaymentRequest(tokenData);
            EbayDownloadSummary summary = request.GetPaymentSummary(startDate, endDate);

            return summary;
        }

        /// <summary>
        /// Gets the active payments.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="pageToDownload">The page to download.</param>
        /// <returns>A List of OrderType objects.</returns>
        public List<OrderType> GetAllPayments(string tokenKey, DateTime startDate, DateTime endDate, int pageToDownload)
        {
            ValidateDateRange(startDate, endDate);
            TokenData tokenData = CreateTokenData(tokenKey);

            ICombinedPaymentRequest request = requestFactory.CreateCombinedPaymentRequest(tokenData);
            GetOrdersResponseType response = request.GetAllPayments(startDate, endDate, pageToDownload);

            if (response.OrderArray == null)
            {
                // Instantiate an order array, so we don't get a null reference exception
                // when transforming it to a list
                response.OrderArray = new OrderType[] { };
            }

            return response.OrderArray.ToList();
        }

        /// <summary>
        /// Gets the active payments.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A List of OrderType objects.</returns>
        public List<OrderType> GetActivePayments(string tokenKey, DateTime startDate, DateTime endDate)
        {
            ValidateDateRange(startDate, endDate);
            TokenData tokenData = CreateTokenData(tokenKey);

            ICombinedPaymentRequest request = requestFactory.CreateCombinedPaymentRequest(tokenData);
            GetOrdersResponseType response = request.GetActivePayments(startDate, endDate);

            if (response.OrderArray == null)
            {
                // Instantiate an order array, so we don't get a null reference exception
                // when transforming it to a list
                response.OrderArray = new OrderType[] { };
            }

            return response.OrderArray.ToList();
        }

        /// <summary>
        /// Gets the completed payments.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A List of OrderType objects.</returns>
        public List<OrderType> GetCompletedPayments(string tokenKey, DateTime startDate, DateTime endDate)
        {
            ValidateDateRange(startDate, endDate);
            TokenData tokenData = CreateTokenData(tokenKey);

            ICombinedPaymentRequest request = requestFactory.CreateCombinedPaymentRequest(tokenData);
            GetOrdersResponseType response = request.GetCompletedPayments(startDate, endDate);
            
            if (response.OrderArray == null)
            {
                // Instantiate an order array, so we don't get a null reference exception
                // when transforming it to a list
                response.OrderArray = new OrderType[] { };
            }

            return response.OrderArray.ToList();
        }

        /// <summary>
        /// Gets the sold items summary.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="durationInDays">The duration in days.</param>
        /// <returns>An EbayDownloadSummary object</returns>
        public EbayDownloadSummary GetSoldItemsSummary(string tokenKey, int durationInDays)
        {
            TokenData tokenData = CreateTokenData(tokenKey);
            ISellingRequest request = requestFactory.CreateSellingRequest(tokenData);

            // Now that we have our request object make sure we have an acceptable duration
            if (durationInDays < 0 || durationInDays > request.MaximumDurationInDays)
            {
                string message = string.Format("eBay does not permit obtaining sold items for the last {0} days.", durationInDays);
                throw new EbayException(message);
            }

            return request.GetSoldItemsSummary(durationInDays);
        }

        /// <summary>
        /// Gets the sold items.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="durationInDays">The duration in days.</param>
        /// <returns>A list of OrderTransactionType objects; an empty list is returned if no results are provided by eBay.</returns>
        public List<OrderTransactionType> GetSoldItems(string tokenKey, int durationInDays, int pageNumber)
        {
            List<OrderTransactionType> orderTransactions = new List<OrderTransactionType>();

            ValidatePageNumber(pageNumber);

            TokenData tokenData = CreateTokenData(tokenKey);
            ISellingRequest request = requestFactory.CreateSellingRequest(tokenData);

            // Now that we have our request object make sure we have an acceptable duration
            if (durationInDays < 0 || durationInDays > request.MaximumDurationInDays)
            {
                string message = string.Format("eBay does not permit obtaining sold items for the last {0} days.", durationInDays);
                throw new EbayException(message);
            }

            GetMyeBaySellingResponseType response = request.GetSoldItems(durationInDays, pageNumber);
            if (response.SoldList != null && response.SoldList.OrderTransactionArray != null)
            {
                // We have transactions in our response that we want to return
                orderTransactions = response.SoldList.OrderTransactionArray.ToList();
            }

            return orderTransactions;
        }


        /// <summary>
        /// Gets the feedback summary.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <returns>An EbayDownloadSummary object.</returns>
        public EbayDownloadSummary GetFeedbackSummary(string tokenKey)
        {
            TokenData tokenData = CreateTokenData(tokenKey);
            IFeedbackRequest request = requestFactory.CreateFeedbackRequest(tokenData);

            EbayDownloadSummary summary = request.GetFeedbackDownloadSummary();
            if (summary == null)
            {
                summary = new EbayDownloadSummary(0, 1, DateTime.Now, DateTime.Now);
            }

            return summary;
        }

        /// <summary>
        /// Gets the feedback.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>A list of FeedbackDetailType objects.</returns>
        public List<FeedbackDetailType> GetFeedbackDetails(string tokenKey, int pageNumber)
        {
            List<FeedbackDetailType> feedbackDetails = new List<FeedbackDetailType>();
            
            ValidatePageNumber(pageNumber);
            TokenData tokenData = CreateTokenData(tokenKey);

            IFeedbackRequest request = requestFactory.CreateFeedbackRequest(tokenData);
            GetFeedbackResponseType response = request.GetFeedbackDetails(pageNumber);

            if (response.FeedbackDetailArray != null)
            {
                // We have feedback, so update the feedback detail list
                feedbackDetails = response.FeedbackDetailArray.ToList();
            }

            return feedbackDetails;
        }


        /// <summary>
        /// Gets the item details.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="itemId">The item ID.</param>
        /// <returns>An ItemType object.</returns>
        public ItemType GetItemDetails(string tokenKey, string itemId)
        {
            ValidateString(itemId, "An invalid item ID value was provided.");

            TokenData tokenData = CreateTokenData(tokenKey);
            IItemRequest request = requestFactory.CreateItemRequest(tokenData);

            GetItemResponseType response = request.GetItemDetails(itemId);
            if (response.Item == null)
            {
                throw new EbayException(string.Format("Unable to retrieve item ID {0} from eBay.", itemId));
            }

            return response.Item;
        }

        /// <summary>
        /// Gets the transaction details. A null value is returned if the
        /// transaction is not found.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="itemId">The item ID.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <returns>An TransactionType object.</returns>
        public TransactionType GetTransactionDetails(string tokenKey, string itemId, string transactionId)
        {
            ValidateString(itemId, "An invalid item ID value was provided.");
            ValidateString(transactionId, "An invalid transaction ID value was provided."); 
            
            TransactionType transaction = null;
            
            TokenData tokenData = CreateTokenData(tokenKey);
            IItemTransactionRequest request = requestFactory.CreateItemTransactionRequest(tokenData);

            GetItemTransactionsResponseType response = request.GetTransactionDetail(itemId, transactionId);
            if (response.TransactionArray != null)
            {
                transaction = response.TransactionArray[0];
            }

            return transaction;
        }

        /// <summary>
        /// Leaves feedback for the given transaction/item.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="itemId">The item ID.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="targetUserId">The target user ID.</param>
        /// <param name="commentType">Type of the comment.</param>
        /// <param name="comment">The comment.</param>
        public void LeaveFeedback(string tokenKey, string itemId, string transactionId, string targetUserId, CommentTypeCodeType commentType, string comment)
        {
            ValidateString(itemId, "An invalid item ID value was provided.");
            ValidateString(transactionId, "An invalid transaction ID value was provided.");
            ValidateString(targetUserId, "A user ID to leave feedback with was not provided.");

            TokenData tokenData = CreateTokenData(tokenKey);
            ILeaveFeedbackRequest request = requestFactory.CreateLeaveFeedbackRequest(tokenData);

            request.LeaveFeedback(itemId, transactionId, targetUserId, commentType, comment);
        }

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
        public void SendMessageToPartner(string tokenKey, string itemId, string userId, QuestionTypeCodeType messageType, string subject, string message, bool copySender)
        {
            ValidateString(itemId, "An invalid item ID value was provided.");
            ValidateString(userId, "A user ID must be provided to send a message.");

            TokenData tokenData = CreateTokenData(tokenKey);
            ISendPartnerMessageRequest request = requestFactory.CreateSendPartnerMessageRequest(tokenData);

            request.SendMessage(itemId, userId, messageType, subject, message, copySender);
        }


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
        public void CompleteSale(string tokenKey, string itemId, string transactionId, bool? isPaid, bool? isShipped, string trackingNumber, string shippingCarrier)
        {
            ValidateString(itemId, "An invalid item ID value was provided.");
            ValidateString(transactionId, "An invalid transaction ID was provided.");

            TokenData tokenData = CreateTokenData(tokenKey);
            ICompleteSaleRequest request = requestFactory.CreateCompleteSaleRequest(tokenData);

            request.CompleteSale(itemId, transactionId, isPaid, isShipped, trackingNumber, shippingCarrier);
        }


        /// <summary>
        /// Saves the notefor the specified item/transaction.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <param name="itemId">The item ID.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="noteText">The note text.</param>
        public void SaveNote(string tokenKey, string itemId, string transactionId, string noteText)
        {
            // Only validate the item ID here - the transaction ID can be a null value
            ValidateString(itemId, "An invalid item ID value was provided.");

            TokenData tokenData = CreateTokenData(tokenKey);
            IUserNotesRequest request = requestFactory.CreateUserNotesRequest(tokenData);

            request.SaveNote(itemId, transactionId, noteText);
        }

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
        public string CombineOrders(string tokenKey, IEnumerable<TransactionType> transactionsToCombine, double orderTotal, IEnumerable<BuyerPaymentMethodCodeType> paymentMethods, decimal shippingCost, string shippingCountryCode, string shippingService, decimal salesTaxPercent, string taxState, bool isShippingTaxed)
        {
            if (transactionsToCombine == null || transactionsToCombine.Count() < 2)
            {
                throw new EbayException("There must be at least two orders to combine orders through eBay.");
            }

            if (orderTotal < 0)
            {
                throw new EbayException("The order total of the combined orders cannot be less than zero.");
            }

            if (salesTaxPercent < 0)
            {
                throw new EbayException("The sales tax rate of the combined orders cannot be less than zero.");
            }

            if (shippingCost < 0)
            {
                throw new EbayException("The shipping cost of the combined orders cannot be less than zero.");
            }

            TokenData tokenData = CreateTokenData(tokenKey);
            ICombineOrdersRequest request = requestFactory.CreateCombineOrdersRequest(tokenData);

            AddOrderResponseType response = request.CombineOrders(transactionsToCombine, orderTotal, paymentMethods, shippingCost, shippingCountryCode, shippingService, salesTaxPercent, taxState, isShippingTaxed);
            if (string.IsNullOrEmpty(response.OrderID) || response.OrderID == "0")
            {
                throw new EbayException("ShipWorks did not receive a valid order ID from eBay for the combined orders.");
            }

            return response.OrderID;

        }


        /// <summary>
        /// A helper method that validates the string value provided and throws an eBay 
        /// exception with the message provided if the value is null or empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        private static void ValidateString(string value, string exceptionMessage)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new EbayException(exceptionMessage);
            }
        }

        /// <summary>
        /// A helper method that validates the page number of requests.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        private static void ValidatePageNumber(int pageNumber)
        {
            if (pageNumber <= 0)
            {
                string message = string.Format("There was an attempt to download an invalid page of transactions from eBay: page {0}", pageNumber);
                throw new EbayException(message);
            }
        }

        /// <summary>
        /// A helper method that validates the date range of requests.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        private static void ValidateDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new EbayException("An invalid date range was provided; start date cannot be later than the end date.");
            }

            if (endDate.Subtract(startDate).Days > 30)
            {
                throw new EbayException("The date range provided exceeds the maximum allowable date range");
            }
        }

        /// <summary>
        /// A helper method that to create a TokenData object for the given key.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        /// <returns>A TokenData object.</returns>
        private static TokenData CreateTokenData(string tokenKey)
        {
            if (string.IsNullOrEmpty(tokenKey))
            {
                throw new EbayException("An eBay token must be provided.");
            }

            return new TokenData { Token = tokenKey };
        }
    }
}
