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
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Client for interacting with ShipEngine
    /// </summary>
    [Component]
    public class ShipEngineWebClient : IShipEngineWebClient
    {
        private readonly IShipEngineApiKey apiKey;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private readonly IShipEngineCarrierAccountsApiFactory carrierAccountsApiFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEngineWebClient(IShipEngineApiKey apiKey,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory,
            IShipEngineCarrierAccountsApiFactory carrierAccountsApiFactory)
        {
            this.apiKey = apiKey;
            this.apiLogEntryFactory = apiLogEntryFactory;
            this.carrierAccountsApiFactory = carrierAccountsApiFactory;
        }

        /// <summary>
        /// Connect the account number to ShipEngine
        /// </summary>
        /// <returns>The CarrierId</returns>
        public async Task<GenericResult<string>> ConnectDhlAccount(string accountNumber)
        {
            string key = await GetApiKey();
            // If for some reason the key is blank show an error because we have to have the key to make the request
            if (string.IsNullOrWhiteSpace(key))
            {
                return GenericResult.FromError<string>("Unable to add your DHL Express account at this time. Please try again later.");
            }

            // First check and see if we already have the account connected
            string accountId = await GetCarrierIdByAccountNumber(accountNumber, key);
            if (!string.IsNullOrWhiteSpace(accountId))
            {
                return GenericResult.FromSuccess(accountId);
            }
            
            DHLExpressAccountInformationDTO dhlAccountInfo = new DHLExpressAccountInformationDTO { AccountNumber = accountNumber, Nickname = accountNumber };

            ICarrierAccountsApi apiInstance = carrierAccountsApiFactory.CreateCarrierAccountsApi();

            ConfigureLogging(apiInstance, ApiLogSource.DHLExpress, "ConnectDHLExpressAccount");
            
            try
            {
                ConnectAccountResponseDTO result = await apiInstance
                    .DHLExpressAccountCarrierConnectAccountAsync(dhlAccountInfo, key).ConfigureAwait(false);
                return GenericResult.FromSuccess(result.CarrierId);
            }
            catch (ApiException ex)
            {
                return GenericResult.FromError<string>(GetErrorMessage(ex));
            }
        }

        /// <summary>
        /// Get the error message from an ApiException
        /// </summary>
        private static string GetErrorMessage(ApiException ex)
        {
            try
            {
                ApiErrorResponseDTO error = JsonConvert.DeserializeObject<ApiErrorResponseDTO>(ex.ErrorContent);
                if (error.Errors.Any())
                {
                    return error.Errors.First().Message;
                }
            }
            catch (JsonReaderException)
            {
                return ex.Message;
            }

            return ex.Message;
        }

        /// <summary>
        /// Get the account if it exists
        /// </summary>
        public async Task<string> GetCarrierIdByAccountNumber(string accountNumber, string key)
        {
            ICarriersApi carrierApi = carrierAccountsApiFactory.CreateCarrierApi();
            ConfigureLogging(carrierApi, ApiLogSource.ShipEngine, $"FindAccount{accountNumber}");
            try
            {
                CarrierListResponse result = await carrierApi.CarriersListAsync(key);
                return result?.Carriers?.FirstOrDefault(c => c.AccountNumber == accountNumber)?.CarrierId ?? string.Empty;
            }
            catch (ApiException ex)
            {
                return GetErrorMessage(ex);
            }
        }

        /// <summary>
        /// Get the api key
        /// </summary>
        private async Task<string> GetApiKey()
        {
            if (string.IsNullOrWhiteSpace(apiKey.Value))
            {
                await apiKey.Configure().ConfigureAwait(false);
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

        public Task<RateShipmentResponse> RateShipment(RateShipmentRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
