using System;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Makes requests to the warehouse and handles authentication token management
    /// </summary>
    [Component(RegistrationType.Self, SingleInstance = true)]
    public class WarehouseRequestClient : IInitializeForCurrentUISession
    {
        private readonly IWarehouseRemoteLoginWithToken warehouseRemoteLoginWithToken;
        private readonly IWarehouseRefreshToken warehouseRefreshToken;
        private readonly WebClientEnvironmentFactory webClientEnvironmentFactory;
        private string authenticationToken = string.Empty;
        private string refreshToken = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseRequestClient(IWarehouseRemoteLoginWithToken warehouseRemoteLoginWithToken,
            IWarehouseRefreshToken warehouseRefreshToken,
            WebClientEnvironmentFactory webClientEnvironmentFactory)
        {
            this.warehouseRemoteLoginWithToken = warehouseRemoteLoginWithToken;
            this.webClientEnvironmentFactory = webClientEnvironmentFactory;
            this.warehouseRefreshToken = warehouseRefreshToken;
        }

        /// <summary>
        /// Make an authenticated request
        /// </summary>
        public async Task<GenericResult<IRestResponse>> MakeRequest(IRestRequest restRequest, string logName)
        {
            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.ShipWorksWarehouse, logName);
            IRestResponse restResponse = null;

            try
            {
                if (authenticationToken.IsNullOrWhiteSpace())
                {
                    // Get new token
                    GenericResult<TokenResponse> redirectTokenResult = await warehouseRemoteLoginWithToken.RemoteLoginWithToken()
                        .ConfigureAwait(false);

                    if (redirectTokenResult.Failure)
                    {
                        return GenericResult.FromError<IRestResponse>("Unable to obtain a valid token to authenticate request.");
                    }

                    authenticationToken = redirectTokenResult.Value.token;
                    refreshToken = redirectTokenResult.Value.refreshToken;
                }

                logEntry.LogRequest(restRequest);

                restRequest.AddHeader("Authorization", $"Bearer {authenticationToken}");

                IRestClient restClient = new RestClient(webClientEnvironmentFactory.SelectedEnvironment.WarehouseUrl);

                restResponse = await restClient.ExecuteTaskAsync(restRequest).ConfigureAwait(false);
                logEntry.LogResponse(restResponse);

                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    return GenericResult.FromSuccess(restResponse);
                }

                if (restResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    // Get new token using the refresh token
                    GenericResult<TokenResponse> refreshTokenResult = await warehouseRefreshToken.RefreshToken(refreshToken)
                        .ConfigureAwait(false);

                    if (refreshTokenResult.Failure)
                    {
                        return GenericResult.FromError<IRestResponse>("Unable to obtain a valid token from refreshToken.");
                    }

                    restResponse = await ResendAction(restRequest, restClient, refreshTokenResult);
                }

                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    return GenericResult.FromSuccess(restResponse);
                }

                return GenericResult.FromError<IRestResponse>($"Unable to make warehouse request. StatusCode: {restResponse.StatusCode}");
            }
            catch (Exception e)
            {
                if (restResponse != null)
                {
                    logEntry.LogResponse(restResponse);
                }

                return GenericResult.FromError<IRestResponse>(e.Message);
            }
        }

        /// <summary>
        /// Resend the action after getting a new auth token
        /// </summary>
        private async Task<IRestResponse> ResendAction(
            IRestRequest restRequest,
            IRestClient restClient,
            GenericResult<TokenResponse> refreshTokenResult)
        {
            authenticationToken = refreshTokenResult.Value.token;

            foreach (var param in restRequest.Parameters)
            {
                if (param.Name == "Authorization")
                {
                    param.Value = $"Bearer {authenticationToken}";
                }
            }

            return await restClient.ExecuteTaskAsync(restRequest).ConfigureAwait(false);
        }

        /// <summary>
        /// Initialize for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            EndSession();
        }

        /// <summary>
        /// End the current session
        /// </summary>
        public void EndSession()
        {
            authenticationToken = string.Empty;
            refreshToken = string.Empty;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() => EndSession();
    }
}
