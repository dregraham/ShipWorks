using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;
using Newtonsoft.Json;
using RestSharp;

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
        public GenericResult<TokenResponse> RefreshToken(string refreshToken)
        {
            WebClientEnvironment  webClientEnvironment = webClientEnvironmentFactory.SelectedEnvironment;
            RestRequest restRequest = new RestRequest(WarehouseEndpoints.RefreshToken, Method.POST);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddJsonBody(new { refreshToken = refreshToken });

            var restClient = new RestClient(webClientEnvironment.WarehouseUrl);

            IRestResponse restResponse = restClient.Execute(restRequest);

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
