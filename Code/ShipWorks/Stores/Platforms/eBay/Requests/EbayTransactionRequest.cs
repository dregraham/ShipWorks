using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// An implementation of the ITransactionRequest interface that is responsible for
    /// making requests to eBay to obtain transaction data.
    /// </summary>
    public class EbayTransactionRequest : EbayRequest, ITransactionRequest
    {
        private const int MaximumOrdersPerRequest = 25;

        private GetSellerTransactionsRequestType transactionRequest;        

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayTransactionRequest"/> class.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        public EbayTransactionRequest(TokenData tokenData)
            : base(tokenData, "GetSellerTransactions")
        {
            InitalizeTransactionCountRequest();
        }

        /// <summary>
        /// A helper method to setup the transaction request for obtaining transaction counts.
        /// </summary>
        private void InitalizeTransactionCountRequest()
        {
            transactionRequest = new GetSellerTransactionsRequestType()
            {                
                Pagination = new PaginationType()
                {
                    EntriesPerPageSpecified = true,
                    PageNumber = 1,
                    PageNumberSpecified = true
                },

                ModTimeFromSpecified = true,
                ModTimeToSpecified = true,
            };
        }

        /// <summary>
        /// Gets high level, summary information if transactions were to be downloaded with the given date range.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An EbayDownloadSummary object.</returns>
        public EbayDownloadSummary GetTransactionSummary(DateTime startDate, DateTime endDate)
        {
            // Just need to set the date range on the transaction request before submitting it to eBay and 
            // set the paging info to return minimal information
            transactionRequest.ModTimeFrom = startDate;
            transactionRequest.ModTimeTo = endDate;
            transactionRequest.Pagination.PageNumber = 1;
            transactionRequest.DetailLevel = new DetailLevelCodeType[] { DetailLevelCodeType.ItemReturnDescription };
            transactionRequest.Pagination.EntriesPerPage = 1;

            GetSellerTransactionsResponseType response = SubmitRequest() as GetSellerTransactionsResponseType;
            if (response == null)
            {
                throw new EbayException("Unable to obtain transactions from eBay.");
            }

            // Construct a summary object based on the response and our request criteria; NOTE: we're using 
            // the maximum orders per request value for the transactions per page because that is what will
            // be used when we actually download the orders.
            return new EbayDownloadSummary(response.PaginationResult.TotalNumberOfPages, MaximumOrdersPerRequest, startDate, endDate);
        }

        /// <summary>
        /// Gets the transactions.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>An GetSellerTransactionsResponseType object containing detailed information about orders/transactions.</returns>
        public GetSellerTransactionsResponseType GetTransactions(DateTime startDate, DateTime endDate, int pageNumber)
        {
            // Just need to set the date range on the transaction request before submitting it to eBay and 
            // set the paging info, so return as much data as possible
            transactionRequest.ModTimeFrom = startDate;
            transactionRequest.ModTimeTo = endDate;
            transactionRequest.Pagination.PageNumber = pageNumber;
            transactionRequest.DetailLevel = new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll };
            transactionRequest.Pagination.EntriesPerPage = MaximumOrdersPerRequest;

            GetSellerTransactionsResponseType response = SubmitRequest() as GetSellerTransactionsResponseType;
            if (response == null)
            {
                throw new EbayException("Unable to obtain transactions from eBay.");
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
            return "GetSellerTransactions";
        }

        /// <summary>
        /// Gets the specific eBay request (GetUserRequestType, GetOrdersRequestType, etc.) containing any parameter data necessary to make the request.
        /// </summary>
        /// <returns>An GetSellerTransactionsRequestType object.</returns>
        public override AbstractRequestType GetEbayRequest()
        {
            return transactionRequest;
        }
    }
}
