using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication.Throttling;
using ShipWorks.Stores.Platforms.Shopify.DTOs;
using ShipWorks.Stores.Platforms.Shopify.Enums;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Interface to connecting to Shopify
    /// </summary>
    [Component]
    public class ShopifyWebClient : IShopifyWebClient
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ShopifyWebClient));

        private static readonly LruCache<string, JToken> productCache = new LruCache<string, JToken>(1000);
        private static readonly ShopifyWebClientRequestThrottle throttler = new ShopifyWebClientRequestThrottle();

        // Progress reporting
        private readonly IProgressReporter progress;

        private readonly ShopifyStoreEntity store;
        private ShopifyEndpoints endpoints;

        /// <summary>
        /// Create an instance of the web client for connecting to the specified store
        /// </summary>
        public ShopifyWebClient(ShopifyStoreEntity store, IProgressReporter progress)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            this.store = store;
            this.progress = progress;
        }

        /// <summary>
        /// Endpoints object for getting api urls
        /// </summary>
        public ShopifyEndpoints Endpoints
        {
            get
            {
                if (endpoints == null)
                {
                    if (string.IsNullOrWhiteSpace(store.ShopifyShopUrlName))
                    {
                        throw new ShopifyException("ShopifyShopUrlName is missing.");
                    }

                    // Create the Endpoints object for getting api urls
                    endpoints = new ShopifyEndpoints(store.ShopifyShopUrlName);
                }

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
                    bool isLoginPage = pageText.IndexOf("login", StringComparison.OrdinalIgnoreCase) > -1;

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
                string msg = $"An error occurred in GetAccessToken({requestTokenUrl}).{Environment.NewLine}     ";
                log.ErrorFormat("{0}An error occurred during JObect.Parse. {1}", msg, ex);

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
                ShopifyShop shopifyShop = GetShop();

                if (shopifyShop != null)
                {
                    store.StoreName = shopifyShop.StoreName;
                    store.ShopifyShopDisplayName = store.StoreName;
                    store.Street1 = shopifyShop.Street1;
                    store.City = shopifyShop.City;
                    store.StateProvCode = Geography.GetStateProvCode(shopifyShop.StateProvince);
                    store.PostalCode = shopifyShop.PostalCode;
                    store.CountryCode = shopifyShop.Country;
                    store.Email = shopifyShop.Email;
                    store.Phone = shopifyShop.Phone;
                }
                else
                {
                    throw new ShopifyException("Shopify returned an invalid response to ShipWorks.");
                }
            }
            catch (JsonException ex)
            {
                log.Error("An error occurred during JObect.Parse", ex);

                throw new ShopifyException("Shopify returned an invalid response to ShipWorks while retrieving store information.", ex);
            }
        }

        /// <summary>
        /// Makes a call to the web server and get's it's current date and time
        /// </summary>
        public DateTime GetServerCurrentDateTime()
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter { Verb = HttpVerb.Get };
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
                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter { Verb = HttpVerb.Get };
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
                string message = $"An error occurred in GetOrderCount for Url: '{url}'){Environment.NewLine}     ";
                log.ErrorFormat("{0}An error occurred during JObect.Parse. {1}", message, ex);

                throw new ShopifyException("Shopify returned an invalid response to ShipWorks while getting the order count.", ex);
            }
        }

        /// <summary>
        /// Make the call to Shopify to get a list of orders in the date range
        /// </summary>
        /// <returns>List of JToken orders, sorted by updated_at ascending</returns>
        public List<JToken> GetOrders(DateTime startDate, DateTime endDate, int page = 1)
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter { Verb = HttpVerb.Get };
            request.Uri = new Uri(Endpoints.ApiGetOrdersUrl);

            // We only want to filter by modified date, but Shopify excludes some of these statuses by default.  For example, status is defaulted to only return open orders.
            request.Variables.Add("status", "any");
            request.Variables.Add("financial_status", "any");
            request.Variables.Add("fulfillment_status", "any");

            // Set max results and page if provided
            request.Variables.Add("limit", ShopifyConstants.ShopifyOrdersPerPage.ToString());

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
                    ordersToReturn = ordersToken.OrderBy(o => o["updated_at"]).ToList();
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

            try
            {
                // See if we have a cached version of the product
                JToken product;
                if (productCache.Contains(url.ToLower()))
                {
                    product = productCache[url.ToLower()];
                }
                else
                {
                    // Not cached, so go get it
                    HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter { Verb = HttpVerb.Get };
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
                string message = $"An error occurred in GetProduct for Url: '{url}'){Environment.NewLine}     ";
                log.ErrorFormat("{0}An error occurred during Json operation. {1}", message, ex);

                throw new ShopifyException("Shopify returned an invalid response to ShipWorks while getting a product.", ex);
            }
        }

        /// <summary>
        /// Get fraud risks for an order
        /// </summary>
        /// <param name="shopifyOrderId">Shopify Order Id</param>
        /// <returns>Collection of fraud risks</returns>
        public IEnumerable<JToken> GetFraudRisks(long shopifyOrderId)
        {
            string url = Endpoints.ApiFraudUrl(shopifyOrderId);

            try
            {
                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter { Verb = HttpVerb.Get, Uri = new Uri(url) };
                string fraudRisksAsString = ProcessAuthenticatedRequest(request, ShopifyWebClientApiCall.GetFraud, progress);

                return ParseFraudRisks(fraudRisksAsString) ?? Enumerable.Empty<JToken>();
            }
            catch (JsonException ex)
            {
                string message = $"An error occurred in GetFraudRisks for Url: '{url}'){Environment.NewLine}     ";
                log.ErrorFormat("{0}An error occurred during Json operation. {1}", message, ex);

                throw new ShopifyException("Shopify returned an invalid response to ShipWorks while getting fraud risks.", ex);
            }
        }

        /// <summary>
        /// Upload the shipment details for an order
        /// </summary>
        public void UploadOrderShipmentDetails(ShopifyUploadDetails details)
        {
            try
            {
                string url = Endpoints.ApiFulfillmentsUrl(details.ShopifyOrderID);
                var trackingNumber = string.IsNullOrEmpty(details.TrackingNumber) ? "null" : details.TrackingNumber;

                JObject fulfillmentReq = new JObject(
                    new JProperty("fulfillment",
                    new JObject(
                        new JProperty("tracking_company", details.Carrier),
                        new JProperty("tracking_number", trackingNumber),
                        new JProperty("custom_tracking_url", details.CarrierTrackingUrl),
                        new JProperty("notify_customer", store.ShopifyNotifyCustomer))));

                string jsonRequest = fulfillmentReq.ToString();

                // Create the JSON post request submitter, with default params
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
            AddAuthHeaderToRequest(request);

            return ProcessRequestReader(request, action, progressReporter);
        }

        /// <summary>
        /// Add the auth header info to the request
        /// </summary>
        private void AddAuthHeaderToRequest(HttpRequestSubmitter request)
        {
            if (string.IsNullOrWhiteSpace(store.ApiKey) || string.IsNullOrWhiteSpace(store.Password))
            {
                request.Headers.Add("X-Shopify-Access-Token", store.ShopifyAccessToken);
            }
            else
            {
                string authInfo = $"{store.ApiKey}:{store.Password}";
                string encodedAuthInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));

                request.Headers.Add("Authorization", "Basic " + encodedAuthInfo);
            }
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
            MethodConditions.EnsureArgumentIsNotNull(request, nameof(request));

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
                log.Error($"Error in ProcessRequest for action: '{action}'.", ex);
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
            MethodConditions.EnsureArgumentIsNotNull(request, nameof(request));

            try
            {
                IHttpResponseReader responseReader = request.GetResponse();

                return responseReader;
            }
            catch (WebException ex)
            {
                HttpWebResponse webResponse = ex.Response as HttpWebResponse;
                if (webResponse?.StatusCode == (HttpStatusCode) ShopifyConstants.OverApiLimitStatusCode)
                {
                    throw new RequestThrottledException(ex.Message);
                }

                if (webResponse?.StatusCode == (HttpStatusCode) ShopifyConstants.AlreadyShippedStatusCode)
                {
                    throw new ShopifyAlreadyUploadedException(ex.Message);
                }

                throw;
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

                return queryStringParams[ShopifyConstants.RequestTokenParamName];
            }

            throw new InvalidOperationException($"The requestToken could not be extracted from {requestTokenUrl}");
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
            return queryStringParams?[ShopifyConstants.RequestTokenParamName] != null &&
                   !string.IsNullOrEmpty(queryStringParams[ShopifyConstants.RequestTokenParamName]);
        }

        /// <summary>
        /// Parse the fraud risks
        /// </summary>
        /// <param name="risks">Fraud risks text that should be parsed</param>
        public static IEnumerable<JToken> ParseFraudRisks(string risks)
        {
            if (risks.IsNullOrWhiteSpace())
            {
                return Enumerable.Empty<JToken>();
            }

            return JObject.Parse(risks)?
                .SelectToken("risks")
                .Where(x => x != null);
        }

        /// <summary>
        /// Gets shop information for a Shopify store
        /// </summary>
        public ShopifyShop GetShop()
        {
            try
            {
                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter { Verb = HttpVerb.Get };
                request.Uri = new Uri(Endpoints.ShopUrl);

                // Make the call and get the response
                string shopAsString = ProcessAuthenticatedRequest(request, ShopifyWebClientApiCall.GetShop, progress);

                JToken shop = JObject.Parse(shopAsString);

                if (shop?["shop"] != null)
                {
                    return shop["shop"].ToObject<ShopifyShop>();
                }
                else
                {
                    throw new ShopifyException("Shopify returned an invalid response to ShipWorks.");
                }
            }
            catch (JsonException ex)
            {
                log.Error("An error occurred during JObect.Parse", ex);

                throw new ShopifyException("Shopify returned an invalid response to ShipWorks while retrieving store information.", ex);
            }
        }

        /// <summary>
        /// Get inventory levels for a list of inventory ids
        /// </summary>
        public IEnumerable<ShopifyInventoryLevel> GetInventoryLevels(IEnumerable<long> itemInventoryIdList)
        {
            try
            {
                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter { Verb = HttpVerb.Get };
                request.Uri = new Uri(Endpoints.InventoryLevelUrl(itemInventoryIdList));

                // Make the call and get the response
                string inventoryLevelsString = ProcessAuthenticatedRequest(request, ShopifyWebClientApiCall.GetInventoryLevels, progress);

                JToken inventoryLevels = JObject.Parse(inventoryLevelsString);

                if (inventoryLevels != null)
                {
                    return inventoryLevels.SelectToken("inventory_levels")
                        .Select(x => x.ToObject<ShopifyInventoryLevel>())
                        .ToList();
                }
                else
                {
                    throw new ShopifyException("Shopify returned an invalid response to ShipWorks.");
                }
            }
            catch (JsonException ex)
            {
                log.Error("An error occurred during JObect.Parse", ex);

                throw new ShopifyException("Shopify returned an invalid response to ShipWorks while retrieving inventory levels information.", ex);
            }
        }
    }
}
