using System;
using System.Net;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Platforms.Jet.DTO;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Communicates with the Jet Rest API
    /// </summary>
    /// <seealso cref="IJetWebClient" />
    [Component]
    public class JetWebClient : IJetWebClient
    {
        private readonly IHttpRequestSubmitterFactory submitterFactory;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;

        private const string EndpointBase = "https://merchant-api.jet.com/api/";
        private readonly string tokenEndpoint = $"{EndpointBase}/token";

        /// <summary>
        /// Constructor
        /// </summary>
        public JetWebClient(IHttpRequestSubmitterFactory submitterFactory, Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            this.submitterFactory = submitterFactory;
            this.apiLogEntryFactory = apiLogEntryFactory;
        }

        /// <summary>
        /// Get Token
        /// </summary>
        public GenericResult<string> GetToken(string username, string password)
        {
            IHttpRequestSubmitter submitter = submitterFactory.GetHttpTextPostRequestSubmitter(
                $"{{\"user\": \"{username}\",\"pass\":\"{password}\"}}",
                "application/json");

            submitter.Uri = new Uri(tokenEndpoint);

            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.Jet, "GetToken");
            apiLogEntry.LogRequest(submitter);

            try
            {
                IHttpResponseReader httpResponseReader = submitter.GetResponse();
                string result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");

                JetTokenResponse token = JsonConvert.DeserializeObject<JetTokenResponse>(result, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });

                return GenericResult.FromSuccess(token.Token);
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.BadRequest)
            {
                apiLogEntry.LogResponse(ex);
                return GenericResult.FromError<string>("Invalid API User or Secret.");
            }
            catch (Exception ex)
            {
                apiLogEntry.LogResponse(ex);
                return GenericResult.FromError<string>("Error communicating with Jet.");
            }
        }
    }
}