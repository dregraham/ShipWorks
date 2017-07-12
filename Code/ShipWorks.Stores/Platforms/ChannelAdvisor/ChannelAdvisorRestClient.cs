using System;
using System.Net;
using System.Text;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Communication;
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
        public const string EndpointBase = "https://api.channeladvisor.com/oauth2";
        private readonly string tokenEndpoint = $"{EndpointBase}/token";
		private const string SharedSecret = "Preb8E42ckWZZpFHh6OV2w";
		
        public ChannelAdvisorRestClient(Func<IHttpVariableRequestSubmitter> submitterFactory,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            this.submitterFactory = submitterFactory;
            this.apiLogEntryFactory = apiLogEntryFactory;
        }
        

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        public string GetRefreshToken(string code)
        {
            IHttpVariableRequestSubmitter submitter = submitterFactory();
            submitter.Uri = new Uri(tokenEndpoint);
            submitter.Verb = HttpVerb.Post;
            submitter.ContentType = "application/x-www-form-urlencoded";
            submitter.Headers.Add("Authorization", GetAuthorizationHeaderValue);
            submitter.Variables.Add("grant_type", "authorization_code");
            submitter.Variables.Add("code", code);
            submitter.Variables.Add(new HttpVariable("redirect_uri", ChannelAdvisorStoreType.RedirectUrl, false));

            ChannelAdvisorOAuthResponse response =
                JsonConvert.DeserializeObject<ChannelAdvisorOAuthResponse>(ProcessRequest(submitter, "GetRefreshToken"));

            if (string.IsNullOrWhiteSpace(response.RefreshToken))
            {
                throw new ChannelAdvisorException("Response did not contain a refresh token.");
            }

            return response.RefreshToken;
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        private string ProcessRequest(IHttpRequestSubmitter request, string action)
        {
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
        private static string GetAuthorizationHeaderValue => $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{ChannelAdvisorStoreType.ApplicationID}:{SharedSecret}"))}";
    }
}