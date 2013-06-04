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
    /// An implementation of the ISendPartnerMessageRequest interface that is responsible for
    /// making requests to eBay to send a message to a buyer regarding an item/transaction.
    /// </summary>
    public class EbaySendMessageToPartnerRequest : EbayRequest, ISendPartnerMessageRequest
    {
        AddMemberMessageAAQToPartnerRequestType request;

        public EbaySendMessageToPartnerRequest(TokenData tokenData)
            : base(tokenData, "AddMemberMessageAAQToPartner")
        {
            request = new AddMemberMessageAAQToPartnerRequestType()
            {
                MemberMessage = new MemberMessageType()
                {
                    EmailCopyToSenderSpecified = true,
                    QuestionTypeSpecified = true
                }
            };
        }


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
        public AddMemberMessageAAQToPartnerResponseType SendMessage(string ebayItemId, string userId, QuestionTypeCodeType messageType, string subject, string message, bool copySender)
        {
            request.ItemID = ebayItemId;
            request.MemberMessage.RecipientID = new string[] { userId };
            request.MemberMessage.EmailCopyToSender = copySender;
            request.MemberMessage.QuestionType = messageType;
            request.MemberMessage.Subject = subject;
            request.MemberMessage.Body = message;

            AddMemberMessageAAQToPartnerResponseType response = SubmitRequest() as AddMemberMessageAAQToPartnerResponseType;
            if (response == null)
            {
                throw new EbayException(string.Format("Unable to send a message through eBay to {0}.", userId));
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
            return "AddMemberMessageAAQToPartner";
        }

        /// <summary>
        /// Gets the specific eBay request (GetUserRequestType, GetOrdersRequestType, etc.) containing any parameter data necessary to make the request.
        /// </summary>
        /// <returns>An AddMemberMessageAAQToPartnerRequestType object.</returns>
        public override AbstractRequestType GetEbayRequest()
        {
            return request;
        }
    }
}
