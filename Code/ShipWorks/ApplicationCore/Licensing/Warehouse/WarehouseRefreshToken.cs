using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;
using Newtonsoft.Json;
using RestSharp;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Refresh our token from the warehouse
    /// </summary>
    [Component]
    public class WarehouseRefreshToken : IWarehouseRefreshToken
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WarehouseRefreshToken));
        private readonly WebClientEnvironmentFactory webClientEnvironmentFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseRefreshToken(WebClientEnvironmentFactory webClientEnvironmentFactory)
        {
            this.webClientEnvironmentFactory = webClientEnvironmentFactory;
        }

        /// <summary>
        /// Get a new token
        /// </summary>
        public async Task<GenericResult<TokenResponse>> RefreshToken(string refreshToken)
        {
            WebClientEnvironment  webClientEnvironment = webClientEnvironmentFactory.SelectedEnvironment;
            IRestRequest restRequest = new RestRequest(WarehouseEndpoints.RefreshToken, Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            restRequest.AddJsonBody(new { refreshToken = refreshToken });

            var restClient = new RestClient(webClientEnvironment.WarehouseUrl);

            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.ShipWorksWarehouse, "RefreshToken");
            logEntry.LogRequest(restRequest);

            IRestResponse restResponse = await restClient.ExecuteTaskAsync(restRequest)
                .ConfigureAwait(false);

            logEntry.LogResponse(restResponse);

            // De-serialize the result
            TokenResponse requestResult = JsonConvert.DeserializeObject<TokenResponse>(restResponse.Content,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore
                });

            return requestResult;
        }
    }
}
