using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipEngine.ApiClient.Api;
using ShipEngine.ApiClient.Client;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Client for interacting with ShipEngine
    /// </summary>
    public class ShipEngineClient
    {
        private ShipEngineApiKey apiKey;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private readonly IShipEngineCarrierAccountsApiFactory carrierAccountsApiFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiKey"></param>
        public ShipEngineClient(ShipEngineApiKey apiKey,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory,
            IShipEngineCarrierAccountsApiFactory carrierAccountsApiFactory)
        {
            this.apiKey = apiKey;
            this.apiLogEntryFactory = apiLogEntryFactory;
            this.carrierAccountsApiFactory = carrierAccountsApiFactory;
        }

        /// <summary>
        /// The SE Api Key
        /// </summary>
        public string ApiKey
        {
            get
            {
                if (apiKey.Value == string.Empty)
                {
                    apiKey.Configure();
                }

                return apiKey.Value;
            }
        }

        /// <summary>
        /// Connect the accout number to ShipEngine
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public async Task<GenericResult<string>> ConnectDHLAccount(string accountNumber)
        {
            try
            {
                ICarrierAccountsApi apiInstance = carrierAccountsApiFactory.CreateCarrierAccountsApi();

                ConfigureLogging(apiInstance, ApiLogSource.DHLExpress, "ConnectDHLExpressAccount");

                DHLExpressAccountInformationDTO dhlAccountInfo = new DHLExpressAccountInformationDTO() { AccountNumber = accountNumber, Nickname = string.Empty };

                ConnectAccountResponseDTO result = await apiInstance.DHLExpressAccountCarrierConnectAccountAsync(dhlAccountInfo, ApiKey);

                return GenericResult.FromSuccess(result.CarrierId);
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<string>(ex);
            }
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
