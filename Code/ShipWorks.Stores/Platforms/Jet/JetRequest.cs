using System;
using System.Net;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet.DTO;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// An implementation of the IJetRequest interface for submitting requests to the Jet.com API. This 
    /// implementation will handle/manage any authentication that may be required for submitting requests
    /// to the API.
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Platforms.Jet.IJetRequest" />
    [Component]
    public class JetRequest : IJetRequest
    {
        public const string EndpointBase = "https://merchant-api.jet.com/api";

        private readonly IHttpRequestSubmitterFactory submitterFactory;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly string tokenEndpoint = $"{EndpointBase}/token";
        private readonly LruCache<string, string> tokenCache;

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="JetRequest"/> class.
        /// </summary>
        public JetRequest(IHttpRequestSubmitterFactory submitterFactory, Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory, IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.submitterFactory = submitterFactory;
            this.apiLogEntryFactory = apiLogEntryFactory;
            this.encryptionProviderFactory = encryptionProviderFactory;
            tokenCache = new LruCache<string, string>(50, TimeSpan.FromHours(9));
        }

        /// <summary>
        /// Get Token
        /// </summary>
        public GenericResult<string> GetToken(string username, string password)
        {
            if (tokenCache.Contains(username))
            {
                return GenericResult.FromSuccess(tokenCache[username]);
            }

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

                JetTokenResponse token = JsonConvert.DeserializeObject<JetTokenResponse>(result, jsonSerializerSettings);
                tokenCache[username] = token.Token;

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

        /// <summary>
        /// Processes the request.
        /// </summary>
        public GenericResult<T> ProcessRequest<T>(string action, string path, HttpVerb method, JetStoreEntity store)
        {
            IHttpRequestSubmitter request = submitterFactory.GetHttpVariableRequestSubmitter();
            request.Uri = new Uri(EndpointBase + path);
            request.Verb = method;

            var result =  ProcessRequest(action, store, request);

            if (result.Failure)
            {
                return GenericResult.FromError<T>(result.Message);
            }

            return GenericResult.FromSuccess(JsonConvert.DeserializeObject<T>(result.Value, jsonSerializerSettings));
        }

        /// <summary>
        /// Process the request
        /// </summary>
        public GenericResult<string> ProcessRequest(string action, JetStoreEntity store, IHttpRequestSubmitter request)
        {
            AuthenticateRequest(store, request);

            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.Jet, action);
            apiLogEntry.LogRequest(request);
            try
            {
                IHttpResponseReader httpResponseReader = request.GetResponse();
                string result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");

                return GenericResult.FromSuccess(result);
            }
            catch (Exception ex)
            {
                apiLogEntry.LogResponse(ex);
                return GenericResult.FromError<string>(ex.Message);
            }
        }

        /// <summary>
        /// Authenticate the request using the stores token
        /// </summary>
        private void AuthenticateRequest(JetStoreEntity store, IHttpRequestSubmitter request)
        {
            try
            {
                string password = encryptionProviderFactory.CreateSecureTextEncryptionProvider(store.ApiUser)
                    .Decrypt(store.Secret);

                GenericResult<string> token = GetToken(store.ApiUser, password);

                if (token.Failure)
                {
                    throw new JetException($"Failed to get token: {token.Message}");
                }

                request.Headers.Add("Authorization", $"bearer {token.Value}");
            }
            catch (EncryptionException ex)
            {
                throw new JetException("Failed to decrypt the shared secret", ex);
            }
        }
    }
}