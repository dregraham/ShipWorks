using System;
using System.Net;
using System.Text;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Communication;
using System.Web;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    [Component]
    public class ChannelAdvisorRestClient : IChannelAdvisorRestClient
    {
        private const string EndpointBase = "https://api.channeladvisor.com/oauth2";
        private const string ApplicationID = "wx76dgzjcwlfy1ck3nb8oke7ql2ukv05";
        private const string SharedSecret = "Preb8E42ckWZZpFHh6OV2w";

        private readonly string redirectUrl = WebUtility.UrlEncode("https://www.interapptive.com/channeladvisor/subscribe.php");
        private readonly string authorizeEndpoint = $"{EndpointBase}/authorize";
        private readonly string tokenEndpoint = $"{EndpointBase}/token";

        public Uri AuthorizeUrl
        {
            get
            {
                string url = $"{authorizeEndpoint}?client_id={ApplicationID}&response_type=code&access_type=offline&scope=orders+inventory&approval_prompt=force&redirect_uri={redirectUrl}";
                return new Uri(url);
            }
        }

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        public string GetRefreshToken(string code)
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
            submitter.Uri = new Uri(tokenEndpoint);
            submitter.Verb = HttpVerb.Post;
            submitter.ContentType = "application/x-www-form-urlencoded";
            submitter.Headers.Add("Authorization", AuthorizationHeaderValue);
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
        /// Processes the request.
        /// </summary>
        private string ProcessRequest(HttpVariableRequestSubmitter request, string action)
        {
            ApiLogEntry apiLogEntry = new ApiLogEntry(ApiLogSource.ChannelAdvisor, action);
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
        /// Gets the authorization header.
        /// </summary>
        private string AuthorizationHeaderValue => $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{ApplicationID}:{SharedSecret}"))}";
    }
}