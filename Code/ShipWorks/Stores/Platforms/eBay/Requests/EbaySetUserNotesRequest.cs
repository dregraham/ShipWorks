using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// Implements the eBay SetUserNotes API
    /// </summary>
    public class EbaySetUserNotesRequest : EbayRequest<SetUserNotesResponseType, SetUserNotesRequestType, SetUserNotesResponseType>
    {
        SetUserNotesRequestType request;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayUserNotesRequest"/> class.
        /// </summary>
        public EbaySetUserNotesRequest(EbayToken token, long itemID, long transactionID, string noteText)
            : base(token, "SetUserNotes")
        {
            request = new SetUserNotesRequestType();

            request.ActionSpecified = true;
            request.Action = SetUserNotesActionCodeType.AddOrUpdate;

            request.ItemID = itemID.ToString();
            request.NoteText = noteText;

            // Only set the transaction ID if there is a valid eBay transaction ID associated with the eBay item;
            // setting a transaction ID to 0 will result in an eBay error
            if (transactionID > 0)
            {
                request.TransactionID = transactionID.ToString();
            }
        }

        /// <summary>
        /// Saves a note for the specified item/transation.
        /// </summary>
        public override SetUserNotesResponseType Execute()
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
