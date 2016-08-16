using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// An implementation of the ILeaveFeedbackRequest interface that is responsible for
    /// making requests to eBay to leave feedback regarding an item/transaction.
    /// </summary>
    public class EbayLeaveFeedbackRequest : EbayRequest<LeaveFeedbackResponseType, LeaveFeedbackRequestType, LeaveFeedbackResponseType>
    {
        LeaveFeedbackRequestType request;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayLeaveFeedbackRequest"/> class.
        /// </summary>
        [NDependIgnoreTooManyParams]
        public EbayLeaveFeedbackRequest(EbayToken token, long itemID, long transactionID, string targetUserID, CommentTypeCodeType commentType, string comment)
            : base(token, "LeaveFeedback")
        {
            request = new LeaveFeedbackRequestType();

            request.ItemID = itemID.ToString();
            request.TransactionID = transactionID.ToString();

            request.CommentType = commentType;
            request.CommentTypeSpecified = true;

            request.TargetUser = targetUserID;
            request.CommentText = comment;
       }

        /// <summary>
        /// Leaves feedback for the given transaction/item.
        /// </summary>
        public override LeaveFeedbackResponseType Execute()
        {
            return SubmitRequest();
        }

        /// <summary>
        /// Gets the specific eBay request (GetUserRequestType, GetOrdersRequestType, etc.) containing any parameter data necessary to make the request.
        /// </summary>
        protected override AbstractRequestType CreateRequest()
        {
            return request;
        }
    }
}
