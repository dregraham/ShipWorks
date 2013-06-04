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
    /// The eBay request factory interface.
    /// </summary>
    public interface IRequestFactory
    {
        /// <summary>
        /// Creates the tango authorization request.
        /// </summary>
        /// <param name="license">The license.</param>
        /// <returns>An ITangoAuthorizationRequest object.</returns>
        ITangoAuthorizationRequest CreateTangoAuthorizationRequest(string license);

        /// <summary>
        /// Creates the user info request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An IUserInfoRequest object.</returns>
        IUserInfoRequest CreateUserInfoRequest(TokenData tokenData);

        /// <summary>
        /// Creates the time request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ITimeRequest object.</returns>
        ITimeRequest CreateTimeRequest(TokenData tokenData);

        /// <summary>
        /// Creates the transaction request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ITransactionRequest object.</returns>
        ITransactionRequest CreateTransactionRequest(TokenData tokenData);

        /// <summary>
        /// Creates the combined payment request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ICombinedPaymentRequest object.</returns>
        ICombinedPaymentRequest CreateCombinedPaymentRequest(TokenData tokenData);

        /// <summary>
        /// Creates the selling request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ISellingRequest object.</returns>
        ISellingRequest CreateSellingRequest(TokenData tokenData);

        /// <summary>
        /// Creates the feedback request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An IFeedbackRequest object.</returns>
        IFeedbackRequest CreateFeedbackRequest(TokenData tokenData);

        /// <summary>
        /// Creates the item request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An IItemRequest object.</returns>
        IItemRequest CreateItemRequest(TokenData tokenData);

        /// <summary>
        /// Creates the item transaction request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An IItemTransactionRequest object.</returns>
        IItemTransactionRequest CreateItemTransactionRequest(TokenData tokenData);

        /// <summary>
        /// Creates the leave feedback request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ILeaveFeedbackRequest object.</returns>
        ILeaveFeedbackRequest CreateLeaveFeedbackRequest(TokenData tokenData);

        /// <summary>
        /// Creates the send partner message request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ISendPartnerMessageRequest object.</returns>
        ISendPartnerMessageRequest CreateSendPartnerMessageRequest(TokenData tokenData);

        /// <summary>
        /// Creates the complete sale request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ICompleteSaleRequest object.</returns>
        ICompleteSaleRequest CreateCompleteSaleRequest(TokenData tokenData);

        /// <summary>
        /// Creates the user notes request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An IUserNotesRequest object.</returns>
        IUserNotesRequest CreateUserNotesRequest(TokenData tokenData);

        /// <summary>
        /// Creates the combine orders request.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ICombineOrdersRequest object.</returns>
        ICombineOrdersRequest CreateCombineOrdersRequest(TokenData tokenData);
    }
}
