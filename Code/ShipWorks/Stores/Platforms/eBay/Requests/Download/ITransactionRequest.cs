using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Download
{
    /// <summary>
    /// An interface for downloading transaction data.
    /// </summary>
    public interface ITransactionRequest
    {
        /// <summary>
        /// Gets high level, summary information if transactions were to be downloaded with the given date range.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An EbayDownloadSummary object.</returns>
        EbayDownloadSummary GetTransactionSummary(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets the transactions.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>An GetSellerTransactionsResponseType object containing detailed information about orders/transactions.</returns>
        GetSellerTransactionsResponseType GetTransactions(DateTime startDate, DateTime endDate, int pageNumber);
    }
}
