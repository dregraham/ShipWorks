using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Web client for interacting with ChannelAdvisors REST API
    /// </summary>
    [Component(SingleInstance = true)]
    public class ChannelAdvisorRestClient : IChannelAdvisorRestClient
    {
        private readonly LruCache<string, string> accessTokenCache;
        private readonly LruCache<int, ChannelAdvisorProduct> productCache;

        private readonly IHttpRequestSubmitterFactory submitterFactory;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private readonly IEncryptionProvider encryptionProvider;

        private const string EncryptedSharedSecret = "hij91GRVDQQP9SvJq7tKvrTVAyaqNeyG8AwzcuRHXg4=";

        public const string defaultEndpointBase = "https://api.channeladvisor.com";
        private readonly string tokenEndpoint;
        private readonly string ordersEndpoint;
        private readonly string profilesEndpoint;
        private readonly string productEndpoint;
        private readonly string distributionCenterEndpoint;
        private readonly IInterapptiveOnly interapptiveOnly;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelAdvisorRestClient"/> class.
        /// </summary>
        /// <param name="submitterFactory">The submitter factory.</param>
        /// <param name="apiLogEntryFactory">The API log entry factory.</param>
        /// <param name="encryptionProviderFactory"></param>
        public ChannelAdvisorRestClient(IHttpRequestSubmitterFactory submitterFactory,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory,
            IEncryptionProviderFactory encryptionProviderFactory,
            IInterapptiveOnly interapptiveOnly)
        {
            this.interapptiveOnly = interapptiveOnly;
            this.submitterFactory = submitterFactory;
            this.apiLogEntryFactory = apiLogEntryFactory;
            encryptionProvider = encryptionProviderFactory.CreateChannelAdvisorEncryptionProvider();

            accessTokenCache = new LruCache<string, string>(50, TimeSpan.FromMinutes(50));
            productCache = new LruCache<int, ChannelAdvisorProduct>(1000);

            var endpointBase = GetEndpointBase();
            tokenEndpoint = $"{endpointBase}/oauth2/token";
            ordersEndpoint = $"{endpointBase}/v1/Orders";
            profilesEndpoint = $"{endpointBase}/v1/Profiles";
            productEndpoint = $"{endpointBase}/v1/Products";
            distributionCenterEndpoint = $"{endpointBase}/v1/DistributionCenters";
        }

        /// <summary>
        /// Get the base endpoint for ChannelAdvisor requests
        /// </summary>
        public string GetEndpointBase()
        {
            if (UseFakeApi)
            {
                var endpointOverride = interapptiveOnly.Registry.GetValue("ChannelAdvisorEndpoint", string.Empty);
                if (!string.IsNullOrWhiteSpace(endpointOverride))
                {
                    return endpointOverride;
                }
            }

            return defaultEndpointBase;
        }

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        public GenericResult<string> GetRefreshToken(string code, string redirectUrl)
        {
            IHttpVariableRequestSubmitter submitter = CreateRequest(tokenEndpoint, HttpVerb.Post);

            submitter.Variables.Add("grant_type", "authorization_code");
            submitter.Variables.Add("code", code);
            submitter.Variables.Add(new HttpVariable("redirect_uri", redirectUrl, false));

            ChannelAdvisorOAuthResponse response =
                ProcessRequest<ChannelAdvisorOAuthResponse>(submitter, "GetRefreshToken", string.Empty);

            if (string.IsNullOrWhiteSpace(response.RefreshToken))
            {
                return GenericResult.FromError<string>("Response did not contain a refresh token.");
            }

            return GenericResult.FromSuccess(response.RefreshToken);
        }

        /// <summary>
        /// Get the access token given a refresh token
        /// </summary>
        private string GetAccessToken(string refreshToken, bool generateNewToken = false)
        {
            if (accessTokenCache.Contains(refreshToken) && !generateNewToken)
            {
                return accessTokenCache[refreshToken];
            }

            IHttpVariableRequestSubmitter submitter = CreateRequest(tokenEndpoint, HttpVerb.Post);

            submitter.Variables.Add("grant_type", "refresh_token");
            submitter.Variables.Add("refresh_token", refreshToken);

            ChannelAdvisorOAuthResponse response =
                ProcessRequest<ChannelAdvisorOAuthResponse>(submitter, "GetAccessToken", refreshToken, false);

            if (string.IsNullOrWhiteSpace(response.AccessToken))
            {
                throw new ChannelAdvisorException("Response did not contain an access token.");
            }

            accessTokenCache[refreshToken] = response.AccessToken;

            return response.AccessToken;
        }

        /// <summary>
        /// Get profile info for the given token
        /// </summary>
        /// <remarks>
        /// Expanding sites throws errors for some customers. If setting expandSites to true, make sure to 
        /// catch any error that comes of it.
        /// </remarks>
        public ChannelAdvisorProfilesResponse GetProfiles(string refreshToken, bool expandSites)
        {
            IHttpVariableRequestSubmitter submitter = CreateRequest(profilesEndpoint, HttpVerb.Get);

            submitter.Variables.Add("access_token", GetAccessToken(refreshToken));
            if (expandSites)
            {
                submitter.Variables.Add("$expand", "SiteAccounts");
            }

            return ProcessRequest<ChannelAdvisorProfilesResponse>(submitter, "GetProfiles", refreshToken);
        }

        /// <summary>
        /// Gets the distribution centers.
        /// </summary>
        public ChannelAdvisorDistributionCenterResponse GetDistributionCenters(string refreshToken)
        {
            IHttpVariableRequestSubmitter submitter = CreateRequest(distributionCenterEndpoint, HttpVerb.Get);

            submitter.Variables.Add("access_token", GetAccessToken(refreshToken));

            return ProcessRequest<ChannelAdvisorDistributionCenterResponse>(submitter, "GetDistributionCenters", refreshToken);
        }

        /// <summary>
        /// Gets the next batch of distribution centers
        /// </summary>
        public ChannelAdvisorDistributionCenterResponse GetDistributionCenters(string nextToken, string refreshToken)
        {
            IHttpVariableRequestSubmitter submitter = CreateRequest(nextToken, HttpVerb.Get);

            return ProcessRequest<ChannelAdvisorDistributionCenterResponse>(submitter, "GetDistributionCenters", refreshToken);
        }

        /// <summary>
        /// Get orders from the start date for the store
        /// </summary>
        public ChannelAdvisorOrderResult GetOrders(int daysBack, string refreshToken, bool includeExternallyManagedDistributionCenters)
        {
            IHttpVariableRequestSubmitter getOrdersRequestSubmitter = CreateRequest(ordersEndpoint, HttpVerb.Get);

            var clampedDaysBack = daysBack.Clamp(1, 30);

            var downloadStartDate = DateTime.UtcNow.AddDays(-clampedDaysBack);

            string ordersFilter = "(ShippingStatus eq 'Unshipped' OR ShippingStatus eq 'PendingShipment' OR ShippingStatus eq 'PartiallyShipped' OR ShippingStatus eq 'ThirdPartyManaged') AND " +
                "(CheckoutStatus eq 'Completed' OR CheckoutStatus eq 'CompletedAndVisited' OR CheckoutStatus eq 'CompletedOffline') AND " +
                "(PaymentStatus eq 'Cleared' OR PaymentStatus eq 'Submitted' OR PaymentStatus eq 'Deposited')";

            if (includeExternallyManagedDistributionCenters)
            {
                ordersFilter = "(" + ordersFilter + ")" + "OR DistributionCenterTypeRollup eq 'ExternallyManaged' ";
            }
            else
            {
                ordersFilter += "AND DistributionCenterTypeRollup ne 'ExternallyManaged' ";
            }

            ordersFilter += $"AND (CreatedDateUtc ge {downloadStartDate:o})";

            getOrdersRequestSubmitter.Variables.Add("access_token", GetAccessToken(refreshToken));
            getOrdersRequestSubmitter.Variables.Add("$filter", ordersFilter);
            getOrdersRequestSubmitter.Variables.Add("$orderby", "CreatedDateUtc desc");
            getOrdersRequestSubmitter.Variables.Add("$expand", "Fulfillments,Items($expand=FulfillmentItems)");

            return SubmitGetOrders(getOrdersRequestSubmitter, refreshToken);
        }

        /// <summary>
        /// Gets the next batch of orders
        /// </summary>
        public ChannelAdvisorOrderResult GetOrders(string nextToken, string refreshToken)
        {
            IHttpVariableRequestSubmitter getOrdersRequestSubmitter = CreateRequest(nextToken, HttpVerb.Get);

            return SubmitGetOrders(getOrdersRequestSubmitter, refreshToken);
        }

        /// <summary>
        /// Gets the next batch of order items
        /// </summary>
        public ChannelAdvisorOrderItemsResult GetOrderItems(string nextToken, string refreshToken)
        {
            IHttpVariableRequestSubmitter submitter = CreateRequest(nextToken, HttpVerb.Get);

            return Functional
                .Retry(() => ProcessRequest<ChannelAdvisorOrderItemsResult>(submitter, "GetOrders", refreshToken), 10, ShouldRetryRequest)
                .Match(x => x, ex => throw ex);
        }

        /// <summary>
        /// Fetches the given products and adds them to the cache if they aren't already in it
        /// </summary>
        public void AddProductsToCache(IEnumerable<int> productIds, string refreshToken)
        {
            var uncachedIds = productIds.Where(x => !productCache.Contains(x));

            if (uncachedIds.None())
            {
                return;
            }

            try
            {
                string previousLink = string.Empty;

                ChannelAdvisorProductList productBatch = GetProductBatch(uncachedIds, refreshToken);

                while (productBatch?.Products?.Any() == true)
                {
                    if (productBatch.OdataNextLink?.Equals(previousLink) == true)
                    {
                        return;
                    }

                    previousLink = productBatch.OdataNextLink;

                    productBatch.Products.ForEach(x => productCache[x.ID] = x);

                    productBatch = string.IsNullOrEmpty(productBatch.OdataNextLink) ? null : GetProductBatch(previousLink, refreshToken);
                }
            }
            catch (ChannelAdvisorException)
            {
                // Log should already be written.
                return;
            }
        }

        /// <summary>
        /// Get a batch of products
        /// </summary>
        private ChannelAdvisorProductList GetProductBatch(IEnumerable<int> productIds, string refreshToken)
        {
            IHttpVariableRequestSubmitter submitter = CreateRequest(productEndpoint, HttpVerb.Get);

            submitter.Variables.Add("access_token", GetAccessToken(refreshToken));
            submitter.Variables.Add("$filter", $"ID eq {string.Join(" or ID eq ", productIds)}");
            submitter.Variables.Add("$expand", "Attributes, Images, DCQuantities");

            return SubmitGetProductBatch(submitter, refreshToken);
        }

        /// <summary>
        /// Get a batch of products
        /// </summary>
        private ChannelAdvisorProductList GetProductBatch(string link, string refreshToken)
        {
            IHttpVariableRequestSubmitter submitter = CreateRequest(link, HttpVerb.Get);

            return SubmitGetProductBatch(submitter, refreshToken);
        }

        /// <summary>
        /// Submit the get product batch request
        /// </summary>
        private ChannelAdvisorProductList SubmitGetProductBatch(IHttpVariableRequestSubmitter submitter, string refreshToken)
        {
            return Functional.Retry(() => ProcessRequest<ChannelAdvisorProductList>(submitter, "GetProduct", refreshToken), 10, ShouldRetryRequest)
                .Match(x => x, ex => throw ex);
        }

        /// <summary>
        /// Get detailed product information from ChannelAdvisor with the given product ID
        /// </summary>
        public ChannelAdvisorProduct GetProduct(int productID, string refreshToken)
        {
            try
            {
                if (productCache.Contains(productID))
                {
                    return productCache[productID];
                }

                var productSpecificEndpoint = UseFakeApi ?
                    $"{productEndpoint}/{productID}" :
                    $"{productEndpoint}({productID})";

                IHttpVariableRequestSubmitter submitter = CreateRequest(productSpecificEndpoint, HttpVerb.Get);

                submitter.Variables.Add("access_token", GetAccessToken(refreshToken));
                submitter.Variables.Add("$expand", "Attributes, Images, DCQuantities");

                ChannelAdvisorProduct product = Functional
                    .Retry(() => ProcessRequest<ChannelAdvisorProduct>(submitter, "GetProduct", refreshToken), 10, ShouldRetryRequest)
                    .Match(x => x, ex => throw ex);

                productCache[productID] = product;

                return product;
            }
            catch (ChannelAdvisorException)
            {
                // Log should already be written. Return null
                return null;
            }
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        public void UploadShipmentDetails(ChannelAdvisorShipment channelAdvisorShipment,
            string refreshToken,
            string channelAdvisorOrderID) =>
            UploadShipmentDetails(channelAdvisorShipment, refreshToken, channelAdvisorOrderID, false);

        /// <summary>
        /// Submits a GetOrders request
        /// </summary>
        private ChannelAdvisorOrderResult SubmitGetOrders(IHttpVariableRequestSubmitter getOrdersRequestSubmitter, string refreshToken)
        {
            // Increase timeout due to filtering a large number of orders taking a long time
            getOrdersRequestSubmitter.Timeout = TimeSpan.FromSeconds(300);

            return Functional
                .Retry(() => ProcessRequest<ChannelAdvisorOrderResult>(getOrdersRequestSubmitter, "GetOrders", refreshToken), 10, ShouldRetryRequest)
                .Match(x => x, ex => throw ex);
        }

        /// <summary>
        /// Determines if the exception is a timeout or internal server error
        /// </summary>
        private static bool ShouldRetryRequest(Exception ex)
        {
            return (ex as ChannelAdvisorException)?.InnerException is WebException;
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        private void UploadShipmentDetails(ChannelAdvisorShipment channelAdvisorShipment,
            string refreshToken,
            string channelAdvisorOrderID,
            bool isRetry)
        {
            string requestBody;
            try
            {
                string serializedShipment =
                    JsonConvert.SerializeObject(channelAdvisorShipment,
                        new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddThh:mm:ssZ" });

                requestBody = $"{{\"Value\":{serializedShipment}}}";
            }
            catch (JsonException ex)
            {
                throw new ChannelAdvisorException("Error creating ChannelAdvisor shipment request.", ex);
            }

            IHttpRequestSubmitter submitter =
                submitterFactory.GetHttpTextPostRequestSubmitter(requestBody, "application/json");
            submitter.Uri =
                new Uri($"{ordersEndpoint}({channelAdvisorOrderID})/Ship?access_token={GetAccessToken(refreshToken, isRetry)}");

            // NoContent is the expected response form ChannelAdvisor for a sucessful upload
            submitter.AllowHttpStatusCodes(HttpStatusCode.NoContent, HttpStatusCode.BadRequest);

            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.ChannelAdvisor, "UploadShipmentDetails");
            apiLogEntry.LogRequest(submitter);

            const string unknownError = "Error communicating with ChannelAdvisor REST API";
            IHttpResponseReader httpResponseReader = null;
            string result = string.Empty;

            try
            {
                httpResponseReader = submitter.GetResponse();
                result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");
            }
            catch (WebException ex) when (((HttpWebResponse) ex.Response).StatusCode == HttpStatusCode.Unauthorized &&
                                          !isRetry)
            {
                apiLogEntry.LogResponse(ex);
                UploadShipmentDetails(channelAdvisorShipment, refreshToken, channelAdvisorOrderID, true);
            }
            catch (Exception ex)
            {
                apiLogEntry.LogResponse(ex);
                throw new ChannelAdvisorException(unknownError, ex);
            }

            if (httpResponseReader?.HttpWebResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new ChannelAdvisorException(GetErrorMessage(result, channelAdvisorShipment) ?? unknownError);
            }
        }

        /// <summary>
        /// Parse the channel advisor error message. Null if no error.message
        /// </summary>
        private string GetErrorMessage(string result, ChannelAdvisorShipment channelAdvisorShipment)
        {
            string errorMessage;
            try
            {
                errorMessage = JObject.Parse(result).SelectToken("error.message")?.Value<string>() ??
                    JObject.Parse(result).SelectToken("Message")?.Value<string>() ??
                    null;

                if(errorMessage == "Carrier and Class is invalid.")
                {
                    errorMessage = $"The shipment details upload failed for Carrier '{channelAdvisorShipment.ShippingCarrier}' and Class '{channelAdvisorShipment.ShippingClass}'.\r\n" +
                                                          "Update your ChannelAdvisor store's Account Shipping Carriers to include these values as supported carriers.\r\n" +
                                                          "The supported carriers are located at Fulfill > Carriers menu in your online store.\r\n\r\n";
                            
                }
            }
            catch (Exception)
            {
                errorMessage = null;
            }

            return errorMessage;
        }

        /// <summary>
        /// Create a request to channel advisor
        /// </summary>
        private IHttpVariableRequestSubmitter CreateRequest(string endpoint, HttpVerb method)
        {
            IHttpVariableRequestSubmitter submitter = submitterFactory.GetHttpVariableRequestSubmitter();
            submitter.Uri = new Uri(endpoint);
            submitter.Verb = method;

            submitter.ContentType = "application/x-www-form-urlencoded";
            AuthenticateRequest(submitter);

            return submitter;
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        private T ProcessRequest<T>(IHttpVariableRequestSubmitter request, string action, string refreshToken, bool generateNewTokenIfExpired = true)
        {
            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.ChannelAdvisor, action);
            apiLogEntry.LogRequest(request);

            try
            {
                IHttpResponseReader httpResponseReader = request.GetResponse();
                string result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");

                return JsonConvert.DeserializeObject<T>(result, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });
            }
            catch (WebException ex) when (((HttpWebResponse) ex.Response).StatusCode == HttpStatusCode.Unauthorized && generateNewTokenIfExpired)
            {
                apiLogEntry.LogResponse(ex);
                return RetryRequestWithNewToken<T>(request, action, refreshToken);
            }
            catch (Exception ex)
            {
                apiLogEntry.LogResponse(ex);
                throw new ChannelAdvisorException("Error communicating with ChannelAdvisor REST API", ex);
            }
        }

        /// <summary>
        /// Retry the request with a new access token
        /// </summary>
        private T RetryRequestWithNewToken<T>(IHttpVariableRequestSubmitter request, string action, string refreshToken)
        {
            // If the request has variables, it must be the initial request, so replace the variable.
            // If not, the url came from the last get order response, and the variables are already in the query string,
            // so adjust them there.
            if (request.Variables.Any())
            {
                request.Variables.Remove("access_token");
                request.Variables.Add("access_token", GetAccessToken(refreshToken, true));
            }
            else
            {
                NameValueCollection parameterValues = HttpUtility.ParseQueryString(request.Uri.Query);
                parameterValues.Set("access_token", GetAccessToken(refreshToken, true));
                string url = request.Uri.AbsolutePath;
                request.Uri = new Uri(GetEndpointBase() + url + "?" + parameterValues);
            }

            return ProcessRequest<T>(request, action, refreshToken, false);
        }


        /// <summary>
        /// Gets the authorization header value.
        /// </summary>
        private void AuthenticateRequest(IHttpRequestSubmitter request)
        {
            try
            {
                string appId = ChannelAdvisorStoreType.ApplicationID;
                string sharedSecret = encryptionProvider.Decrypt(EncryptedSharedSecret);
                string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{appId}:{sharedSecret}"));

                request.Headers.Add("Authorization", $"Basic {auth}");
            }
            catch (EncryptionException ex)
            {
                throw new ChannelAdvisorException("Failed to decrypt the shared secret", ex);
            }
        }

        /// <summary>
        /// Should the client use the fake api
        /// </summary>
        private bool UseFakeApi =>
            interapptiveOnly.IsInterapptiveUser && !interapptiveOnly.Registry.GetValue("ChannelAdvisorLive", true);
    }
}