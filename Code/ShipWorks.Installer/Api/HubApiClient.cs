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
using ShipWorks.Installer.Models;

namespace ShipWorks.Installer.Api
{
    /// <summary>
    /// Client for communicating with the Hub API
    /// </summary>
    public class HubApiClient : IHubApiClient
    {
        private const string LoginEndpoint = "api/auth/login";
        private const string WarehousesEndpoint = "api/warehouses";
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

            return await MakeRequest<TokenResponse>(restRequest, "Login").ConfigureAwait(false);
        }

        /// <summary>
        /// Get list of warehouses
        /// </summary>
        public async Task<WarehouseList> GetWarehouseList(HubToken token)
        {
            RestRequest restRequest = new RestRequest(WarehousesEndpoint, Method.GET)
            {
                RequestFormat = DataFormat.Json
            };

            restRequest.AddHeader("Authorization", $"Bearer {token.Token}");

            return await MakeRequest<WarehouseList>(restRequest, "Get Warehouses");
        }

        /// <summary>
        /// Log hub calls, redacting sensitive information
        /// </summary>
        private void Log(IRestClient client, IRestRequest request, IRestResponse response, string title)
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
            if (jsonRequest.SelectToken("parameters[0].value.password") != null)
            {
                jsonRequest["parameters"][0]["value"]["password"] = "REDACTED";
            }

            var responseToLog = new
            {
                statusCode = response.StatusCode,
                content = response.Content,
                headers = response.Headers,
                // The Uri that actually responded (could be different from the requestUri if a redirection occurred)
                responseUri = response.ResponseUri,
                errorMessage = response.ErrorMessage,
            };

            log.Info($"{title}:\nRequest: {jsonRequest}\nResponse: {JToken.FromObject(responseToLog)}");
        }

        /// <summary>
        /// Make a request to the Hub
        /// </summary>
        private async Task<T> MakeRequest<T>(IRestRequest request, string logTitle)
        {
            var restClient = new RestClient(webClientEnvironment.WarehouseUrl);

            IRestResponse restResponse = await restClient.ExecuteAsync(request)
                .ConfigureAwait(false);

            Log(restClient, request, restResponse, logTitle);

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
            return JsonConvert.DeserializeObject<T>(restResponse.Content,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
    }
}
