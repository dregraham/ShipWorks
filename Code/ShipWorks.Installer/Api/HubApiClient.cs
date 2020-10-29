using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using ShipWorks.Installer.Api.DTO;
using ShipWorks.Installer.Environments;

namespace ShipWorks.Installer.Api
{
    /// <summary>
    /// Client for communicating with the Hub API
    /// </summary>
    public class HubApiClient : IHubApiClient
    {
        private const string LoginEndpoint = "api/auth/login";
        private readonly WebClientEnvironment webClientEnvironment;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubApiClient(Func<Type, ILog> logFactory, IWebClientEnvironmentFactory webClientEnvironmentFactory)
        {
            log = logFactory(typeof(HubApiClient));
            webClientEnvironment = webClientEnvironmentFactory.SelectedEnvironment;
        }

        /// <summary>
        /// Login to Hub with a username and password
        /// </summary>
        public async Task<TokenResponse> Login(string username, string password)
        {
            IRestRequest restRequest = new RestRequest(LoginEndpoint, Method.POST)
            {
                RequestFormat = DataFormat.Json
            };

            restRequest.AddJsonBody(new { username, password });

            var restClient = new RestClient(webClientEnvironment.WarehouseUrl);

            IRestResponse restResponse = await restClient.ExecuteAsync(restRequest)
                .ConfigureAwait(false);

            LogLoginCalls(restClient, restRequest, restResponse);

            if (!restResponse.IsSuccessful)
            {
                if (restResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Incorrect username or password entered. Please try again.");
                }

                if (restResponse.ErrorException != null)
                {
                    throw restResponse.ErrorException;
                }

                throw new WebException("An unknown error occurred");
            }

            // De-serialize the result
            TokenResponse requestResult = JsonConvert.DeserializeObject<TokenResponse>(restResponse.Content,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore
                });

            return requestResult;
        }

        /// <summary>
        /// Log the login call, redacting sensitive information
        /// </summary>
        private void LogLoginCalls(IRestClient client, IRestRequest request, IRestResponse response)
        {
            var requestToLog = new
            {
                resource = request.Resource,
                // Parameters are custom anonymous objects in order to have the parameter type as a nice string
                // otherwise it will just show the enum value
                parameters = request.Parameters.Select(parameter => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                }),
                // ToString() here to have the method as a nice string otherwise it will just show the enum value
                method = request.Method.ToString(),
                // This will generate the actual Uri used in the request
                uri = client.BuildUri(request),
            };

            var jsonRequest = JToken.FromObject(requestToLog);
            jsonRequest["parameters"][0]["value"]["password"] = "REDACTED";

            var responseToLog = new
            {
                statusCode = response.StatusCode,
                content = response.Content,
                headers = response.Headers,
                // The Uri that actually responded (could be different from the requestUri if a redirection occurred)
                responseUri = response.ResponseUri,
                errorMessage = response.ErrorMessage,
            };

            log.Info($"Login Call:\nRequest: {jsonRequest}\nResponse: {JsonConvert.SerializeObject(responseToLog)}");
        }
    }
}
