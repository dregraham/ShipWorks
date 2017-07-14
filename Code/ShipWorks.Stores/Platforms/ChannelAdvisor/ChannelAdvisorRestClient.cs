using System;
using System.Text;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Logging;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Web client for interacting with ChannelAdvisors REST API
    /// </summary>
    [Component]
    public class ChannelAdvisorRestClient : IChannelAdvisorRestClient
    {
        private readonly Func<IHttpVariableRequestSubmitter> submitterFactory;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private readonly IEncryptionProvider encryptionProvider;

        private const string EncryptedSharedSecret = "hij91GRVDQQP9SvJq7tKvrTVAyaqNeyG8AwzcuRHXg4=";

        public const string EndpointBase = "https://api.channeladvisor.com";
        private readonly string tokenEndpoint = $"{EndpointBase}/oauth2/token";
        private readonly string ordersEndpoint = $"{EndpointBase}/v1/Orders";
        private readonly string profilesEndpoint = $"{EndpointBase}/v1/Profiles";

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelAdvisorRestClient"/> class.
        /// </summary>
        /// <param name="submitterFactory">The submitter factory.</param>
        /// <param name="apiLogEntryFactory">The API log entry factory.</param>
        /// <param name="encryptionProviderFactory"></param>
        public ChannelAdvisorRestClient(Func<IHttpVariableRequestSubmitter> submitterFactory,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory,
            IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.submitterFactory = submitterFactory;
            this.apiLogEntryFactory = apiLogEntryFactory;
            encryptionProvider = encryptionProviderFactory.CreateChannelAdvisorEncryptionProvider();
        }

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        public string GetRefreshToken(string code, string redirectUrl)
        {
            IHttpVariableRequestSubmitter submitter = CreateRequest(tokenEndpoint, HttpVerb.Post);

            submitter.Variables.Add("grant_type", "authorization_code");
            submitter.Variables.Add("code", code);
            submitter.Variables.Add(new HttpVariable("redirect_uri", redirectUrl, false));

            ChannelAdvisorOAuthResponse response =
                ProcessRequest<ChannelAdvisorOAuthResponse>(submitter, "GetRefreshToken");

            if (string.IsNullOrWhiteSpace(response.RefreshToken))
            {
                throw new ChannelAdvisorException("Response did not contain a refresh token.");
            }

            return response.RefreshToken;
        }

        /// <summary>
        /// Get the access token given a refresh token
        /// </summary>
        public string GetAccessToken(string refreshToken)
        {
            IHttpVariableRequestSubmitter submitter = CreateRequest(tokenEndpoint, HttpVerb.Post);

            submitter.Variables.Add("grant_type", "refresh_token");
            submitter.Variables.Add("refresh_token", refreshToken);

            ChannelAdvisorOAuthResponse response =
                ProcessRequest<ChannelAdvisorOAuthResponse>(submitter, "GetAccessToken");

            if (string.IsNullOrWhiteSpace(response.AccessToken))
            {
                throw new ChannelAdvisorException("Response did not contain an access token.");
            }

            return response.AccessToken;
        }

        /// <summary>
        /// Get profile info for the given token
        /// </summary>
        public ChannelAdvisorProfilesResponse GetProfiles(string accessToken)
        {
            IHttpVariableRequestSubmitter submitter = CreateRequest(profilesEndpoint, HttpVerb.Get);

            submitter.Variables.Add("access_token", accessToken);

            return ProcessRequest<ChannelAdvisorProfilesResponse>(submitter, "GetProfiles");
        }

        /// <summary>
        /// Get orders from the start date for the store
        /// </summary>
        public ChannelAdvisorOrderResult GetOrders(DateTime start, string accessToken)
        {
            IHttpVariableRequestSubmitter submitter = CreateRequest(ordersEndpoint, HttpVerb.Get);

            submitter.Variables.Add("access_token", accessToken);

            // Manually formate the date because the Universal Sortable Date Time format does not include milliseconds but CA does include milliseconds
            submitter.Variables.Add("$filter", $"CreatedDateUtc gt {start:yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ff'Z'}");
            submitter.Variables.Add("$orderby", "CreatedDateUtc desc");
            submitter.Variables.Add("$count", "true");

            return ProcessRequest<ChannelAdvisorOrderResult>(submitter, "GetOrders");
        }

        /// <summary>
        /// Create a request to channel advisor
        /// </summary>
        private IHttpVariableRequestSubmitter CreateRequest(string endpoint, HttpVerb method)
        {
            IHttpVariableRequestSubmitter submitter = submitterFactory();
            submitter.Uri = new Uri(endpoint);
            submitter.Verb = method;

            submitter.ContentType = "application/x-www-form-urlencoded";
            AuthenticateRequest(submitter);

            return submitter;
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        private T ProcessRequest<T>(IHttpRequestSubmitter request, string action)
        {
            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.ChannelAdvisor, action);
            apiLogEntry.LogRequest(request);

            try
            {
                IHttpResponseReader httpResponseReader = request.GetResponse();
                string result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");

                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception ex)
            {
                apiLogEntry.LogResponse(ex);
                throw new ChannelAdvisorException("Error communicating with ChannelAdvisor REST API", ex);
            }
        }

        /// <summary>
        /// Gets the authorization header value.
        /// </summary>
        private void AuthenticateRequest(IHttpRequestSubmitter request)
        {
            try
            {
                string authHeader =
                    $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{ChannelAdvisorStoreType.ApplicationID}:{encryptionProvider.Decrypt(EncryptedSharedSecret)}"))}";
                request.Headers.Add("Authorization", authHeader);
            }
            catch (EncryptionException ex)
            {
                throw new ChannelAdvisorException("Failed to decrypt the shared secret", ex);
            }
        }
    }
}