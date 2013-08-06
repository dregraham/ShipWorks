using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.Requests;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Download
{
    /// <summary>
    /// An implementation of the ICombinedPaymentRequest interface that is responsible for
    /// making requests to eBay to download combined payment data.
    /// </summary>
    public class EbayCombinedPaymentRequest : EbayRequest, ICombinedPaymentRequest
    {
        // Set the orders per page to 10 per eBay support due to eBay timeouts and slow response times with this service call
        private const int MaximumOrdersPerPage = 25;
        private GetOrdersRequestType request; 

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayCombinedPaymentRequest"/> class.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        public EbayCombinedPaymentRequest(TokenData tokenData)
            : base(tokenData, "GetOrders")
        {
            request = new GetOrdersRequestType();

            request.CreateTimeFromSpecified = true;
            request.CreateTimeToSpecified = true;
            request.OrderRole = TradingRoleCodeType.Seller;
            request.OrderRoleSpecified = true;
            request.OrderStatusSpecified = true;
            request.DetailLevel = new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll };
            request.Pagination = new PaginationType
            {
                EntriesPerPageSpecified = true
            };
        }

        /// <summary>
        /// Gets high level, summary information if payments of a particular type were to be downloaded with the given date range.
        /// </summary>
        /// <param name="orderStatus">The order status.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An EbayDownloadSummary object.</returns>
        public EbayDownloadSummary GetPaymentSummary(OrderStatusCodeType orderStatus, DateTime startDate, DateTime endDate)
        {
            request.CreateTimeFrom = startDate;
            request.CreateTimeTo = endDate;
            request.OrderStatus = orderStatus;
            request.Pagination.EntriesPerPage = 1;
            request.Pagination.EntriesPerPageSpecified = true;

            GetOrdersResponseType response = SubmitRequest() as GetOrdersResponseType;
            if (response == null)
            {
                throw new EbayException("Unable to obtain payment information from eBay.");
            }

            return new EbayDownloadSummary(response.PaginationResult.TotalNumberOfEntries, MaximumOrdersPerPage, startDate, endDate);
        }

        /// <summary>
        /// Gets high level, summary information all payment types were to be downloaded with the given date range.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An EbayDownloadSummary object.</returns>
        public EbayDownloadSummary GetPaymentSummary(DateTime startDate, DateTime endDate)
        {
            request.CreateTimeFrom = startDate;
            request.CreateTimeTo = endDate;
            request.OrderStatus = OrderStatusCodeType.All;
            request.Pagination.EntriesPerPage = 1;
            request.Pagination.EntriesPerPageSpecified = true;

            GetOrdersResponseType response = SubmitRequest() as GetOrdersResponseType;
            if (response == null)
            {
                throw new EbayException("Unable to obtain payment information from eBay.");
            }

            return new EbayDownloadSummary(response.PaginationResult.TotalNumberOfEntries, MaximumOrdersPerPage, startDate, endDate);
        }

        /// <summary>
        /// Gets the active payments.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A GetOrdersResponseType object.</returns>
        public GetOrdersResponseType GetActivePayments(DateTime startDate, DateTime endDate)
        {
            request.CreateTimeFrom = startDate;
            request.CreateTimeTo = endDate;
            request.OrderStatus = OrderStatusCodeType.Active;
            request.Pagination.EntriesPerPage = MaximumOrdersPerPage;

            GetOrdersResponseType response = SubmitRequest() as GetOrdersResponseType;
            if (response == null)
            {
                throw new EbayException("Unable to obtain active payments from eBay.");
            }

            return response; 
        }

        /// <summary>
        /// Gets the completed payments.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A GetOrdersResponseType object.</returns>
        public GetOrdersResponseType GetCompletedPayments(DateTime startDate, DateTime endDate)
        {
            request.CreateTimeFrom = startDate;
            request.CreateTimeTo = endDate;
            request.OrderStatus = OrderStatusCodeType.Completed;
            request.Pagination.EntriesPerPage = MaximumOrdersPerPage;

            GetOrdersResponseType response = SubmitRequest() as GetOrdersResponseType;
            if (response == null)
            {
                throw new EbayException("Unable to obtain completed payments from eBay.");
            }

            return response; 
        }

        /// <summary>
        /// Gets the active payments.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="pageToDownload">The page to download.</param>
        /// <returns>A GetOrdersResponseType object.</returns>
        public GetOrdersResponseType GetAllPayments(DateTime startDate, DateTime endDate, int pageToDownload)
        {
            request.CreateTimeFrom = startDate;
            request.CreateTimeTo = endDate;
            request.OrderStatus = OrderStatusCodeType.All;
            request.Pagination.PageNumberSpecified = true;
            request.Pagination.PageNumber = pageToDownload;
            request.Pagination.EntriesPerPage = MaximumOrdersPerPage;

            GetOrdersResponseType response = SubmitRequest() as GetOrdersResponseType;
            if (response == null)
            {
                throw new EbayException("Unable to obtain active payments from eBay.");
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
            return "GetOrders";
        }

        /// <summary>
        /// Gets the specific eBay request (GetUserRequestType, GetOrdersRequestType, etc.) containing any parameter data necessary to make the request.
        /// </summary>
        /// <returns>An GetOrdersRequestType object.</returns>
        public override AbstractRequestType GetEbayRequest()
        {
            return request;
        }
    }
}
