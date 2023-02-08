using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Makes requests to the warehouse and handles authentication token management
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(IWarehouseRequestClient), SingleInstance = true)]
    public class WarehouseRequestClient : IInitializeForCurrentUISession, IWarehouseRequestClient
    {
        private readonly IWarehouseRemoteLoginWithToken warehouseRemoteLoginWithToken;
        private readonly IConfigurationData configurationData;
        private readonly WebClientEnvironmentFactory webClientEnvironmentFactory;
        private string authenticationToken = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseRequestClient(IWarehouseRemoteLoginWithToken warehouseRemoteLoginWithToken,
            IConfigurationData configurationData,
            WebClientEnvironmentFactory webClientEnvironmentFactory)
        {
            this.warehouseRemoteLoginWithToken = warehouseRemoteLoginWithToken;
            this.configurationData = configurationData;
            this.webClientEnvironmentFactory = webClientEnvironmentFactory;
        }

        /// <summary>
        /// Make an authenticated request
        /// </summary>
        public Task<GenericResult<IRestResponse>> MakeRequest(IRestRequest restRequest, string logName)
            => MakeRequest(restRequest, logName, ApiLogSource.ShipWorksWarehouse, CancellationToken.None);

        /// <summary>
        /// Make an authenticated request
        /// </summary>
        public async Task<GenericResult<IRestResponse>> MakeRequest(IRestRequest restRequest, string logName, ApiLogSource apiLogSource, CancellationToken cancellationToken)
        {
            ApiLogEntry logEntry = new ApiLogEntry(apiLogSource, logName);
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
                }

                IRestClient restClient = new RestClient(webClientEnvironmentFactory.SelectedEnvironment.WarehouseUrl);

                logEntry.LogRequest(restRequest, restClient, "json");

                restRequest
                    .AddHeader("Authorization", $"Bearer {authenticationToken}")
                    .AddHeader("warehouse-id", configurationData.FetchReadOnly().WarehouseID);

                restResponse = await restClient.ExecuteTaskAsync(restRequest, cancellationToken).ConfigureAwait(false);
                logEntry.LogResponse(restResponse, "json");

                if (restResponse.StatusCode == HttpStatusCode.OK || restResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    return GenericResult.FromSuccess(restResponse);
                }

                if (restResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    GenericResult<TokenResponse> redirectTokenResult = await warehouseRemoteLoginWithToken.RemoteLoginWithToken()
                        .ConfigureAwait(false);

                    if (redirectTokenResult.Failure)
                    {
                        return GenericResult.FromError<IRestResponse>("Unable to obtain a valid token from redirectToken.");
                    }

                    restResponse = await ResendAction(restRequest, restClient, redirectTokenResult, cancellationToken);
                }

                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    return GenericResult.FromSuccess(restResponse);
                }

                return GenericResult.FromError<IRestResponse>(HubApiException.FromResponse(restResponse));
            }
            catch (Exception e)
            {
                if (restResponse != null)
                {
                    logEntry.LogResponse(restResponse, "json");
                }

                return GenericResult.FromError<IRestResponse>(e.Message);
            }
        }

        /// <summary>
        /// Make an authenticated request
        /// </summary>
        public Task<T> MakeRequest<T>(IRestRequest restRequest, string logName) =>
            MakeRequest<T>(restRequest, logName, CancellationToken.None);

        /// <summary>
        /// Get the WarehouseUrl
        /// </summary>
        public string WarehouseUrl => webClientEnvironmentFactory.SelectedEnvironment.WarehouseUrl;

        /// <summary>
        /// Make an authenticated request
        /// </summary>
        public async Task<T> MakeRequest<T>(IRestRequest restRequest, string logName, CancellationToken cancellationToken)
        {
            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.ShipWorksWarehouse, logName);

            if (authenticationToken.IsNullOrWhiteSpace())
            {
                // Get new token
                GenericResult<TokenResponse> redirectTokenResult = await warehouseRemoteLoginWithToken.RemoteLoginWithToken()
                    .ConfigureAwait(false);

                if (redirectTokenResult.Failure)
                {
                    throw new TangoException("Unable to obtain a valid token to authenticate request.");
                }

                authenticationToken = redirectTokenResult.Value.token;
            }

            IRestClient restClient = new RestClient(webClientEnvironmentFactory.SelectedEnvironment.WarehouseUrl);

            logEntry.LogRequest(restRequest, restClient, "json");

            restRequest
                .AddHeader("Authorization", $"Bearer {authenticationToken}")
                .AddHeader("warehouse-id", configurationData.FetchReadOnly().WarehouseID);

            var restResponse = await restClient.ExecuteTaskAsync<T>(restRequest, cancellationToken).ConfigureAwait(false);
            
            try
            {
                logEntry.LogResponse(restResponse, "json");
                
                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    if (restResponse.Data != null)
                    {
                        return restResponse.Data;
                    }

                    return JsonConvert.DeserializeObject<T>(restResponse.Content);
                }

                if (restResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    GenericResult<TokenResponse> redirectTokenResult = await warehouseRemoteLoginWithToken.RemoteLoginWithToken()
                        .ConfigureAwait(false);

                    if (redirectTokenResult.Failure)
                    {
                        throw new TangoException("Unable to obtain a valid token from redirectToken.");
                    }

                    restResponse = await ResendAction<T>(restRequest, restClient, redirectTokenResult);
                }

                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    return restResponse.Data;
                }

                throw HubApiException.FromResponse(restResponse);
            }
            catch (Exception)
            {
                if (restResponse != null)
                {
                    logEntry.LogResponse(restResponse, "json");
                }

                throw;
            }
        }

        /// <summary>
        /// Resend the action after getting a new auth token
        /// </summary>
        private async Task<IRestResponse> ResendAction(
            IRestRequest restRequest,
            IRestClient restClient,
            GenericResult<TokenResponse> refreshTokenResult,
            CancellationToken cancellationToken)
        {
            authenticationToken = refreshTokenResult.Value.token;

            foreach (var param in restRequest.Parameters)
            {
                if (param.Name == "Authorization")
                {
                    param.Value = $"Bearer {authenticationToken}";
                }
            }

            return await restClient.ExecuteTaskAsync(restRequest, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Resend the action after getting a new auth token
        /// </summary>
        private async Task<IRestResponse<T>> ResendAction<T>(
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

            return await restClient.ExecuteTaskAsync<T>(restRequest).ConfigureAwait(false);
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
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() => EndSession();
    }
}
