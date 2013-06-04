using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Fulfillment
{
    /// <summary>
    /// An interface for submitting feedback about items/transactions.
    /// </summary>
    public interface ILeaveFeedbackRequest 
    {
        /// <summary>
        /// Leaves feedback for the given transaction/item.
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="targetUserId">The target user ID.</param>
        /// <param name="commentType">Type of the comment.</param>
        /// <param name="comment">The comment.</param>
        /// <returns>A LeaveFeedbackResponseType object.</returns>
        LeaveFeedbackResponseType LeaveFeedback(string itemId, string transactionId, string targetUserId, CommentTypeCodeType commentType, string comment);
    }
}
