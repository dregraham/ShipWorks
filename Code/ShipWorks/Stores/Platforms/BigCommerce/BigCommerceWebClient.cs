using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using ShipWorks.Common.Threading;
using ShipWorks.Stores.Communication.Throttling;
using ShipWorks.Stores.Platforms.BigCommerce.DTO;
using ShipWorks.Stores.Platforms.BigCommerce.Enums;
using log4net;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Interface for connecting to BigCommerce
    /// </summary>
    public class BigCommerceWebClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BigCommerceWebClient));
        static readonly LruCache<int, BigCommerceProductImage> productImageCache = new LruCache<int, BigCommerceProductImage>(1000);
        readonly List<BigCommerceOrderStatus> bigCommerceOrderStatuses;
        readonly string apiUserName;
        readonly string apiToken;
        readonly string apiUrl;
        readonly static BigCommerceWebClientRequestThrottle throttler = new BigCommerceWebClientRequestThrottle();
        readonly IProgressReporter progressReporter;
        readonly List<HttpStatusCode> successHttpStatusCodes;
        RestClient apiClient;

        private const string storeSettingMissingErrorMessage = "The BigCommerce {0} is missing or invalid.  Please enter your {0} by going to Manage > Stores > Your Store > Edit > Store Connection.  You will find instructions on how to find the {0} there.";

        /// <summary>
        /// Create an instance of the web client for connecting to the specified store
        /// </summary>
        /// <exception cref="BigCommerceException" />
        public BigCommerceWebClient(string apiUserName, string apiUrl, string apiToken) :
            this(apiUserName, apiUrl, apiToken, null)
        {
        }
        
        /// <summary>
        /// Create an instance of the web client for connecting to the specified store
        /// </summary>
        /// <exception cref="BigCommerceException" />
        public BigCommerceWebClient(string apiUserName, string apiUrl, string apiToken, IProgressReporter progressReporter)
        {

            if (string.IsNullOrWhiteSpace(apiUrl))
            {
                throw new BigCommerceException(string.Format(storeSettingMissingErrorMessage, "Store API Path"));
            }

            if (string.IsNullOrWhiteSpace(apiUserName))
            {
                throw new BigCommerceException(string.Format(storeSettingMissingErrorMessage, "Store API Username"));
            }

            if (string.IsNullOrWhiteSpace(apiToken))
            {
                throw new BigCommerceException(string.Format(storeSettingMissingErrorMessage, "Store API Token"));
            }

            this.apiUserName = apiUserName;
            this.apiToken = apiToken;
            this.apiUrl = apiUrl;
            this.progressReporter = progressReporter;

            bigCommerceOrderStatuses = new List<BigCommerceOrderStatus>();

            successHttpStatusCodes = new List<HttpStatusCode> { HttpStatusCode.OK, 
                                                                HttpStatusCode.Created, 
                                                                HttpStatusCode.Accepted, 
                                                                HttpStatusCode.NoContent};
        }

        /// <summary>
        /// Gets the RestClient used to contact BigCommerce
        /// </summary>
        /// <exception cref="BigCommerceException" />
        private RestClient CreateApiClient()
        {
            if (apiClient == null)
            {
                apiClient = new RestClient(apiUrl);

                if (apiClient == null)
                {
                    throw new BigCommerceException("Unable to create API client for BigCommerce.");
                }

                apiClient.Authenticator = new HttpBasicAuthenticator(apiUserName, apiToken);
            }

            return apiClient;
        }

        /// <summary>
        /// Update the online status and details of the given shipment
        /// </summary>
        /// <param name="orderNumber">The order number of this shipment</param>
        /// <param name="orderAddressID">The BigCommerce order addressID for this shipment</param>
        /// <param name="trackingNumber">Tracking number for this shipment</param>
        /// <param name="orderItems">The list of BigCommerceItem's in this shipment</param>
        /// <exception cref="BigCommerceException" />
        public void UploadOrderShipmentDetails(long orderNumber, long orderAddressID, string trackingNumber, List<BigCommerceItem> orderItems)
        {
            string uploadShipmentResource = BigCommerceWebClientEndpoints.GetUploadShipmentResource(orderNumber);

            RestRequest restRequest = new RestRequest(uploadShipmentResource, Method.POST);
            restRequest.RequestFormat = DataFormat.Json;

            BigCommerceAddShipment addShipment = new BigCommerceAddShipment
                {
                    order_address_id = (int)orderAddressID,
                    tracking_number = trackingNumber,
                    comments = string.Empty,
                    items = orderItems
                };

            restRequest.AddBody(addShipment);

            IRestResponse restResponse;
            try
            {
                RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(BigCommerceWebClientApiCall.CreateShipment, restRequest, progressReporter);

                restResponse = throttler.ExecuteRequest<RestRequest, RestResponse>(requestThrottleArgs, MakeRequest<RestRequest, RestResponse>);
            }
            catch (BigCommerceException bigCommerceException)
            {
                string errorMessage =
                    string.Format("ShipWorks was unable to create an online shipment for order number {0}.",
                                    orderNumber);
                throw new BigCommerceException(errorMessage, bigCommerceException);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(BigCommerceException));
            }

            // Check for createFulfillment specific error
            if ((restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != 0) || restResponse.ErrorException != null)
            {
                string errorMessage = string.Format("Unable to create shipment for order number {0}.", orderNumber);
                log.Error(errorMessage, restResponse.ErrorException);
                throw new BigCommerceException(errorMessage, restResponse.ErrorException);
            }
        }

        /// <summary>
        /// Updates the online status of orders
        /// </summary>
        /// <exception cref="BigCommerceException" />
        public void UpdateOrderStatus(int orderNumber, int statusCode)
        {
            string updateOrderStatusResource = BigCommerceWebClientEndpoints.GetOrderResource(orderNumber);

            RestRequest restRequest = new RestRequest(updateOrderStatusResource, Method.PUT)
                {
                    RequestFormat = DataFormat.Json
                };

            BigCommerceUpdateOrderStatusRequest updateOrderStatusRequest = new BigCommerceUpdateOrderStatusRequest
                {
                    status_id = statusCode
                };

            restRequest.AddBody(updateOrderStatusRequest);

            IRestResponse restResponse;
            try
            {
                RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(BigCommerceWebClientApiCall.UpdateOrderStatus, restRequest, progressReporter);

                restResponse =
                    throttler.ExecuteRequest<RestRequest, RestResponse>(requestThrottleArgs, MakeRequest<RestRequest, RestResponse>);
            }
            catch (BigCommerceException bigCommerceException)
            {
                string errorMessage = string.Format("ShipWorks was unable to update the online order status for order number {0}.", orderNumber);
                throw new BigCommerceException(errorMessage, bigCommerceException);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(BigCommerceException));
            }

            // Check the response update order status specific error
            if (restResponse.StatusCode != HttpStatusCode.OK || restResponse.ErrorException != null)
            {
                string errorMessage = string.Format("Unable to update status for order number {0}.", orderNumber);
                log.Error(errorMessage, restResponse.ErrorException);
                throw new BigCommerceException(errorMessage, restResponse.ErrorException);
            }
        }

        /// <summary>
        /// Make a call to BigCommerce requesting a count of orders matching criteria.
        /// </summary>
        /// <param name="orderSearchCriteria">Get order count based on BigCommerceWebClientOrderSearchCriteria.</param>
        /// <returns>Number of orders matching criteria</returns>
        /// <exception cref="BigCommerceException" />
        public int GetOrderCount(BigCommerceWebClientOrderSearchCriteria orderSearchCriteria)
        {
            // Get the number of created orders
            int createdCount = GetOrderCount(orderSearchCriteria, BigCommerceWebClientOrderDateSearchType.CreatedDate);

            // Get the number of modified orders
            int modifiedCount = GetOrderCount(orderSearchCriteria, BigCommerceWebClientOrderDateSearchType.ModifiedDate);

            // Return the sum of modified and created orders
            return modifiedCount + createdCount;
        }
        
        /// <summary>
        /// Make a call to BigCommerce requesting a count of orders matching criteria.
        /// </summary>
        /// <param name="orderSearchCriteria">Get order count based on BigCommerceWebClientOrderSearchCriteria.</param>
        /// <param name="orderDateSearchType">Filter by BigCommerceWebClientOrderDateSearchType.  To get all orders, we have to query by created and modified date in separate calls.</param>
        /// <returns>Number of orders matching criteria</returns>
        /// <exception cref="BigCommerceException" />
        private int GetOrderCount(BigCommerceWebClientOrderSearchCriteria orderSearchCriteria, BigCommerceWebClientOrderDateSearchType orderDateSearchType)
        {
            RestRequest getOrderCountRequest = new RestRequest(BigCommerceWebClientEndpoints.GetOrderCountResource());

            // Unfortunately we can't query by modified and created date in one call...they seem to be ANDed together and will miss orders
            // So the public GetOrderCount makes 2 calls to this override, one for modified and one for created date query
            if (orderDateSearchType == BigCommerceWebClientOrderDateSearchType.CreatedDate)
            {
                getOrderCountRequest.AddParameter("min_date_created", orderSearchCriteria.LastCreatedFromDate.ToUniversalTime().ToString("r"), ParameterType.GetOrPost);
                getOrderCountRequest.AddParameter("max_date_created", orderSearchCriteria.LastCreatedToDate.ToUniversalTime().ToString("r"), ParameterType.GetOrPost);
            }
            else
            {
                getOrderCountRequest.AddHeader("If-Modified-Since", orderSearchCriteria.LastModifiedFromDate.ToUniversalTime().ToString("r"));
            }

            BigCommerceGetOrderCountResponse restResponse;
            try
            {
                RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(BigCommerceWebClientApiCall.GetOrderCount, getOrderCountRequest, progressReporter);

                restResponse =
                    throttler.ExecuteRequest<RestRequest, BigCommerceGetOrderCountResponse>(requestThrottleArgs, MakeRequest<RestRequest, BigCommerceGetOrderCountResponse>);
            }
            catch (BigCommerceException bigCommerceException)
            {
                throw new BigCommerceException("ShipWorks was unable to download orders for your BigCommerce store.", bigCommerceException);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(BigCommerceException));
            }

            // If we deserialize a response to null, make sure we don't crash since the error isn't fatal
            if (restResponse == null)
            {
                throw new BigCommerceException("ShipWorks received an invalid response from BigCommerce. Please try again later.");
            }

            return restResponse.count;
        }

        /// <summary>
        /// Generic method to make a request to the BigCommerce rest api
        /// </summary>
        /// <typeparam name="TRestRequest">Type of the request being made</typeparam>
        /// <typeparam name="TRestResponse">Type of the response from the API call</typeparam>
        /// <returns>The response from the API call</returns>
        /// <exception cref="BigCommerceException" />
        /// <exception cref="BigCommerceWebClientRequestThrottledException" />
        private TRestResponse MakeRequest<TRestRequest, TRestResponse>(TRestRequest request)
            where TRestRequest : IRestRequest
            where TRestResponse : new()
        {
            TRestResponse requestResult;
            try
            {
                // Make a call to the api
                IRestResponse restResponse = CreateApiClient().Execute(request);

                // Serialize the response and log it
                log.Error(restResponse.Content);

                // See if there were any errors
                CheckRestResponseForError(restResponse);

                // Deserialize the result
                requestResult = JsonConvert.DeserializeObject<TRestResponse>(restResponse.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                        NullValueHandling = NullValueHandling.Ignore
                    });

                if (request.Method == Method.PUT || request.Method == Method.POST)
                {
                    requestResult = (TRestResponse) restResponse;
                }
            }
            catch (NotSupportedException ex)
            {
                log.Error("A NotSupportedException occurred during MakeRequest.", ex);
                throw new BigCommerceException(ex.Message, ex);
            }
            catch (JsonReaderException ex)
            {
                log.Error("A JsonReaderException occurred during MakeRequest.", ex);
                throw new BigCommerceException(ex.Message, ex);
            }

            return requestResult;
        }

        /// <summary>
        /// Checks an IRestResponse StatusCode and ResponseStatus for errors
        /// </summary>
        /// <param name="restResponse"></param>
        /// <exception cref="BigCommerceWebClientRequestThrottledException" />
        /// <exception cref="BigCommerceException" />
        private void CheckRestResponseForError(IRestResponse restResponse)
        {
            if ((int)restResponse.StatusCode == BigCommerceConstants.MaxRequestsPerHourLimitReachedStatusCode)
            {
                // Check to see if we are over the API requests per hour limit
                log.Error("BigCommerce max API requests per hour reached.");
                throw new BigCommerceWebClientRequestThrottledException();
            }

            if (restResponse.StatusCode == HttpStatusCode.InternalServerError)
            {
                // BC wants to treat Internal Server Error as a "please try again soon", so we will.
                log.Error("BigCommerce returned a 500 error, so we will wait and try again soon.");
                throw new BigCommerceWebClientRequestThrottledException();
            }
            
            if (restResponse.StatusCode == HttpStatusCode.NoContent || restResponse.StatusCode == HttpStatusCode.NotModified)
            {
                // If NoContent or NotModified is returned, the search found nothing, so just return a blank Content
                restResponse.Content = string.Empty;
            }
            else if (restResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Check to see if we we denied access due to authentication.
                log.Error("BigCommerce API credentials are invalid.");
                throw new BigCommerceException("The BigCommerce API credentials are invalid", (int)restResponse.StatusCode);
            }
            else if (!successHttpStatusCodes.Contains(restResponse.StatusCode) || 
                     !(restResponse.ResponseStatus == ResponseStatus.Completed || restResponse.ResponseStatus == ResponseStatus.None))
            {
                // There was an error, find the error message and throw
                log.Error(string.Format("An error occured while communicating with BigCommerce.  Response content: {0}{1}", Environment.NewLine, restResponse.Content), restResponse.ErrorException);

                string errorMessage = GetExceptionMessage(restResponse.Content);

                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = string.Format("ShipWorks was unable to communicate with BigCommerce due to the following error: {0}{0}{1}", Environment.NewLine, errorMessage);
                }
                else
                {
                    errorMessage = string.Format("ShipWorks was unable to communicate with BigCommerce due to the following error: {0}{0}{1}", Environment.NewLine, restResponse.ErrorMessage);    
                }

                throw new BigCommerceException(errorMessage, (int)restResponse.StatusCode);
            }
            else if (restResponse.Content == "null")
            {
                // BigCommerce sometimes returns "null" in the response for some reason which was causing
                // a NullReferenceException to bubble up and crash ShipWorks
                throw new BigCommerceException("ShipWorks received an invalid response from BigCommerce. Please try again later.");
            }
        }

        /// <summary>
        /// Parses a json response for any known exceptions
        /// </summary>
        /// <param name="jsonContent">The json content to check for exceptions</param>
        /// <returns>An error message found in the jsonContent, if any</returns>
        private static string GetExceptionMessage(string jsonContent)
        {
            string errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                return errorMessage;
            }

            try
            {
                // Parse the content into an JArray (most responses seem to come as an array from BigCommerce...)
                JArray exceptionContentArray = JArray.Parse(jsonContent);
                if (exceptionContentArray != null && exceptionContentArray.Count > 0)
                {
                    // See if there is a message, and set errorMessage if so
                    errorMessage = exceptionContentArray[0]["message"] == null ? string.Empty : exceptionContentArray[0]["message"].ToString();

                    if (errorMessage.ToUpperInvariant() == "The field 'quantity' is invalid.".ToUpperInvariant())
                    {
                        errorMessage =
                            "This order has already been shipped.  If you need to modify the shipping and tracking information, please do so using the BigCommerce admin website.";
                    }
                }
            }
            catch (JsonReaderException)
            {
                try
                {
                    // There was an error parsing as an array, try as an JObject
                    JObject exceptionContent = JObject.Parse(jsonContent);
                    if (exceptionContent != null)
                    {
                        // See if there is a message, and set errorMessage if so
                        errorMessage = exceptionContent["message"] == null ? string.Empty : exceptionContent["message"].ToString();
                    }
                }
                catch (JsonReaderException ex)
                {
                    // There was an error parsing as an JObject, not sure whats happening, so log and return this exception message
                    log.Error("Unable to parse response from BigCommerce.", ex);
                    errorMessage = ex.Message;
                }
            }

            return errorMessage;
        }

        /// <summary>
        /// Make the call to BigCommerce to get a list of orders matching criteria
        /// </summary>
        /// <param name="orderSearchCriteria">Filter by BigCommerceWebClientOrderSearchCriteria.</param>
        /// <returns>List of orders matching criteria, sorted by LastUpdate ascending </returns>
        public List<BigCommerceOrder> GetOrders(BigCommerceWebClientOrderSearchCriteria orderSearchCriteria)
        {
            List<BigCommerceOrder> ordersToReturn = new List<BigCommerceOrder>();

            // Create a request for getting orders
            RestRequest request = new RestRequest(BigCommerceWebClientEndpoints.GetOrdersResource());
            
            request.AddParameter("limit", orderSearchCriteria.PageSize);
            request.AddParameter("page", orderSearchCriteria.Page);

            // Unfortunately we can't query by modified and created date in one call...they seem to be ANDed together and will miss orders
            // So the public GetOrders makes 2 calls to this override, one for modified and one for created date query
            if (orderSearchCriteria.OrderDateSearchType == BigCommerceWebClientOrderDateSearchType.CreatedDate)
            {
                request.AddParameter("min_date_created", orderSearchCriteria.LastCreatedFromDate.ToString("r"));
                request.AddParameter("max_date_created", orderSearchCriteria.LastCreatedToDate.ToString("r"));
            }
            else
            {
                request.AddHeader("If-Modified-Since", orderSearchCriteria.LastModifiedFromDate.ToUniversalTime().ToString("r"));
            }

            List<BigCommerceOrder> ordersRestResponse;
            try
            {
                RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(BigCommerceWebClientApiCall.GetOrders, request, progressReporter);

                ordersRestResponse = throttler.ExecuteRequest<RestRequest, List<BigCommerceOrder>>(requestThrottleArgs, MakeRequest<RestRequest, List<BigCommerceOrder>>);
            }
            catch (BigCommerceException bigCommerceException)
            {
                throw new BigCommerceException("ShipWorks was unable to download orders for your BigCommerce store.", bigCommerceException);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(BigCommerceException));
            }

            if (ordersRestResponse != null)
            {
                // Filter out any ids we've already processed, or just want to ignore
                IEnumerable<BigCommerceOrder> nonIncompleteOrders = ordersRestResponse.Where(o => o.status_id != (long) BigCommerceOnlineOrderStatus.Incomplete);

                // Since BigCommerce creates an order when something is added to the shopping cart (with status of InComplete), while downloading we can get into
                // the situation where all orders in a page are only InComplete.  When we filter out the InComplete orders, there aren't any left, so we think
                // there are no orders left to download.  
                // So here we check to see if the page is the full page size and if there are no non-InComplete orders.  If so, throw the
                // BigCommerceMaxIncompleteOrdersReachedException so that we skip to the next page.
                if (ordersRestResponse.Count == BigCommerceConstants.OrdersPageSize && !nonIncompleteOrders.Any())
                {
                    throw new BigCommerceMaxIncompleteOrdersReachedException("MaxIncompleteOrdersReached");
                }

                foreach (BigCommerceOrder order in nonIncompleteOrders)
                {
                    // Check for user cancel
                    if (progressReporter != null && progressReporter.IsCancelRequested)
                    {
                        break;
                    }

                    PopulateOrder(order);
                    ordersToReturn.Add(order);
                }
            }

            return ordersToReturn.OrderBy(o => o.id).ToList();
        }

        /// <summary>
        /// Loads additional order info (product images, shipments, shipping addresses, coupons, etc...)
        /// </summary>
        /// <param name="order">The order to populate</param>
        private void PopulateOrder(BigCommerceOrder order)
        {
            LoadProductImages(order);

            // Get the order shipping addresses
            PopulateOrderShippingAddresses(order);

            // Get the order coupons
            RestRequest request = new RestRequest();
            request.Resource = order.coupons.resource;
            RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(BigCommerceWebClientApiCall.GetCoupons, request, progressReporter);
            List<BigCommerceCoupon> couponsRestResponse = throttler.ExecuteRequest<RestRequest, List<BigCommerceCoupon>>(requestThrottleArgs, MakeRequest<RestRequest, List<BigCommerceCoupon>>);
            order.OrderCoupons = couponsRestResponse;
        }

        /// <summary>
        /// Populate the order's OrderShippingAddresses
        /// </summary>
        private void PopulateOrderShippingAddresses(BigCommerceOrder order)
        {
            // Get the order shipping addresses
            RestRequest request = new RestRequest();
            request.Resource = order.shipping_addresses.resource;
            request.AddParameter("limit", BigCommerceConstants.MaxPageSize);

            RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(BigCommerceWebClientApiCall.GetShippingAddress, request, progressReporter);
            List<BigCommerceAddress> shipToAddressesRestResponse = throttler.ExecuteRequest<RestRequest, List<BigCommerceAddress>>(requestThrottleArgs, MakeRequest<RestRequest, List<BigCommerceAddress>>);
            order.OrderShippingAddresses = shipToAddressesRestResponse;

            // If there were no shipping addresses, just make sure we have a non-null one to work with in our join below
            if (order.OrderShippingAddresses == null)
            {
                order.OrderShippingAddresses = new List<BigCommerceAddress>();
            }

            // If there were no items, we will take care of that in other code so just return.
            if (order.OrderProducts == null)
            {
                return;
            }

            // BigCommerce has a bug where shipping address id's are incorrect.  So check to see if there are any matches
            // between the order product order address id (which they said should be correct) and the shipping addresses order address ids
            var shipmentAddressValidation = from shipmentAddress in order.OrderShippingAddresses
                                            let orderProductAddressIDs =
                                                from orderProductAddress in order.OrderProducts
                                                select orderProductAddress.order_address_id
                                            where orderProductAddressIDs.Contains(shipmentAddress.id)
                                            select shipmentAddress;

            if (shipmentAddressValidation != null && !shipmentAddressValidation.Any())
            {
                // There were not matching shipping addresses to order product addresses, so get each address per distinct order product
                // address id.  We need to do this individually as the order could have multiple ship to addresses and we wouldn't know which 
                // shipping address goes to which orde product address id.

                // Clear out the current shipping addresses
                order.OrderShippingAddresses.Clear();

                // Get the list of distinct product order address id's
                foreach (int realShipmentOrderAddressID in order.OrderProducts.Where(op => op.order_address_id > 0).Select(op => op.order_address_id).Distinct())
                {
                    // Create a new request for this order address id
                    request = new RestRequest();
                    request.Resource = string.Format("{0}/{1}.json", order.shipping_addresses.resource, realShipmentOrderAddressID.ToString());
                    requestThrottleArgs = new RequestThrottleParameters(BigCommerceWebClientApiCall.GetShippingAddress, request, progressReporter);

                    BigCommerceAddress fixedShipmentAddress =
                        throttler.ExecuteRequest<RestRequest, BigCommerceAddress>(requestThrottleArgs, MakeRequest<RestRequest, BigCommerceAddress>);

                    // Set the shipment address id to this product address id
                    fixedShipmentAddress.id = realShipmentOrderAddressID;

                    // Add the fixed address to the order shipping addresses.
                    order.OrderShippingAddresses.Add(fixedShipmentAddress);
                }
            }
        }

        /// <summary>
        /// Get a list of Order Products for the order
        /// </summary>
        public List<BigCommerceProduct> GetOrderProducts(long orderNumber)
        {
            // Get the order shipments
            RestRequest request = new RestRequest();
            request.Resource = BigCommerceWebClientEndpoints.GetOrderProducts(orderNumber);
            RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(BigCommerceWebClientApiCall.GetOrderProducts, request, progressReporter);
            List<BigCommerceProduct> orderProductsRestResponse = throttler.ExecuteRequest<RestRequest, List<BigCommerceProduct>>(requestThrottleArgs, MakeRequest<RestRequest, List<BigCommerceProduct>>);

            return orderProductsRestResponse;
        }

        /// <summary>
        /// Get the list of BigCommerce online order statuses
        /// </summary>
        public List<BigCommerceOrderStatus> FetchOrderStatuses()
        {
            // If we don't have any order statuses, retrieve them
            if (bigCommerceOrderStatuses.Count == 0)
            {
                // Create a request for getting order statuses
                RestRequest request = new RestRequest(BigCommerceWebClientEndpoints.GetOrderStatusesPath());
                List<BigCommerceApiOrderStatus> orderStatusesRestResponse;

                try
                {
                    RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(BigCommerceWebClientApiCall.GetOrderStatuses, request, progressReporter);

                    orderStatusesRestResponse = throttler.ExecuteRequest<RestRequest, List<BigCommerceApiOrderStatus>>(requestThrottleArgs, MakeRequest<RestRequest, List<BigCommerceApiOrderStatus>>);
                }
                catch (BigCommerceException bigCommerceException)
                {
                    throw new BigCommerceException("ShipWorks was unable to download order statuses for your BigCommerce store.", bigCommerceException);
                }
                catch (Exception ex)
                {
                    throw WebHelper.TranslateWebException(ex, typeof(BigCommerceException));
                }

                List<BigCommerceApiOrderStatus> orderStatuses = orderStatusesRestResponse;
                foreach (BigCommerceApiOrderStatus orderStatus in orderStatuses)
                {
                    if (!string.IsNullOrWhiteSpace(orderStatus.name))
                    {
                        BigCommerceOrderStatus bigCommerceOrderStatus = new BigCommerceOrderStatus(orderStatus.id, orderStatus.name);
                        bigCommerceOrderStatuses.Add(bigCommerceOrderStatus);
                    }
                }
            }

            return bigCommerceOrderStatuses;
        }

        /// <summary>
        /// Get a list of products for the specified order id
        /// </summary>
        /// <param name="bigCommerceOrder">The BigCommerce order</param>
        /// <returns>List of BigCommerceProduct for the order</returns>
        private List<BigCommerceProduct> GetOrderProducts(BigCommerceOrder bigCommerceOrder)
        {
            string orderProductsResource = bigCommerceOrder.products.resource;
            List<BigCommerceProduct> products = new List<BigCommerceProduct>();

            // Set the start page
            int page = 1;

            while (true)
            {
                // Get the products for this order, one page at a time
                try
                {
                    RestRequest request = new RestRequest
                    {
                        Resource = orderProductsResource
                    };

                    // Add the paging params
                    request.AddParameter("limit", BigCommerceConstants.MaxPageSize);
                    request.AddParameter("page", page);

                    RequestThrottleParameters requestThrottleArgs =
                        new RequestThrottleParameters(BigCommerceWebClientApiCall.GetProducts, request, progressReporter);

                    List<BigCommerceProduct> pageOfProductsRestResponse =
                        throttler.ExecuteRequest<RestRequest, List<BigCommerceProduct>>(requestThrottleArgs, MakeRequest<RestRequest, List<BigCommerceProduct>>);

                    // If the page is empty, pageOfProductsRestResponse is null, so break out of the loop
                    if (pageOfProductsRestResponse == null)
                    {
                        break;
                    }

                    // Add this page of order products to the main list
                    products.AddRange(pageOfProductsRestResponse);

                    // If the number returned is less than the page size, we know we are on the last page, break out of the loop
                    if (pageOfProductsRestResponse.Count < BigCommerceConstants.MaxPageSize)
                    {
                        break;
                    }

                    // Increment for the next page
                    page++;
                }
                catch (BigCommerceException bigCommerceException)
                {
                    throw new BigCommerceException(
                        string.Format("ShipWorks was unable to download order products for order number {0}.",
                                      bigCommerceOrder.id), bigCommerceException);
                }
                catch (Exception ex)
                {
                    throw WebHelper.TranslateWebException(ex, typeof (BigCommerceException));
                }
            }

            return products;
        }

        /// <summary>
        /// Loads the product images for products in an order
        /// </summary>
        /// <param name="order">The order for which to get product images</param>
        private void LoadProductImages(BigCommerceOrder order)
        {
            // Get the order products from BigCommerce for this order
            order.OrderProducts = GetOrderProducts(order);

            foreach (BigCommerceProduct product in order.OrderProducts)
            {
                // See if we've already downloaded this product's image
                BigCommerceProductImage productImage = productImageCache[product.product_id];
                if (productImage == null)
                {
                    // Get the orduct images from BigCommerce
                    List<BigCommerceImage> productImagesRestResponse = GetProductImages(order.id, product.product_id);

                    // If we received a productImagesRestResponse, process it
                    // If we did not, BigCommerce didn't return any relevant info about the product image, so we don't want to
                    // keep trying to download it, so below we'll just put blank image info into the cached image.
                    if (productImagesRestResponse != null)
                    {
                        BigCommerceImage image = productImagesRestResponse.FirstOrDefault(img => img != null && !img.is_thumbnail);
                        if (image != null)
                        {
                            product.Image = image.standard_url;
                        }

                        image = productImagesRestResponse.FirstOrDefault(img => img.is_thumbnail);
                        if (image != null)
                        {
                            product.ThumbnailImage = image.thumbnail_url;
                        }
                    }

                    // Create the dto product image
                    productImage = new BigCommerceProductImage
                        {
                            ProductID = product.product_id,
                            Image = product.Image,
                            Thumbnail = product.ThumbnailImage
                        };

                    // And add it to the cache
                    productImageCache[product.product_id] = productImage;
                }
                else
                {
                    // The product image was in the cache, set the order product's image fields
                    product.Image = productImage.Image;
                    product.ThumbnailImage = productImage.Thumbnail;
                }

                // If the product image isn't set, but we have a thumbnail, set the main image to the thumbnail
                if (product.Image == null && product.ThumbnailImage != null)
                {
                    product.Image = product.ThumbnailImage;
                }
            }
        }

        /// <summary>
        /// Makes calls to BigCommerce to get the order product images
        /// </summary>
        /// <param name="orderId">The BigCommerce order id for which to get images</param>
        /// <param name="productId">The BigCommerce product id for which to get images</param>
        /// <returns></returns>
        private List<BigCommerceImage> GetProductImages(int orderId, int productId)
        {
            // Create a request for getting order product images
            RestRequest request = new RestRequest(BigCommerceWebClientEndpoints.GetProductImagesResource(productId));

            List<BigCommerceImage> productImagesRestResponse = new List<BigCommerceImage>();
            try
            {
                RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(BigCommerceWebClientApiCall.GetProduct, request, progressReporter);

                productImagesRestResponse = throttler.ExecuteRequest<RestRequest, List<BigCommerceImage>>(requestThrottleArgs, MakeRequest<RestRequest, List<BigCommerceImage>>);
            }
            catch (BigCommerceException bigCommerceException)
            {
                // Sometimes we get a resource not found when asking for products.  Since this is just images, we can continue on
                if (bigCommerceException.HttpStatusCode != (int)HttpStatusCode.NotFound)
                {
                    throw new BigCommerceException(string.Format("ShipWorks was unable to download products for order ID {0}.", orderId), bigCommerceException);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(BigCommerceException));
            }

            return productImagesRestResponse;
        }

        /// <summary>
        /// Attempt to get an order count to test connecting to BigCommerce.  If any error, assume connection failed.
        /// </summary>
        /// <exception cref="BigCommerceException" />
        public void TestConnection()
        {
            // See if we can successfully call GetOrderCount, if we throw, we can't connect or login
            try
            {
                BigCommerceWebClientOrderSearchCriteria orderSearchCriteria = new BigCommerceWebClientOrderSearchCriteria(BigCommerceWebClientOrderDateSearchType.CreatedDate, 
                    DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow, 
                    DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow)
                    { 
                        PageSize = BigCommerceConstants.OrdersPageSize, 
                        Page = 1
                    };
                
                GetOrderCount(orderSearchCriteria);
            }
            catch (BigCommerceException ex)
            {
                // The inner exception will have the connection error, which is the one we want.
                string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new BigCommerceException(errorMessage);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(BigCommerceException));
            }
        }
    }
}
