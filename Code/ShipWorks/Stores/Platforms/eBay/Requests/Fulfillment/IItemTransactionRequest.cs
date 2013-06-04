using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Fulfillment
{
    /// <summary>
    /// An interface for submitting requests to obtain information transaction details based on an item.
    /// </summary>
    public interface IItemTransactionRequest
    {
        /// <summary>
        /// Gets the transaction detail for a given item and transaction. (The eBay API does
        /// not have a way to get a transaction from a transaction ID.)
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <returns>A GetItemTransactionsResponseType object.</returns>
        GetItemTransactionsResponseType GetTransactionDetail(string itemId, string transactionId);
    }
}
