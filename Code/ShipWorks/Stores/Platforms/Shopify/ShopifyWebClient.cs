using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Communication.Throttling;
using log4net;
using Newtonsoft.Json.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Shopify.Enums;
using System.Text;
using Interapptive.Shared.Collections;
using ShipWorks.Shipping;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Interface to connecting to Shopify
    /// </summary>
    public class ShopifyWebClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShopifyWebClient));

        static readonly LruCache<string, JToken> productCache = new LruCache<string, JToken>(1000);
        readonly static ShopifyWebClientRequestThrottle throttler = new ShopifyWebClientRequestThrottle();

        // Progress reporting
        IProgressReporter progress;

        ShopifyStoreEntity store;
        ShopifyEndpoints endpoints;
        
        /// <summary>
        /// Create an instance of the web client for connecting to the specified store
        /// </summary>
        public ShopifyWebClient(ShopifyStoreEntity store, IProgressReporter progress)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            this.store = store;

            if (string.IsNullOrWhiteSpace(store.ShopifyShopUrlName))
            {
                throw new ShopifyException("ShopifyShopUrlName is missing.");
            }

            // Create the Endpoints object for getting api urls
            endpoints = new ShopifyEndpoints(store.ShopifyShopUrlName);

            this.progress = progress;
        }

        /// <summary>
        /// Endpoints object for getting api urls
        /// </summary>
        public ShopifyEndpoints Endpoints
        {
            get
            {
                return endpoints;
            }
        }

        /// <summary>
        /// Determines if the given shop name is a valid shopify shop
        /// </summary>
        public static bool IsRealShopifyShopUrlName(string shopUrlName)
        {
            HttpVariableRequestSubmitter requestSubmitter = new HttpVariableRequestSubmitter();
            requestSubmitter.Verb = HttpVerb.Get;

            try
            {
                requestSubmitter.Uri = new ShopifyEndpoints(shopUrlName).GetApiAuthorizeUrl();
            }
            catch (UriFormatException ex)
            {
                log.Warn("The specified shop name created an invalid URI", ex);

                return false;
            }

            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.Shopify, "LoginPage");
            logEntry.LogRequest(requestSubmitter);

            try
            {
                RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(ShopifyWebClientApiCall.IsRealShopifyShopUrlName, requestSubmitter, null);

                using (IHttpResponseReader respReader = throttler.ExecuteRequest<HttpRequestSubmitter, IHttpResponseReader>(requestThrottleArgs, MakeRequest))
                {
                    string pageText = respReader.ReadResult();
                    logEntry.LogResponse(pageText, "html");

                    // Check the content of the page for the form action that goes to the auth login page.
                    // If the content contains the html, it is the login page.
                    bool isLoginPage = isLoginPage = pageText.IndexOf("login", StringComparison.OrdinalIgnoreCase) > -1;

                    return isLoginPage;
                }
            }
            catch (WebException ex)
            {
                log.Error("Could not open shopify login page", ex);

                return false;
            }
        }

        /// <summary>
        /// Make the call to Shopify to get an AccessToken for accessing the API
        /// </summary>
        /// <param name="shopUrlName">The shop url name.</param>
        /// <param name="requestTokenUrl">The url returned from the Shopify Callback</param>
        /// <returns>The access token needed to access the api</returns>
        public static string GetAccessToken(string shopUrlName, Uri requestTokenUrl)
        {
            if (requestTokenUrl == null)
            {
                throw new ArgumentNullException("requestTokenUrl", "requestTokenUrl is required");
            }

            try
            {
                // Get the request token needed for requesting the access token
                string requestToken = ExtractRequestToken(requestTokenUrl);

                // Create the variable request submitter with default params
                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                request.Uri = new Uri(new ShopifyEndpoints(shopUrlName).GetApiAccessTokenUrl(requestToken));
                request.Verb = HttpVerb.Post;

                // Get the response from the call.  
                // This is the JSON response that contains the access token
                using (var reader = ProcessRequestReader(request, ShopifyWebClientApiCall.GetAccessToken, null))
                {
                    string response = reader.ReadResult();

                    // Parse out the access token
                    JObject accessTokenRensponse = JObject.Parse(response);
                    return (string) accessTokenRensponse[ShopifyConstants.AccessTokenParamName];
                }
            }
            catch (JsonException ex)
            {
                string msg = string.Format("An error occurred in GetAccessToken({0}).{1}     ", requestTokenUrl, Environment.NewLine);
                log.ErrorFormat("{0}An error occurred during JObect.Parse. {1}", msg, ex.ToString());

                throw new ShopifyException("Shopify did not return a valid access token to ShipWorks.", ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(ShopifyException));
            }
        }

        /// <summary>
        /// Gets the JSON representation of the Shop from Shopify
        /// </summary>
        public void RetrieveShopInformation()
        {
            try
            {
                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter() { Verb = HttpVerb.Get };
                request.Uri = new Uri(Endpoints.ShopUrl);

                // Make the call and get the response
                string shopAsString = ProcessAuthenticatedRequest(request, ShopifyWebClientApiCall.GetShop, progress);

                JToken shop = JObject.Parse(shopAsString);

                if (shop != null && shop["shop"] != null)
                {
                    shop = shop["shop"];

                    store.StoreName = shop.GetValue<string>("name", "Shopify Store");
                    store.ShopifyShopDisplayName = store.StoreName;

                    store.Street1 = shop.GetValue<string>("address1", string.Empty);
                    store.City = shop.GetValue<string>("city", string.Empty);
                    store.StateProvCode = Geography.GetStateProvCode(shop.GetValue<string>("province", string.Empty));
                    store.PostalCode = shop.GetValue<string>("zip", string.Empty);
                    store.CountryCode = Geography.GetCountryCode(shop.GetValue<string>("country", string.Empty));

                    store.Email = shop.GetValue<string>("email", string.Empty);
                    store.Phone = shop.GetValue<string>("phone", string.Empty);
                }
                else
                {
                    throw new ShopifyException("Shopify returned an invalid response to ShipWorks.");
                }
            }
            catch (JsonException ex)
            {
                log.ErrorFormat("An error occurred during JObect.Parse", ex);

                throw new ShopifyException("Shopify returned an invalid response to ShipWorks while retrieving store information.", ex);
            }
        }

        /// <summary>
        /// Makes a call to the web server and get's it's current date and time
        /// </summary>
        public DateTime GetServerCurrentDateTime()
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter() { Verb = HttpVerb.Get };
            request.Uri = new Uri(Endpoints.ApiGetOrderCountUrl);

            // We don't really care about the order count, we just need a call to the server, so use Now
            request.Variables.Add("updated_at_min", DateTime.UtcNow.ToString("o"));

            using (IHttpResponseReader respReader = ProcessAuthenticatedRequestReader(request, ShopifyWebClientApiCall.GetServerCurrentDateTime, progress))
            {            
                DateTime serverDateTime;

                // Try to parse the date.  If it doesn't succeed, use DateTime.Now less 2 min
                if (!DateTime.TryParse(respReader.HttpWebResponse.Headers["Date"], out serverDateTime))
                {
                    serverDateTime = DateTime.UtcNow;
                }

                return serverDateTime.ToUniversalTime();
            }
        }

        /// <summary>
        /// Make a call to Shopify requesting a count of orders matching criteria.
        /// </summary>
        /// <param name="startDate">Filter by shopify order modified date after this date</param>
        /// <param name="endDate">Filter by shopify order modified date before this date</param>
        /// <returns>Number of orders matching criteria</returns>
        public int GetOrderCount(DateTime startDate, DateTime endDate)
        {
            string url = Endpoints.ApiGetOrderCountUrl;

            try
            {
                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter() { Verb = HttpVerb.Get };
                request.Uri = new Uri(url);

                // We only want to filter by modified date, but Shopify excludes some of these statuses by default.  For example, status is defaulted to only return open orders.
                request.Variables.Add("status", "any");
                request.Variables.Add("financial_status", "any");
                request.Variables.Add("fulfillment_status", "any");

                // For times, Shopify provides the date offset, and that is needed to correctly query their orders, so use ToString("o")
                request.Variables.Add("updated_at_min", startDate.ToString("o"));
                request.Variables.Add("updated_at_max", endDate.ToString("o"));

                // Make the call and get the response
                string count = ProcessAuthenticatedRequest(request, ShopifyWebClientApiCall.GetOrderCount, progress);
                JObject jsonCount = JObject.Parse(count);

                int orderCount;
                if (!int.TryParse(jsonCount["count"].ToString(), out orderCount))
                {
                    // Could not convert to an int, so assume none were found
                    orderCount = 0;
                }

                return orderCount;
            }
            catch (JsonException ex)
            {
                string message = string.Format("An error occurred in GetOrderCount for Url: '{0}'){1}     ", url, Environment.NewLine);
                log.ErrorFormat("{0}An error occurred during JObect.Parse. {1}", message, ex.ToString());

                throw new ShopifyException("Shopify returned an invalid response to ShipWorks while getting the order count.", ex);
            }
        }

        /// <summary>
        /// Make the call to Shopify to get a list of orders in the date range
        /// </summary>
        /// <returns>List of JToken orders, sorted by updated_at ascending</returns>
        public List<JToken> GetOrders(DateTime startDate, DateTime endDate, int page = 1)
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter() { Verb = HttpVerb.Get };
            request.Uri = new Uri(Endpoints.ApiGetOrdersUrl);

            // We only want to filter by modified date, but Shopify excludes some of these statuses by default.  For example, status is defaulted to only return open orders.
            request.Variables.Add("status", "any");
            request.Variables.Add("financial_status", "any");
            request.Variables.Add("fulfillment_status", "any");

            // Set max results and page if provided
            request.Variables.Add("limit", ShopifyConstants.OrdersPageSize.ToString());

            // Set page if specified
            if (page > 1)
            {
                request.Variables.Add("page", page.ToString());
            }

            // For times, Shopify provides the date offset, and that is needed to correctly query their orders, so use ToString("o")
            request.Variables.Add("updated_at_min", startDate.ToString("o"));
            request.Variables.Add("updated_at_max", endDate.ToString("o"));

            string ordersAsString = ProcessAuthenticatedRequest(request, ShopifyWebClientApiCall.GetOrders, progress);

            JObject orderList = JObject.Parse(ordersAsString);
            List<JToken> ordersToReturn = new List<JToken>();

            if (orderList != null)
            {
                JArray ordersToken = orderList.SelectToken("orders") as JArray;

                if (ordersToken != null && ordersToken.Count > 0)
                {
                    // Sort the orders by update date ascending
                    ordersToReturn = ordersToken.OrderBy(o => o["updated_at"]).ToList<JToken>();
                }
            }

            return ordersToReturn;
        }

        /// <summary>
        /// Get a shopify product by shopify Product Id
        /// This method will first check the local product cache and return that product object if found,
        /// otherwise, it will make a call to Shopify to get the product, then store it in the cache.
        /// </summary>
        /// <param name="shopifyProductId">Shopify Product Id</param>
        /// <returns></returns>
        public JToken GetProduct(long shopifyProductId)
        {
            string url = Endpoints.ApiProductUrl(shopifyProductId);
            JToken product;

            try
            {
                // See if we have a cached version of the product
                if (productCache.Contains(url.ToLower()))
                {
                    product = productCache[url.ToLower()];
                }
                else
                {
                    // Not cached, so go get it
                    HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter() { Verb = HttpVerb.Get };
                    request.Uri = new Uri(url);

                    try
                    {
                        string productAsString = ProcessAuthenticatedRequest(request, ShopifyWebClientApiCall.GetProduct, progress);
                        product = JObject.Parse(productAsString);
                    }
                    catch (ShopifyException ex)
                    {
                        // If the product does not exist on Shopify, just cache and return null
                        if (ex.InnerException is WebException && 
                            ((HttpWebResponse) ((WebException) ex.InnerException).Response).StatusCode == HttpStatusCode.NotFound)
                        {
                            product = null;
                        }
                        else
                        {
                            throw;
                        }
                    }

                    productCache[url.ToLower()] = product;
                }

                return product;
            }
            catch (JsonException ex)
            {
                string message = string.Format("An error occurred in GetProduct for Url: '{0}'){1}     ", url, Environment.NewLine);
                log.ErrorFormat("{0}An error occurred during Json operation. {1}", message, ex.ToString());

                throw new ShopifyException("Shopify returned an invalid response to ShipWorks while getting the order count.", ex);
            }
        }

        /// <summary>
        /// Update the online status of the given orders
        /// </summary>
        public void UploadOrderShipmentDetails(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            if (shipment.Order.IsManual)
            {
                log.InfoFormat("Not uploading shipment details for OrderID {0} since it is manual.", shipment.Order.OrderID);
                return;            
            }

            try
            {
                ShopifyOrderEntity order = (ShopifyOrderEntity) shipment.Order;

                string carrier = ShippingManager.GetCarrierName((ShipmentTypeCode) shipment.ShipmentType);
                string trackingNumber = shipment.TrackingNumber;

                // Check the order's online status to see if it's Fulfilled.  If it is, don't try to re-ship it...it will throw an error.
                if ((ShopifyFulfillmentStatus) order.FulfillmentStatusCode == ShopifyFulfillmentStatus.Fulfilled)
                {
                    log.WarnFormat("Not updating shipment status of Order {0} since it is already 'Fulfilled'", order.OrderNumberComplete);
                    return;
                }

                string url = Endpoints.ApiFulfillmentsUrl(order.ShopifyOrderID);

                if (string.IsNullOrEmpty(trackingNumber))
                {
                    trackingNumber = "null";
                }

                ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
                shipmentType.LoadShipmentData(shipment, true);

                string carrierTrackingUrl = shipmentType.GetCarrierTrackingUrl(shipment);

                JObject fulfillmentReq = new JObject(
                    new JProperty("fulfillment", new JObject(
                        new JProperty("tracking_company", carrier),
                        new JProperty("tracking_number", trackingNumber),
                        new JProperty("custom_tracking_url", carrierTrackingUrl))));

                string jsonRequest = fulfillmentReq.ToString();

                // Create the json post request submitter, with default params
                HttpTextPostRequestSubmitter request = new HttpTextPostRequestSubmitter(jsonRequest, "application/json; charset=utf-8") { Verb = HttpVerb.Post };
                request.Uri = new Uri(url);

                // The shopify api will return a status code of Created if it can successfully add the shipment.
                // Therefore, we must add Created to the allowed statuses so that the submitter doesn't through
                // when it doesn't receive the OK status code
                List<HttpStatusCode> allowedStatuses = new List<HttpStatusCode>();
                allowedStatuses.Add(HttpStatusCode.Created);
                request.AllowHttpStatusCodes(allowedStatuses.ToArray());

                // Make the call.  If unsuccessful, an error is thrown, so we don't care about the response value
                ProcessAuthenticatedRequest(request, ShopifyWebClientApiCall.AddFulfillment, progress);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(ShopifyException));
            }
        }

        /// <summary>
        /// Process a request that requires authentication headers to be sent to Shopify
        /// </summary>
        private string ProcessAuthenticatedRequest(HttpRequestSubmitter request, ShopifyWebClientApiCall action, IProgressReporter progressReporter)
        {
            using (var reader = ProcessAuthenticatedRequestReader(request, action, progressReporter))
            {
                return reader.ReadResult();
            }
        }

        /// <summary>
        /// Process a request that requires authentication headers to be sent to Shopify
        /// </summary>
        private IHttpResponseReader ProcessAuthenticatedRequestReader(HttpRequestSubmitter request, ShopifyWebClientApiCall action, IProgressReporter progressReporter)
        {
            // Add our authentication header
            request.Headers.Add("X-Shopify-Access-Token", store.ShopifyAccessToken);

            return ProcessRequestReader(request, action, progressReporter);
        }

        /// <summary>
        /// Wrapper method for calling HttpRequestSubmitter.GetResponse().  This method also takes care of creating ApiLogEntry entries.
        /// </summary>
        /// <param name="request">An HttpRequestSubmitter derived class on which to make the http call and log</param>
        /// <param name="action">The action to be passed to the logger</param>
        /// <param name="progressReporter">An IProgressReporter, if available</param>
        /// <returns>The response from the http call</returns>
        private static IHttpResponseReader ProcessRequestReader(HttpRequestSubmitter request, ShopifyWebClientApiCall action, IProgressReporter progressReporter)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            try
            {
                // Log how long the call takes
                using (new LoggedStopwatch(log, "ProcessRequest " + action))
                {
                    // Log the request
                    ApiLogEntry logger = new ApiLogEntry(ApiLogSource.Shopify, EnumHelper.GetDescription(action));
                    logger.LogRequest(request);


                    RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(action, request, progressReporter);

                    // Ask for the response
                    using (IHttpResponseReader responseReader = throttler.ExecuteRequest<HttpRequestSubmitter, IHttpResponseReader>(requestThrottleArgs, MakeRequest))
                    {
                        // Read the result
                        string response = responseReader.ReadResult();

                        // Log the response
                        logger.LogResponse(response, "txt");

                        return responseReader;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Error in ProcessRequest for action: '{0}'.", action), ex);
                throw WebHelper.TranslateWebException(ex, typeof(ShopifyException));
            }
        }

        /// <summary>
        /// Submit a Shopify request, throttled so we don't over-call
        /// </summary>
        /// <typeparam name="THttpRequestSubmitter">Needed by the throttler.  The type of the request to send to the api via throttler.</typeparam>
        /// <typeparam name="THttpResponseReader">Needed by the throttler.  The type of the response that will be received by the api via throttler.</typeparam>
        /// <param name="request">The actual request to make.</param>
        /// <returns>HttpResponseReader received from the call</returns>
        private static IHttpResponseReader MakeRequest<THttpRequestSubmitter>(THttpRequestSubmitter request)
            where THttpRequestSubmitter : HttpRequestSubmitter
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            try
            {
                IHttpResponseReader responseReader = request.GetResponse();

                return responseReader;
            }
            catch (WebException ex)
            {
                HttpWebResponse webResponse = ex.Response as HttpWebResponse;
                if (webResponse != null && webResponse.StatusCode == (HttpStatusCode)ShopifyConstants.OverApiLimitStatusCode)
                {
                    throw new RequestThrottledException(ex.Message);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Make the call to Shopify to get a RequestToken.  This will be used by GetAccessToken later.
        /// Sets the RequestToken for future use.
        /// </summary>
        /// <param name="requestTokenUrl">The url returned from the Shopify Callback</param>
        private static string ExtractRequestToken(Uri requestTokenUrl)
        {
            // Now get the value of the request token param if it exists
            if (UriHasRequestToken(requestTokenUrl))
            {
                // Get the query string from the uri
                NameValueCollection queryStringParams = HttpUtility.ParseQueryString(requestTokenUrl.Query);

                return queryStringParams[ShopifyConstants.RequestTokenParamName].ToString();
            }

            throw new InvalidOperationException(string.Format("The requestToken could not be extracted from {0}", requestTokenUrl));
        }

        /// <summary>
        /// Checks the given Uri to determine if the RequestTokenParamName exists.
        /// </summary>
        /// <param name="requestTokenUrl">The url returned from the Shopify Callback</param>
        public static bool UriHasRequestToken(Uri requestTokenUrl)
        {
            if (requestTokenUrl == null)
            {
                throw new ArgumentNullException("requestTokenUrl", "requestTokenUrl is required");
            }

            // Get the query string from the uri
            NameValueCollection queryStringParams = HttpUtility.ParseQueryString(requestTokenUrl.Query);

            // Now get the value of the request token param
            if (queryStringParams != null && queryStringParams[ShopifyConstants.RequestTokenParamName] != null && !string.IsNullOrEmpty(queryStringParams[ShopifyConstants.RequestTokenParamName]))
            {
                return true;
            }

            return false;
        }

    }
}
