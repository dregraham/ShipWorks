using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipEngine.ApiClient.Api;
using ShipEngine.ApiClient.Client;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using System;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Client for interacting with ShipEngine
    /// </summary>
    [Component]
    public class ShipEngineClient : IShipEngineClient
    {
        private readonly IShipEngineApiKey apiKey;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private readonly IShipEngineCarrierAccountsApiFactory carrierAccountsApiFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEngineClient(IShipEngineApiKey apiKey,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory,
            IShipEngineCarrierAccountsApiFactory carrierAccountsApiFactory)
        {
            this.apiKey = apiKey;
            this.apiLogEntryFactory = apiLogEntryFactory;
            this.carrierAccountsApiFactory = carrierAccountsApiFactory;
        }

        /// <summary>
        /// Connect the accout number to ShipEngine
        /// </summary>
        public async Task<GenericResult<string>> ConnectDHLAccount(string accountNumber)
        {
            Task<string> key = GetApiKey();

            DHLExpressAccountInformationDTO dhlAccountInfo = new DHLExpressAccountInformationDTO() { AccountNumber = accountNumber, Nickname = string.Empty };

            ICarrierAccountsApi apiInstance = carrierAccountsApiFactory.CreateCarrierAccountsApi();

            ConfigureLogging(apiInstance, ApiLogSource.DHLExpress, "ConnectDHLExpressAccount");

            try
            {
                ConnectAccountResponseDTO result = await apiInstance.DHLExpressAccountCarrierConnectAccountAsync(dhlAccountInfo, key.Result).ConfigureAwait(false);

                return GenericResult.FromSuccess(result.CarrierId);
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<string>(ex);
            }
        }

        /// <summary>
        /// Get the api key
        /// </summary>
        private async Task<string> GetApiKey()
        {
            if (string.IsNullOrWhiteSpace(apiKey.Value))
            {
                await Task.Run(() => apiKey.Configure()).ConfigureAwait(false);
            }

            return apiKey.Value;
        }

        /// <summary>
        /// Configure logging for the apiAccessor
        /// </summary>
        /// <param name="apiAccessor">the api accessor to configure</param>
        /// <param name="logSource">the log source</param>
        /// <param name="action">the action being logged</param>
        private void ConfigureLogging(IApiAccessor apiAccessor, ApiLogSource logSource, string action)
        {
            IApiLogEntry apiLogEntry = apiLogEntryFactory(logSource, action);

            apiAccessor.Configuration.ApiClient.RequestLogger = apiLogEntry.LogRequest;
            apiAccessor.Configuration.ApiClient.ResponseLogger = apiLogEntry.LogResponse;
        }
    }
}
