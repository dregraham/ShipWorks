using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// A data transfer object that encapsulates high level summary for the response to a transaction request.
    /// </summary>
    public class EbayDownloadSummary
    {
        private int numberOfTransactions;
        private int maximumTransactionsPerPage;
        private DateTime startDate;
        private DateTime endDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayDownloadSummary"/> class.
        /// </summary>
        /// <param name="numberOfTransactions">The number of transactions.</param>
        /// <param name="maximumTransactionsPerPage">The maximum number of transactions on a page.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        public EbayDownloadSummary(int numberOfTransactions, int maximumTransactionsPerPage, DateTime startDate, DateTime endDate)
        {
            if (maximumTransactionsPerPage <= 0)
            {
                throw new InvalidOperationException("Maximum number of transaction per page must be a positive number.");
            }

            this.numberOfTransactions = numberOfTransactions;
            this.maximumTransactionsPerPage = maximumTransactionsPerPage;
            this.startDate = startDate;
            this.endDate = endDate;            
        }

        /// <summary>
        /// Gets the number of pages.
        /// </summary>
        public int NumberOfPages 
        {
            get
            {
                // Calculate the total page count based on the page size: subtract 1 from the summation of the 
                // max transactions per page and the total orders before dividing by the max page size to handle
                // to account for integer division (assuming the transactions per page is > 0)
                // (e.g.: (100 + 42 - 1) / 100 = 1 page, 100 + 100 - 1 / 100 = 1 page)
                return (maximumTransactionsPerPage + numberOfTransactions - 1) / maximumTransactionsPerPage;
            } 
        }

        /// <summary>
        /// Gets the number of transactions.
        /// </summary>
        public int NumberOfTransactions
        {
            get { return numberOfTransactions; }
        }

        /// <summary>
        /// Gets the start date.
        /// </summary>
        private DateTime StartDate
        {
            get { return startDate; }
        }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        private DateTime EndDate 
        {
            get { return endDate; }
        }

    }
}
