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
    /// Implements the eBay API for sending messages 
    /// </summary>
    public class EbaySendMessageRequest : EbayRequest<AddMemberMessageAAQToPartnerResponseType, AddMemberMessageAAQToPartnerRequestType, AddMemberMessageAAQToPartnerResponseType>
    {
        AddMemberMessageAAQToPartnerRequestType request;

        [NDependIgnoreTooManyParams]
        public EbaySendMessageRequest(EbayToken token, long itemID, string buyerID, QuestionTypeCodeType messageType, string subject, string message, bool copySender)
            : base(token, "AddMemberMessageAAQToPartner")
        {
            request = new AddMemberMessageAAQToPartnerRequestType()
                {
                    MemberMessage = new MemberMessageType()
                };

            request.ItemID = itemID.ToString();
            request.MemberMessage.RecipientID = new string[] { buyerID };

            request.MemberMessage.EmailCopyToSender = copySender;
            request.MemberMessage.EmailCopyToSenderSpecified = true;

            request.MemberMessage.QuestionType = messageType;
            request.MemberMessage.QuestionTypeSpecified = true;

            request.MemberMessage.Subject = subject;
            request.MemberMessage.Body = message;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        public override AddMemberMessageAAQToPartnerResponseType Execute()
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
