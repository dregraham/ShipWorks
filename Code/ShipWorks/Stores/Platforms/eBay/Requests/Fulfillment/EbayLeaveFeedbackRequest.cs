using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.Requests;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Fulfillment
{
    /// <summary>
    /// An implementation of the ILeaveFeedbackRequest interface that is responsible for
    /// making requests to eBay to leave feedback regarding an item/transaction.
    /// </summary>
    public class EbayLeaveFeedbackRequest : EbayRequest, ILeaveFeedbackRequest
    {
        private LeaveFeedbackRequestType request; 

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayLeaveFeedbackRequest"/> class.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        public EbayLeaveFeedbackRequest(TokenData tokenData)
            : base(tokenData, "LeaveFeedback")
        {
            request = new LeaveFeedbackRequestType()
            {
                CommentTypeSpecified = true
            };
        }

        /// <summary>
        /// Leaves feedback for the given transaction/item.
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="targetUserId">The target user ID.</param>
        /// <param name="commentType">Type of the comment.</param>
        /// <param name="comment">The comment.</param>
        /// <returns>A LeaveFeedbackResponseType object.</returns>
        public LeaveFeedbackResponseType LeaveFeedback(string itemId, string transactionId, string targetUserId, CommentTypeCodeType commentType, string comment)
        {
            request.ItemID = itemId;
            request.TransactionID = transactionId;
            request.CommentType = commentType;
            request.TargetUser = targetUserId;
            request.CommentText = comment;

            LeaveFeedbackResponseType response = SubmitRequest() as LeaveFeedbackResponseType;
            if (response == null)
            {
                throw new EbayException("An error occurred communicating with eBay when leaving feedback.");
            }

            return response;
        }

        /// <summary>
        /// Gets the name of the call as it is known to eBay. This value gets used
        /// as a query string parameter sent to eBay.
        /// </summary>
        /// <returns></returns>
        public override string GetEbayCallName()
        {
            return "LeaveFeedback";
        }

        /// <summary>
        /// Gets the specific eBay request (GetUserRequestType, GetOrdersRequestType, etc.) containing any parameter data necessary to make the request.
        /// </summary>
        /// <returns>
        /// An AbstractRequestType object.
        /// </returns>
        public override AbstractRequestType GetEbayRequest()
        {
            return request;
        }
    }
}
