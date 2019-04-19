using System;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Licensing.TangoRequests;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Net;
using ShipWorks.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private readonly WebClientEnvironment webClientEnvironment;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseRemoteLoginWithToken(
            IHttpRequestSubmitterFactory requestSubmitterFactory,
            ITangoWebRequestClient webRequestClient,
            ITangoGetRedirectToken tangoGetRedirectToken,
            IJsonRequest jsonRequest,
            WebClientEnvironmentFactory webClientEnvironmentFactory)
        {
            this.tangoGetRedirectToken = tangoGetRedirectToken;
            webClientEnvironment = webClientEnvironmentFactory.SelectedEnvironment;
        }

        /// <summary>
        /// Get Tango redirect token
        /// </summary>
        public GenericResult<TokenResponse> RemoteLoginWithToken()
        {
            GenericResult<TokenResponse> redirectToken = tangoGetRedirectToken.GetRedirectToken();

            if (redirectToken.Failure)
            {
                return GenericResult.FromError<TokenResponse>("Could not retrieve redirect token");
            }

            return RemoteLoginWithToken(redirectToken.Value.redirectToken);
        }

        /// <summary>
        /// Get Tango redirect token
        /// </summary>
        private GenericResult<TokenResponse> RemoteLoginWithToken(string redirectToken)
        {
            RestRequest restRequest = new RestRequest("api/auth/token/login", Method.POST);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddJsonBody(new { redirectToken = redirectToken});

            var restClient = new RestClient(webClientEnvironment.WarehouseUrl)
            {
                //Authenticator = authenticatorFactory.Create(store)
            };

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
