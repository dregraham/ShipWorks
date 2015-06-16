using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders;
using System.Text;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response;
using ShipWorks.Stores.Platforms.Newegg.Enums;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download
{
    /// <summary>
    /// An implementation of the IDownloadOrderRequest that hits the Newegg API.
    /// </summary>
    public class DownloadOrdersRequest : IDownloadOrderRequest
    {
        // The skeleton of the URL for submitting download requests to
        public const string RequestUrl = "{0}/ordermgmt/order/orderinfo?sellerid={1}";

        Credentials credentials;
        INeweggRequest request;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadOrdersRequest"/> class.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        public DownloadOrdersRequest(Credentials credentials)
            : this(credentials, new NeweggHttpRequest())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadOrdersRequest"/> class.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="request">The request.</param>
        public DownloadOrdersRequest(Credentials credentials, INeweggRequest request)
        {
            this.credentials = credentials;
            this.request = request;
        }

        /// <summary>
        /// Gets the maximum number of results that will appear on a single page.
        /// </summary>
        /// <value>The size of the max page.</value>
        public int MaxPageSize
        {
            // The max page size per the Newegg docs is 100
            get { return 100; }
        }

        /// <summary>
        /// Gets the download info for a particular date range.
        /// </summary>
        /// <param name="utcFromDate">The UTC from date.</param>
        /// <param name="utcToDate">The UTC to date.</param>
        /// <param name="orderType">Type of the order.</param>
        /// <returns>A DownloadInfo object.</returns>
        public DownloadInfo GetDownloadInfo(DateTime utcFromDate, DateTime utcToDate, NeweggOrderType orderType)
        {
            // We just want some high level info about all that would be downloaded, so
            // just submit a request to get the first page with a max page size of 1
            // so we pull down the least amount of data (Newegg does not have a method
            // to obtain information about order quantities)
            string requestBody = GetRequestBody(utcFromDate, utcToDate, 1, 1, orderType);
            DownloadResult downloadResult = GetDownloadResponse(requestBody); 
           
            DownloadInfo info = new DownloadInfo 
            { 
                TotalOrders = downloadResult.Body.PageInfo.RecordCount, 
                StartDate = utcFromDate,
                EndDate = utcToDate,
                PageCount = new PageCalculator().CalculatePageCount(downloadResult.Body.PageInfo.RecordCount, MaxPageSize)
            };

            return info;
        }

        /// <summary>
        /// Gets the download info for a set of orders.
        /// </summary>
        /// <param name="orders">The orders.</param>
        /// <returns>A DownloadInfo object.</returns>
        public DownloadInfo GetDownloadInfo(IEnumerable<Order> orders)
        {
            DownloadInfo info = new DownloadInfo { StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow, PageCount = 0, TotalOrders = 0 };

            if (orders.Count() > 0)
            {
                // We don't need to make a request to Newegg to build the DownloadInfo
                // here. We can pull everything from the set of orders provided, we just
                // need to sort the orders by the order date to get the start and end dates
                orders = orders.OrderBy(o => o.OrderDateInPacificStandardTime);

                info.StartDate = orders.First().OrderDateToUtcTime();
                info.EndDate = orders.Last().OrderDateToUtcTime();
                info.TotalOrders = orders.Count();
                info.PageCount = new PageCalculator().CalculatePageCount(orders.Count(), MaxPageSize);
            }

            return info;
        }

        /// <summary>
        /// Submits the request to obtain the orders.
        /// </summary>
        /// <returns>
        /// An IEnumerable of Order objects.
        /// </returns>
        public DownloadResult Download(DateTime utcFromDate, DateTime utcToDate, int pageNumber, NeweggOrderType orderType)
        {
            // Build the request body based on the parameters provided
            string requestBody = GetRequestBody(utcFromDate, utcToDate, pageNumber, MaxPageSize, orderType);
            return GetDownloadResponse(requestBody);
        }

        /// <summary>
        /// Downloads the specified orders.
        /// </summary>
        /// <param name="orders">The orders.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>A DownloadResult object.</returns>
        public DownloadResult Download(IEnumerable<Order> orders, int pageNumber)
        {
            // Build the request body based on the parameters provided
            string requestBody = GetRequestBody(orders, pageNumber, MaxPageSize);
            return GetDownloadResponse(requestBody);
        }

        /// <summary>
        /// Gets the download response.
        /// </summary>
        /// <param name="requestBody">The request body.</param>
        /// <returns>A DownloadResult object.</returns>
        private DownloadResult GetDownloadResponse(string requestBody)
        {
            // Submit the request and inspect the response
            NeweggResponse response = SubmitRequest(requestBody);
            if (response.ResponseErrors.Count() > 0)
            {
                throw new NeweggException("Failed to get orders from Newegg. Please try again later.", response);
            }

            // We know the result is an OrdersResult since a NeweggException wasn't thrown
            return response.Result as DownloadResult;
        }

        /// <summary>
        /// Submits the request.
        /// </summary>
        /// <param name="requestBody">The request body.</param>
        /// <returns>A NeweggResponse object.</returns>
        private NeweggResponse SubmitRequest(string requestBody)
        {
            // API URL depends on which marketplace the seller selected 
            string marketplace = (credentials.Channel == NeweggChannelType.Business) ? "/b2b" : "";

            // Format our request URL with the value of the seller ID and configure the request
            string formattedUrl = string.Format(RequestUrl, marketplace, credentials.SellerId);

            RequestConfiguration requestConfig = new RequestConfiguration("Downloading Orders", formattedUrl)
            { 
                Method = HttpVerb.Put, 
                Body = requestBody
            };

            string orderData = request.SubmitRequest(credentials, requestConfig);

            // The order data should contain the XML containing the list of orders from Newegg
            NeweggResponse orderResponse = new NeweggResponse(orderData, new DownloadResponseSerializer());
            return orderResponse;
        }

        /// <summary>
        /// Converts a UTC datetime to pacific standard time.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The Pacific Standard Time for the given datetime.</returns>
        private static DateTime ConvertUtcToPacificStandardTime(DateTime date)
        {
            TimeZoneInfo pacificStandardTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(date, pacificStandardTimeZone);
        }

        /// <summary>
        /// Gets the payload for the report generation request.
        /// </summary>
        /// <returns>Data that is being posted to the Newegg API.</returns>
        private static string GetRequestBody(DateTime utcFromDate, DateTime utcToDate, int pageNumber, int pageSize, NeweggOrderType orderType)
        {   
            // Newegg expects all dates to be in PST, so convert the date range used in our request to PST
            DateTime fromDateInPacificStandardTime = ConvertUtcToPacificStandardTime(utcFromDate);
            DateTime toDateInPacificStandardTime = ConvertUtcToPacificStandardTime(utcToDate);

            string requestBody = string.Format(@"
                    <NeweggAPIRequest >
                      <OperationType>GetOrderInfoRequest</OperationType>
                      <RequestBody>
                        <PageIndex>{0}</PageIndex>
                        <PageSize>{1}</PageSize>
                        <RequestCriteria>
                          <OrderNumberList />
                          <OrderDateFrom>{2}</OrderDateFrom>
                          <OrderDateTo>{3}</OrderDateTo>
                          <Type>{4}</Type>
                        </RequestCriteria>
                      </RequestBody>
                    </NeweggAPIRequest>", pageNumber, pageSize, fromDateInPacificStandardTime.ToString("yyyy-MM-dd hh:mm:ss tt"), toDateInPacificStandardTime.ToString("yyyy-MM-dd hh:mm:ss tt"), (int)orderType);
            return requestBody;
        }

        /// <summary>
        /// Gets the payload for the report generation request based on the order numbers of the 
        /// orders specified.
        /// </summary>
        /// <returns>Data that is being posted to the Newegg API.</returns>
        private static string GetRequestBody(IEnumerable<Order> orders, int pageNumber, int pageSize)
        {
            // Build the <OrderNumber> nodes for our request
            StringBuilder orderNodesBuilder = new StringBuilder();
            foreach (Order order in orders)
            {
                orderNodesBuilder.AppendFormat("<OrderNumber>{0}</OrderNumber>", order.OrderNumber);
                orderNodesBuilder.AppendLine();
            }

            string requestBody = string.Format(@"
                    <NeweggAPIRequest >
                      <OperationType>GetOrderInfoRequest</OperationType>
                      <RequestBody>
                        <PageIndex>{0}</PageIndex>
                        <PageSize>{1}</PageSize>
                        <RequestCriteria>
                          <OrderNumberList>{2}</OrderNumberList>
                          <OrderDateFrom />
                          <OrderDateTo/>
                        </RequestCriteria>
                      </RequestBody>
                    </NeweggAPIRequest>", pageNumber, pageSize, orderNodesBuilder.ToString());
            return requestBody;
        }
    }
}
