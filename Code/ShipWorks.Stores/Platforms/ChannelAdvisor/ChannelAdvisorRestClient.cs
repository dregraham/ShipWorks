using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;
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

        public const string EndpointBase = "https://api.channeladvisor.com";
        private readonly string tokenEndpoint = $"{EndpointBase}/oauth2/token";
        private readonly string ordersEndpoint = $"{EndpointBase}/v1/Orders";
        private readonly string profilesEndpoint = $"{EndpointBase}/v1/Profiles";
        private readonly string productEndpoint = $"{EndpointBase}/v1/Products";
        private readonly string distributionCenterEndpoint = $"{EndpointBase}/v1/DistributionCenters";

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelAdvisorRestClient"/> class.
        /// </summary>
        /// <param name="submitterFactory">The submitter factory.</param>
        /// <param name="apiLogEntryFactory">The API log entry factory.</param>
        /// <param name="encryptionProviderFactory"></param>
        public ChannelAdvisorRestClient(IHttpRequestSubmitterFactory submitterFactory,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory,
            IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.submitterFactory = submitterFactory;
            this.apiLogEntryFactory = apiLogEntryFactory;
            encryptionProvider = encryptionProviderFactory.CreateChannelAdvisorEncryptionProvider();

            accessTokenCache = new LruCache<string, string>(50, TimeSpan.FromMinutes(50));
            productCache = new LruCache<int, ChannelAdvisorProduct>(1000);
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
        public ChannelAdvisorProfilesResponse GetProfiles(string refreshToken)
        {
            IHttpVariableRequestSubmitter submitter = CreateRequest(profilesEndpoint, HttpVerb.Get);

            submitter.Variables.Add("access_token", GetAccessToken(refreshToken));

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
        /// Get orders from the start date for the store
        /// </summary>
        public ChannelAdvisorOrderResult GetOrders(DateTime start, string refreshToken)
        {
            IHttpVariableRequestSubmitter submitter = CreateRequest(ordersEndpoint, HttpVerb.Get);

            submitter.Variables.Add("access_token", GetAccessToken(refreshToken));

            // Manually formate the date because the Universal Sortable Date Time format does not include milliseconds but CA does include milliseconds
            submitter.Variables.Add("$filter", $"PaymentDateUtc gt {start:yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff'Z'}");
            submitter.Variables.Add("$count", "true");
            submitter.Variables.Add("$orderby", "CreatedDateUtc");
            submitter.Variables.Add("$expand", "Fulfillments,Items($expand=FulfillmentItems)");

            return ProcessRequest<ChannelAdvisorOrderResult>(submitter, "GetOrders", refreshToken);
        }

        /// <summary>
        /// Get orders from the start date for the store
        /// </summary>
        public ChannelAdvisorOrderResult GetOrders(string nextToken, string refreshToken)
        {
            IHttpVariableRequestSubmitter submitter = CreateRequest(nextToken, HttpVerb.Get);

            return ProcessRequest<ChannelAdvisorOrderResult>(submitter, "GetOrders", refreshToken);
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

                IHttpVariableRequestSubmitter submitter = CreateRequest($"{productEndpoint}({productID})", HttpVerb.Get);

                submitter.Variables.Add("access_token", GetAccessToken(refreshToken));
                submitter.Variables.Add("$expand", "Attributes, Images, DCQuantities");

                ChannelAdvisorProduct product = ProcessRequest<ChannelAdvisorProduct>(submitter, "GetProduct", refreshToken);

                productCache[productID] = product;

                return product;
            }
            catch (ChannelAdvisorException e)
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
            catch (Newtonsoft.Json.JsonException ex)
            {
                throw new ChannelAdvisorException("Error creating ChannelAdvisor shipment request.", ex);
            }

            IHttpRequestSubmitter submitter =
                submitterFactory.GetHttpTextPostRequestSubmitter(requestBody, "application/json");
            submitter.Uri =
                new Uri($"{ordersEndpoint}({channelAdvisorOrderID})/Ship?access_token={GetAccessToken(refreshToken, isRetry)}");

            // NoContent is the expected response form ChannelAdvisor for a sucessful upload
            submitter.AllowHttpStatusCodes(HttpStatusCode.NoContent);

            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.ChannelAdvisor, "UploadShipmentDetails");
            apiLogEntry.LogRequest(submitter);

            try
            {
                IHttpResponseReader httpResponseReader = submitter.GetResponse();
                string result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");
            }
            catch (WebException ex) when (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized &&
                                          !isRetry)
            {
                apiLogEntry.LogResponse(ex);
                UploadShipmentDetails(channelAdvisorShipment, refreshToken, channelAdvisorOrderID, true);
            }
            catch (Exception ex)
            {
                apiLogEntry.LogResponse(ex);
                throw new ChannelAdvisorException("Error communicating with ChannelAdvisor REST API", ex);
            }
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
            catch (WebException ex) when (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized && generateNewTokenIfExpired)
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
            request.Variables.Remove("access_token");
            request.Variables.Add("access_token", GetAccessToken(refreshToken, true));

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
    }
}