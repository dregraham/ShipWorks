using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.Requests.Download;
using ShipWorks.Stores.Platforms.Ebay.Requests.Fulfillment;
using ShipWorks.Stores.Platforms.Ebay.Requests.Authorization;
using ShipWorks.Stores.Platforms.Ebay.Requests.System;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// An implementation of the IRequestFactory interface that creates request objects
    /// for interacting with eBay and Tango.
    /// </summary>
    public class EbayRequestFactory : IRequestFactory
    {
        /// <summary>
        /// Creates the user info request.
        /// </summary>
        /// <returns></returns>
        public IUserInfoRequest CreateUserInfoRequest(TokenData tokenData)
        {
            return new EbayUserInfoRequest(tokenData);
        }

        /// <summary>
        /// Creates the tango authorization request.
        /// </summary>
        /// <returns></returns>
        public ITangoAuthorizationRequest CreateTangoAuthorizationRequest(string license)
        {
            return new EbayTangoAuthorizationRequest(license);
        }

        /// <summary>
        /// Creates the time request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ITimeRequest object.</returns>
        public ITimeRequest CreateTimeRequest(TokenData tokenData)
        {
            return new EbayTimeRequest(tokenData);
        }

        /// <summary>
        /// Creates the transaction request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ITransactionRequest object.</returns>
        public ITransactionRequest CreateTransactionRequest(TokenData tokenData)
        {
            return new EbayTransactionRequest(tokenData);
        }


        /// <summary>
        /// Creates the combined payment request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ICombinedPaymentRequest object.</returns>
        public ICombinedPaymentRequest CreateCombinedPaymentRequest(TokenData tokenData)
        {
            return new EbayCombinedPaymentRequest(tokenData);
        }


        /// <summary>
        /// Creates the selling request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ISellingRequest object.</returns>
        public ISellingRequest CreateSellingRequest(TokenData tokenData)
        {
            return new EbaySellingRequest(tokenData);
        }


        /// <summary>
        /// Creates the feedback request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An IFeedbackRequest object.</returns>
        public IFeedbackRequest CreateFeedbackRequest(TokenData tokenData)
        {
            return new EbayFeedbackRequest(tokenData);
        }


        /// <summary>
        /// Creates the item request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>
        /// An IItemRequest object.
        /// </returns>
        public IItemRequest CreateItemRequest(TokenData tokenData)
        {
            return new EbayItemRequest(tokenData);
        }


        /// <summary>
        /// Creates the item transaction request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An IItemTransactionRequest object.</returns>
        public IItemTransactionRequest CreateItemTransactionRequest(TokenData tokenData)
        {
            return new EbayItemTransactionRequest(tokenData);
        }


        /// <summary>
        /// Creates the leave feedback request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ILeaveFeedbackRequest object.</returns>
        public ILeaveFeedbackRequest CreateLeaveFeedbackRequest(TokenData tokenData)
        {
            return new EbayLeaveFeedbackRequest(tokenData);
        }


        /// <summary>
        /// Creates the send partner message request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ISendPartnerMessageRequest object.\</returns>
        public ISendPartnerMessageRequest CreateSendPartnerMessageRequest(TokenData tokenData)
        {
            return new EbaySendMessageToPartnerRequest(tokenData);
        }


        /// <summary>
        /// Creates the complete sale request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ICompleteSaleRequest object.</returns>
        public ICompleteSaleRequest CreateCompleteSaleRequest(TokenData tokenData)
        {
            return new EbayCompleteSaleRequest(tokenData);
        }

        /// <summary>
        /// Creates the user notes request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An IUserNotesRequest object.</returns>
        public IUserNotesRequest CreateUserNotesRequest(TokenData tokenData)
        {
            return new EbayUserNotesRequest(tokenData);
        }

        /// <summary>
        /// Creates the combine orders request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ICombineOrdersRequest object.</returns>
        public ICombineOrdersRequest CreateCombineOrdersRequest(TokenData tokenData)
        {
            return new EbayCombineOrdersRequest(tokenData);
        }
    }
}
