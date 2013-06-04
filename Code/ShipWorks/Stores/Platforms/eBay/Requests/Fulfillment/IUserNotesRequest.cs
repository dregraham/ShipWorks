using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Fulfillment
{
    /// <summary>
    /// An interface for sending requests to create notes.
    /// </summary>
    public interface IUserNotesRequest
    {
        /// <summary>
        /// Saves a note for the specified item/transation.
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="noteText">The note text.</param>
        /// <returns>A SetUserNotesResponseType object.</returns>
        SetUserNotesResponseType SaveNote(string itemId, string transactionId, string noteText);
    }
}
