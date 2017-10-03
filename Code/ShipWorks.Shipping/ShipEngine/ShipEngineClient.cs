using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipEngine.ApiClient.Api;
using ShipEngine.ApiClient.Client;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using System;
using System.Linq;
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
        public async Task<GenericResult<string>> ConnectDhlAccount(string accountNumber)
        {
            // First check and see if we already have the account connected
            string accountId = await GetCarrierIdByAccountNumber(accountNumber);
            if (!string.IsNullOrWhiteSpace(accountId))
            {
                return GenericResult.FromSuccess(accountId);
            }
            
            Task<string> key = GetApiKey();

            DHLExpressAccountInformationDTO dhlAccountInfo = new DHLExpressAccountInformationDTO { AccountNumber = accountNumber, Nickname = accountNumber };

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
        /// Get the account if it exists
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public async Task<string> GetCarrierIdByAccountNumber(string accountNumber)
        {
            Task<string> key = GetApiKey();
            ICarriersApi carrierApi = carrierAccountsApiFactory.CreateCarrierApi();

            try
            {
                CarrierListResponse result = await carrierApi.CarriersListAsync(key.Result);
                return result.Carriers.FirstOrDefault(c => c.AccountNumber == accountNumber)?.CarrierId ?? string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
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
