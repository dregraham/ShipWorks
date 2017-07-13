using System;
using System.Text;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Logging;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;
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
        public const string EndpointBase = "https://api.channeladvisor.com";
        private readonly string tokenEndpoint = $"{EndpointBase}/oauth2/token";
        private readonly string ordersEndpoint = $"{EndpointBase}/v1/Orders";
        private const string EncryptedSharedSecret = "hij91GRVDQQP9SvJq7tKvrTVAyaqNeyG8AwzcuRHXg4=";

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
            IHttpVariableRequestSubmitter submitter = submitterFactory();
            submitter.Uri = new Uri(tokenEndpoint);
            submitter.Verb = HttpVerb.Post;
            submitter.Variables.Add("grant_type", "authorization_code");
            submitter.Variables.Add("code", code);
            submitter.Variables.Add(new HttpVariable("redirect_uri", redirectUrl, false));

            ChannelAdvisorOAuthResponse response =
                JsonConvert.DeserializeObject<ChannelAdvisorOAuthResponse>(ProcessRequest(submitter, "GetRefreshToken"));

            if (string.IsNullOrWhiteSpace(response.RefreshToken))
            {
                throw new ChannelAdvisorException("Response did not contain a refresh token.");
            }

            return response.RefreshToken;
        }

        /// <summary>
        /// Get the access token given a refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        private string GetAccessToken(string refreshToken)
        {
            IHttpVariableRequestSubmitter submitter = submitterFactory();
            submitter.Uri = new Uri(tokenEndpoint);
            submitter.Verb = HttpVerb.Post;
            submitter.Variables.Add("grant_type", "refresh_token");
            submitter.Variables.Add("refresh_token", refreshToken);

            ChannelAdvisorOAuthResponse response =
                JsonConvert.DeserializeObject<ChannelAdvisorOAuthResponse>(ProcessRequest(submitter, "GetAccessToken"));

            if (string.IsNullOrWhiteSpace(response.AccessToken))
            {
                throw new ChannelAdvisorException("Response did not contain an access token.");
            }

            return response.AccessToken;
        }

        /// <summary>
        /// Get orders from the start date for the store
        /// </summary>
        public ChannelAdvisorOrderResult GetOrders(DateTime start, ChannelAdvisorStoreEntity store)
        {
            IHttpVariableRequestSubmitter submitter = submitterFactory();
            submitter.Uri = new Uri(ordersEndpoint);
            submitter.Verb = HttpVerb.Get;

            submitter.Variables.Add("access_token", GetAccessToken(store.RefreshToken));
            submitter.Variables.Add("filter", $"CreatedDateUtc gt {start:s}");
            submitter.Variables.Add("orderby", "orderby=CreatedDateUtc desc");
            
            return JsonConvert.DeserializeObject<ChannelAdvisorOrderResult>(ProcessRequest(submitter, "GetOrders"));
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        private string ProcessRequest(IHttpRequestSubmitter request, string action)
        {
            request.ContentType = "application/x-www-form-urlencoded";
            AuthenticateRequest(request);

            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.ChannelAdvisor, action);
            apiLogEntry.LogRequest(request);

            try
            {
                IHttpResponseReader httpResponseReader = request.GetResponse();
                string result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");

                return result;
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