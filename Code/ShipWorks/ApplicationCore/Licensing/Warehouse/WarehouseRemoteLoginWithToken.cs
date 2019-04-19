using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Licensing.TangoRequests;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;
using Newtonsoft.Json;
using RestSharp;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Login to the warehouse
    /// </summary>
    [Component]
    public class WarehouseRemoteLoginWithToken : IWarehouseRemoteLoginWithToken
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WarehouseRemoteLoginWithToken));
        private readonly ITangoGetRedirectToken tangoGetRedirectToken;
        private readonly WebClientEnvironmentFactory webClientEnvironmentFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseRemoteLoginWithToken(ITangoGetRedirectToken tangoGetRedirectToken,
            WebClientEnvironmentFactory webClientEnvironmentFactory)
        {
            this.tangoGetRedirectToken = tangoGetRedirectToken;
            this.webClientEnvironmentFactory = webClientEnvironmentFactory;
        }

        /// <summary>
        /// Get Tango redirect token
        /// </summary>
        public async Task<GenericResult<TokenResponse>> RemoteLoginWithToken()
        {
            WebClientEnvironment  webClientEnvironment = webClientEnvironmentFactory.SelectedEnvironment;
            GenericResult<TokenResponse> redirectToken = tangoGetRedirectToken.GetRedirectToken();

            if (redirectToken.Failure)
            {
                return GenericResult.FromError<TokenResponse>("Could not retrieve redirect token");
            }

            return await RemoteLoginWithToken(redirectToken.Value.redirectToken, webClientEnvironment)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get Tango redirect token
        /// </summary>
        private async Task<GenericResult<TokenResponse>> RemoteLoginWithToken(string redirectToken, WebClientEnvironment webClientEnvironment)
        {
            RestRequest restRequest = new RestRequest(WarehouseEndpoints.Login, Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            restRequest.AddJsonBody(new { redirectToken = redirectToken});

            var restClient = new RestClient(webClientEnvironment.WarehouseUrl);
            
            IRestResponse restResponse = await restClient.ExecuteTaskAsync(restRequest)
                .ConfigureAwait(false);

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
