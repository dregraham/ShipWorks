using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Fulfillment
{
    /// <summary>
    /// An interface for submitting requests to send a message to a buyer/partner.
    /// </summary>
    public interface ISendPartnerMessageRequest
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="ebayItemId">The ebay item ID.</param>
        /// <param name="userId">The user ID.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        /// <param name="copySender">if set to <c>true</c> [copy sender].</param>
        /// <returns>An AddMemberMessageAAQToPartnerResponseType object.</returns>
        AddMemberMessageAAQToPartnerResponseType SendMessage(string ebayItemId, string userId, WebServices.QuestionTypeCodeType messageType, string subject, string message, bool copySender);
    }
}
