﻿using System;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Makes requests to the warehouse and handles authentication token management
    /// </summary>
    [Component(RegistrationType.Self, SingleInstance = true)]
    public class WarehouseRequestClient
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
        public async Task<GenericResult<IRestResponse>> MakeRequest(IRestRequest restRequest)
        {
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

                restRequest.AddHeader("Authorization", $"Bearer {authenticationToken}");

                IRestClient restClient = new RestClient(webClientEnvironmentFactory.SelectedEnvironment.WarehouseUrl);

                IRestResponse restResponse = await restClient.ExecuteTaskAsync(restRequest).ConfigureAwait(false);

                if (restResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    // Get new token using the refresh token
                    GenericResult<TokenResponse> refreshTokenResult = await warehouseRefreshToken.RefreshToken(refreshToken)
                        .ConfigureAwait(false);

                    if (refreshTokenResult.Failure)
                    {
                        return GenericResult.FromError<IRestResponse>("Unable to obtain a valid token from refreshToken.");
                    }

                    authenticationToken = refreshTokenResult.Value.token;

                    foreach (var param in restRequest.Parameters)
                    {
                        if (param.Name == "Authorization")
                        {
                            param.Value = $"Bearer {authenticationToken}";
                        }
                    }

                    restResponse = await restClient.ExecuteTaskAsync(restRequest).ConfigureAwait(false);
                }

                return GenericResult.FromSuccess(restResponse);
            }
            catch (Exception e)
            {
                return GenericResult.FromError<IRestResponse>(e.Message);
            }
        }
    }
}
