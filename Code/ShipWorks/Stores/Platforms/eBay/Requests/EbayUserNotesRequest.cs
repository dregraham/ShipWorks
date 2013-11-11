using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// An implementation of the IUserNotesRequest interface that is responsible for
    /// making requests to eBay to create notes regarding an item/transaction.
    /// </summary>
    public class EbayUserNotesRequest : EbayRequest, IUserNotesRequest
    {
        private SetUserNotesRequestType request;


        /// <summary>
        /// Initializes a new instance of the <see cref="EbayUserNotesRequest"/> class.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        public EbayUserNotesRequest(TokenData tokenData)
            : base(tokenData, "SetUserNotes")
        {
            request = new SetUserNotesRequestType()
            {
                ActionSpecified = true,
                Action = SetUserNotesActionCodeType.AddOrUpdate
            };
        }

        /// <summary>
        /// Saves a note for the specified item/transation.
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="noteText">The note text.</param>
        /// <returns>A SetUserNotesResponseType object.</returns>
        public SetUserNotesResponseType SaveNote(string itemId, string transactionId, string noteText)
        {
            request.ItemID = itemId;
            request.TransactionID = transactionId;
            request.NoteText = noteText;

            SetUserNotesResponseType response = SubmitRequest() as SetUserNotesResponseType;
            if (response == null)
            {
                string message = string.Format("Unable to save a note to eBay for item {0}, transaction {1}", itemId, transactionId == null ? "N/A" : transactionId);
                throw new EbayException(message);
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
            return "SetUserNotes";
        }

        /// <summary>
        /// Gets the specific eBay request (GetUserRequestType, GetOrdersRequestType, etc.) containing any parameter data necessary to make the request.
        /// </summary>
        /// <returns>A SetUserNotesRequestType object.</returns>
        public override AbstractRequestType GetEbayRequest()
        {
            return request; 
        }
    }
}
