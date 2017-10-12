using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipEngine.ApiClient.Api;
using ShipEngine.ApiClient.Client;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
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
        private readonly ILogEntryFactory apiLogEntryFactory;
        private readonly IShipEngineApiFactory shipEngineApiFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEngineWebClient(IShipEngineApiKey apiKey,
            ILogEntryFactory apiLogEntryFactory,
            IShipEngineApiFactory shipEngineApiFactory)
        {
            this.apiKey = apiKey;
            this.apiLogEntryFactory = apiLogEntryFactory;
            this.shipEngineApiFactory = shipEngineApiFactory;
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

            ICarrierAccountsApi apiInstance = shipEngineApiFactory.CreateCarrierAccountsApi();

            ConfigureLogging(apiInstance, ApiLogSource.DHLExpress, "ConnectDHLExpressAccount", LogActionType.Other);
            
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
            ICarriersApi carrierApi = shipEngineApiFactory.CreateCarrierApi();
            ConfigureLogging(carrierApi, ApiLogSource.ShipEngine, $"FindAccount{accountNumber}", LogActionType.Other);
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
        /// Gets rates from ShipEngine using the given request
        /// </summary>
        /// <param name="request">The rate shipment request</param>
        /// <returns>The rate shipment response</returns>
        public async Task<RateShipmentResponse> RateShipment(RateShipmentRequest request, ApiLogSource apiLogSource)
        {
            IRatesApi ratesApi = shipEngineApiFactory.CreateRatesApi();
            ConfigureLogging(ratesApi, apiLogSource, "RateShipment", LogActionType.GetRates);
            try
            {
                return await ratesApi.RatesRateShipmentAsync(request, await GetApiKey());
            }
            catch (ApiException ex)
            {
                throw new ShipEngineException(GetErrorMessage(ex));
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
        private void ConfigureLogging(IApiAccessor apiAccessor, ApiLogSource logSource, string action, LogActionType logActionType)
        {
            IApiLogEntry apiLogEntry = apiLogEntryFactory.GetLogEntry(logSource, action, logActionType);

            apiAccessor.Configuration.ApiClient.RequestLogger = apiLogEntry.LogRequest;
            apiAccessor.Configuration.ApiClient.ResponseLogger = apiLogEntry.LogResponse;
        }
    }
}
