﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipEngine.ApiClient.Api;
using ShipEngine.ApiClient.Client;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Stores;

namespace ShipWorks.ShipEngine
{
    /// <summary>
    /// Client for interacting with ShipEngine
    /// </summary>
    [Component]
    public class ShipEngineWebClient : IShipEngineWebClient, IShipEngineResourceDownloader
    {
        private readonly IShipEngineApiKey apiKey;
        private readonly ILogEntryFactory apiLogEntryFactory;
        private readonly IShipEngineApiFactory shipEngineApiFactory;
        private readonly IStoreManager storeManager;
        private readonly IInterapptiveOnly interapptiveOnly;

        private const string liveRegKey = "ShipEngineLive";
        private const string defaultEndpointBase = "https://api.shipengine.com/v1";
        private const string proxyEndpoint = "https://proxy.hub.shipworks.com/shipEngine";
        private const string addStoreResource = "connections/order_sources";

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEngineWebClient(IShipEngineApiKey apiKey,
            ILogEntryFactory apiLogEntryFactory,
            IShipEngineApiFactory shipEngineApiFactory,
            IStoreManager storeManager,
            IInterapptiveOnly interapptiveOnly)
        {
            this.apiKey = apiKey;
            this.apiLogEntryFactory = apiLogEntryFactory;
            this.shipEngineApiFactory = shipEngineApiFactory;
            this.storeManager = storeManager;
            this.interapptiveOnly = interapptiveOnly;
        }

        /// <summary>
        /// Get the base endpoint for ShipEngine requests
        /// </summary>
        private string GetEndpointBase()
        {
            if (interapptiveOnly.UseFakeAPI(liveRegKey))
            {
                var endpointOverride = interapptiveOnly.Registry.GetValue("ShipEngineEndpoint", string.Empty);
                if (!string.IsNullOrWhiteSpace(endpointOverride))
                {
                    return endpointOverride.TrimEnd('/');
                }
            }

            return defaultEndpointBase;
        }

        /// <summary>
        /// Connect the account number to ShipEngine
        /// </summary>
        /// <returns>The CarrierId</returns>
        public async Task<GenericResult<string>> ConnectDhlAccount(string accountNumber)
        {
            // Check to see if the account already exists in ShipEngine
            GenericResult<string> existingAccount = await GetCarrierId(accountNumber);

            if (existingAccount.Success)
            {
                return existingAccount;
            }

            DHLExpressAccountInformationDTO dhlAccountInfo = new DHLExpressAccountInformationDTO { AccountNumber = accountNumber, Nickname = accountNumber };

            ICarrierAccountsApi apiInstance = shipEngineApiFactory.CreateCarrierAccountsApi();

            try
            {
                var apiKey = await GetApiKeyAsync().ConfigureAwait(false);
                return await ConnectCarrierAccount(apiInstance, ApiLogSource.DHLExpress, "ConnectDHLExpressAccount",
                    apiInstance.DHLExpressAccountCarrierConnectAccountAsync(dhlAccountInfo, apiKey)).ConfigureAwait(false);
            }
            catch (ApiException ex)
            {
                return GenericResult.FromError<string>(GetErrorMessage(ex));
            }
        }

        /// <summary>
        /// Disconnect AmazonShipping
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<Result> DisconnectAmazonShippingAccount(string accountId)
        {
            try
            {
                var apiKey = await GetApiKeyAsync().ConfigureAwait(false);

                HttpJsonVariableRequestSubmitter submitter = new HttpJsonVariableRequestSubmitter();
                submitter.Headers.Add($"Content-Type: application/json");
                submitter.Headers.Add($"api-key: {apiKey}");
                submitter.Verb = HttpVerb.Delete;
                submitter.Uri = new Uri($"{GetEndpointBase()}/connections/carriers/amazon_shipping_us/{accountId}");

                // Delete request returns no content, this is not an error
                submitter.AllowHttpStatusCodes(HttpStatusCode.NoContent);

                submitter.ProcessRequest(new ApiLogEntry(ApiLogSource.ShipEngine, "DisconnectAmazonShipping"), typeof(ShipEngineException));
                return Result.FromSuccess();
            }
            catch (Exception ex)
            {
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Update an Amazon Accounts info
        /// </summary>
        public async Task<Result> UpdateAmazonAccount(IAmazonSWAAccountEntity amazonSwaAccount)
        {
            AmazonStoreEntity store = storeManager.GetEnabledStores()
                .FirstOrDefault(s => s.StoreTypeCode == StoreTypeCode.Amazon) as AmazonStoreEntity;

            // only update the info if an amazon store exists
            if (store == null)
            {
                return Result.FromSuccess();
            }

            try
            {
                // first we have to get the account id
                string accountId = await GetAccountIDAsync().ConfigureAwait(false);

                ICarrierAccountsApi apiInstance = shipEngineApiFactory.CreateCarrierAccountsApi();

                AmazonShippingUsAccountSettingsDTO updateRequest = new AmazonShippingUsAccountSettingsDTO(
                    email: amazonSwaAccount.Email,
                    merchantSellerId: store.MerchantID,
                    mwsAuthToken: store.AuthToken);

                await apiInstance.AmazonShippingUsAccountCarrierUpdateSettingsAsync(amazonSwaAccount.ShipEngineCarrierId, updateRequest, apiKey.GetPartnerApiKey(), $"se-{accountId}");
                return Result.FromSuccess();
            }
            catch (ApiException ex)
            {
                string error = GetErrorMessage(ex);
                return Result.FromError(error);
            }
        }

        /// <summary>
        /// Connect an Amazon Shipping Account
        /// </summary>
        /// <remarks>
        /// unlike the other methods in this class we are manually interacting
        /// with the ShipEngine API because they have not added connecting to
        /// Amazon to their DLL yet
        /// </remarks>
        public async Task<GenericResult<string>> ConnectAmazonShippingAccount(string authCode)
        {
            try
            {
                // first we have to get the account id
                string accountId = await GetAccountIDAsync().ConfigureAwait(false);

                AmazonStoreEntity store = storeManager.GetEnabledStores()
                    .FirstOrDefault(s => s.StoreTypeCode == StoreTypeCode.Amazon) as AmazonStoreEntity;

                AmazonShippingUsAccountInformationRequest amazonAccountInfo = new AmazonShippingUsAccountInformationRequest()
                {
                    Nickname = authCode,
                    AuthCode = authCode,
                    MerchantSellerId = store?.MerchantID ?? string.Empty,
                    MwsAuthToken = store?.AuthToken ?? string.Empty,
                    Email = store?.Email ?? string.Empty
                };

                ICarrierAccountsApi apiInstance = shipEngineApiFactory.CreateCarrierAccountsApi();

                return await ConnectCarrierAccount(apiInstance, ApiLogSource.AmazonSWA, "ConnectAmazonShippingAccount",
                apiInstance.AmazonShippingUsAccountCarrierConnectAccountAsync(amazonAccountInfo, apiKey.GetPartnerApiKey(), $"se-{accountId}"));
            }
            catch (ApiException ex)
            {
                string error = GetErrorMessage(ex);

                // If the account has already been connected, get the carrier ID and return it
                if (error.Contains("already been connected", StringComparison.OrdinalIgnoreCase))
                {
                    return await GetAmazonShippingCarrierID();
                }

                return GenericResult.FromError<string>(error);
            }
        }

        /// <summary>
        /// Connect an Asendia account
        /// </summary>
        public async Task<GenericResult<string>> ConnectAsendiaAccount(string accountNumber, string username, string password)
        {
            // Check to see if the account already exists in ShipEngine
            GenericResult<string> existingAccount = await GetCarrierId(accountNumber);

            if (existingAccount.Success)
            {
                return existingAccount;
            }

            AsendiaAccountInformationDTO ascendiaAccountInfo = new AsendiaAccountInformationDTO
            {
                AccountNumber = accountNumber,
                Nickname = accountNumber,
                FtpUsername = username,
                FtpPassword = password
            };

            ICarrierAccountsApi apiInstance = shipEngineApiFactory.CreateCarrierAccountsApi();

            try
            {
                var apiKey = await GetApiKeyAsync().ConfigureAwait(false);
                return await ConnectCarrierAccount(apiInstance, ApiLogSource.Asendia, "ConnectAsendiaAccount",
                apiInstance.AsendiaAccountCarrierConnectAccountAsync(ascendiaAccountInfo, apiKey)).ConfigureAwait(false);
            }
            catch (ApiException ex)
            {
                string error = GetErrorMessage(ex);

                // Asendia returns a cryptic error when the username or password are wrong, clean it up
                if (error.Contains("(530) Not logged in"))
                {
                    return GenericResult.FromError<string>("Unable to connect to Asendia. Please check your account information and try again.");
                }

                return GenericResult.FromError<string>(error);
            }
        }

        /// <summary>
        /// Get the CarrierId for the given account number
        /// </summary>
        private async Task<GenericResult<string>> GetCarrierId(string accountNumber)
        {
            string key = await GetApiKeyAsync().ConfigureAwait(false);
            // If for some reason the key is blank show an error because we have to have the key to make the request
            if (string.IsNullOrWhiteSpace(key))
            {
                return GenericResult.FromError<string>("Unable to add your account at this time. Please try again later.");
            }

            // First check and see if we already have the account connected
            string accountId = await GetCarrierIdByAccountNumber(accountNumber, key);
            if (!string.IsNullOrWhiteSpace(accountId))
            {
                return GenericResult.FromSuccess(accountId);
            }

            return GenericResult.FromError<string>("Unable to find account");
        }

        /// <summary>
        /// Connect to a ShipEngine Carrier Account
        /// </summary>
        /// <param name="apiInstance">Api instance to use for logging</param>
        /// <param name="logSource">Log Source to use(folder where its logged)</param>
        /// <param name="action">The name of the log file</param>
        /// <param name="connect">The task to run to connect</param>
        /// <returns>The carrier Id from ShipEngine</returns>
        public async Task<GenericResult<string>> ConnectCarrierAccount(ICarrierAccountsApi apiInstance, ApiLogSource logSource, string action, Task<ConnectAccountResponseDTO> connect)
        {
            ConfigureLogging(apiInstance, logSource, action, LogActionType.Other);
            ConnectAccountResponseDTO result = await connect.ConfigureAwait(false);

            return GenericResult.FromSuccess(result.CarrierId);
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
            catch (ApiException)
            {
                return null;
            }
        }

        /// <summary>
        /// Purchases a label from ShipEngine using the given rateid
        /// </summary>
        public async Task<Label> PurchaseLabelWithRate(string rateId, PurchaseLabelWithoutShipmentRequest request, ApiLogSource apiLogSource)
        {
            ILabelsApi labelsApi = shipEngineApiFactory.CreateLabelsApi();
            ConfigureLogging(labelsApi, apiLogSource, "PurchaseLabel", LogActionType.Other);
            try
            {
                var apiKey = await GetApiKeyAsync().ConfigureAwait(false);
                return await labelsApi.LabelsPurchaseLabelWithRateAsync(rateId, request, apiKey).ConfigureAwait(false);
            }
            catch (ApiException ex)
            {
                throw new ShipEngineException(GetErrorMessage(ex));
            }
        }

        /// <summary>
        /// Purchases a label from ShipEngine using the given request
        /// </summary>
        public async Task<Label> PurchaseLabel(PurchaseLabelRequest request, ApiLogSource apiLogSource, TelemetricResult<IDownloadedLabelData> telemetricResult)
        {
            ILabelsApi labelsApi = shipEngineApiFactory.CreateLabelsApi();
            ConfigureLogging(labelsApi, apiLogSource, "PurchaseLabel", LogActionType.Other);
            try
            {
                string localApiKey = await GetApiKeyAsync().ConfigureAwait(false);

                Label label = await telemetricResult.RunTimedEventAsync(TelemetricEventType.GetLabel,
                        () => labelsApi.LabelsPurchaseLabelAsync(request, localApiKey)
                        )
                    .ConfigureAwait(false);

                return label;
            }
            catch (ApiException ex)
            {
                throw new ShipEngineException(GetErrorMessage(ex));
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
                var apiKey = await GetApiKeyAsync().ConfigureAwait(false);
                return await ratesApi.RatesRateShipmentAsync(request, apiKey);
            }
            catch (ApiException ex)
            {
                throw new ShipEngineException(GetErrorMessage(ex));
            }
        }

        /// <summary>
        /// Void a ShipEngine Label
        /// </summary>
        public async Task<VoidLabelResponse> VoidLabel(string labelId, ApiLogSource apiLogSource)
        {
            ILabelsApi labelsApi = shipEngineApiFactory.CreateLabelsApi();
            ConfigureLogging(labelsApi, apiLogSource, "VoidShipment", LogActionType.Other);

            try
            {
                var apiKey = await GetApiKeyAsync().ConfigureAwait(false);
                return await labelsApi.LabelsVoidLabelAsync(labelId, apiKey);
            }
            catch (ApiException ex)
            {
                throw new ShipEngineException(GetErrorMessage(ex));
            }
        }

        /// <summary>
        /// Track a shipment using the label ID
        /// </summary>
        public async Task<TrackingInformation> Track(string labelId, ApiLogSource apiLogSource)
        {
            ILabelsApi labelsApi = shipEngineApiFactory.CreateLabelsApi();
            ConfigureLogging(labelsApi, apiLogSource, "Track", LogActionType.GetRates);
            try
            {
                var apiKey = await GetApiKeyAsync().ConfigureAwait(false);
                return await labelsApi.LabelsTrackAsync(labelId, apiKey);
            }
            catch (ApiException ex)
            {
                throw new ShipEngineException(GetErrorMessage(ex));
            }
        }

        /// <summary>
        /// Get the api key
        /// </summary>
        private async Task<string> GetApiKeyAsync()
        {
            if (string.IsNullOrWhiteSpace(apiKey.Value))
            {
                await apiKey.ConfigureAsync().ConfigureAwait(false);
            }

            return apiKey.Value;
        }

        /// <summary>
        /// Get the api key
        /// </summary>
        private string GetApiKey()
        {
            if (string.IsNullOrWhiteSpace(apiKey.Value))
            {
                apiKey.Configure();
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

        /// <summary>
        /// Get the error message from an ApiException
        /// </summary>
        private static string GetErrorMessage(ApiException ex)
        {
            try
            {
                ApiErrorResponseDTO error = JsonConvert.DeserializeObject<ApiErrorResponseDTO>(ex.ErrorContent);
                if (error?.Errors?.Any() ?? false)
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
        /// Download the resource at the given uri
        /// </summary>
        public byte[] Download(Uri uri)
        {
            try
            {
                return WebRequest.Create(uri).GetResponse().GetResponseStream().ToArray();
            }
            catch (Exception ex)
            {
                throw new ShipEngineException($"An error occured while attempting to download reasource.", ex);
            }
        }

        /// <summary>
        /// Get an account ID from a WhoAmI request
        /// </summary>
        public async Task<string> GetAccountIDAsync()
        {
            var apiKey = await GetApiKeyAsync().ConfigureAwait(false);
            HttpJsonVariableRequestSubmitter whoAmIRequest = new HttpJsonVariableRequestSubmitter();
            whoAmIRequest.Headers.Add($"Content-Type: application/json");
            whoAmIRequest.Headers.Add($"api-key: {apiKey}");
            whoAmIRequest.Verb = HttpVerb.Get;
            whoAmIRequest.Uri = new Uri($"{GetEndpointBase()}/environment/whoami");

            EnumResult<HttpStatusCode> result =
                whoAmIRequest.ProcessRequest(new ApiLogEntry(ApiLogSource.ShipEngine, "WhoAmI"), typeof(ShipEngineException));
            return JObject.Parse(result.Message)["data"]["username"].ToString();
        }

        /// <summary>
        /// Get an account ID from a WhoAmI request
        /// </summary>
        public string GetAccountID()
        {
            HttpJsonVariableRequestSubmitter whoAmIRequest = new HttpJsonVariableRequestSubmitter();
            whoAmIRequest.Headers.Add($"Content-Type: application/json");
            whoAmIRequest.Headers.Add($"api-key: {GetApiKey()}");
            whoAmIRequest.Verb = HttpVerb.Get;
            whoAmIRequest.Uri = new Uri($"{GetEndpointBase()}/environment/whoami");

            EnumResult<HttpStatusCode> result =
                whoAmIRequest.ProcessRequest(new ApiLogEntry(ApiLogSource.ShipEngine, "WhoAmI"), typeof(ShipEngineException));
            return JObject.Parse(result.Message)["data"]["username"].ToString();
        }

        /// <summary>
        /// Get the Amazon Shipping carrier ID. There can only ever be one connected per api key.
        /// </summary>
        private async Task<GenericResult<string>> GetAmazonShippingCarrierID()
        {
            string key = await GetApiKeyAsync().ConfigureAwait(false);
            ICarriersApi carrierApi = shipEngineApiFactory.CreateCarrierApi();
            ConfigureLogging(carrierApi, ApiLogSource.ShipEngine, $"ListCarriers", LogActionType.Other);

            try
            {
                CarrierListResponse result = await carrierApi.CarriersListAsync(key);
                return result?.Carriers?.FirstOrDefault(c => c.CarrierCode.Equals("amazon_shipping_us", StringComparison.OrdinalIgnoreCase))?.CarrierId ?? string.Empty;
            }
            catch (ApiException ex)
            {
                string error = GetErrorMessage(ex);

                return GenericResult.FromError<string>(error);
            }
        }

        /// <summary>
        /// Add a new store to ShipEngine
        /// </summary>
        public Guid? AddStore(ApiOrderSourceAccountInformationRequest accountInfo, string storeResource)
        {
            HttpJsonVariableRequestSubmitter addStoreRequest = new HttpJsonVariableRequestSubmitter();
            addStoreRequest.Headers.Add($"api-key: {apiKey.GetPartnerApiKey()}");
            addStoreRequest.Headers.Add($"On-Behalf-Of: se-{GetAccountID()}");
            SetupRequestProxy(addStoreRequest, storeResource);
            addStoreRequest.Headers.Add($"Content-Type: application/json");
            addStoreRequest.Verb = HttpVerb.Post;
            addStoreRequest.RequestBody = JsonConvert.SerializeObject(accountInfo);

            EnumResult<HttpStatusCode> result =
                addStoreRequest.ProcessRequest(new ApiLogEntry(ApiLogSource.ShipEngine, "AddStore"), typeof(ShipEngineException));
            string orderSourceId = JObject.Parse(result.Message)["order_source_id"].ToString();
            return Guid.Parse(orderSourceId);
        }

        /// <summary>
        /// Set URI and headers for proxy if not using fake stores
        /// </summary>
        private void SetupRequestProxy(IHttpRequestSubmitter request, string storeResource)
        {
            var actualEndpoint = GetEndpointBase();

            // We're not using fake stores, so send the request through the proxy
            if (actualEndpoint == defaultEndpointBase)
            {
                // Add "SW-" to each header to prevent them from being stripped out by AWS
                foreach (var key in request.Headers.AllKeys.ToList())
                {
                    request.Headers.Add($"SW-{key}", request.Headers[key]);
                    request.Headers.Remove(key);
                }

                request.Headers.Add($"SW-originalRequestResource: {addStoreResource}/{storeResource}");

                request.Uri = new Uri(proxyEndpoint);
            }
            else
            {
                request.Uri = new Uri($"{actualEndpoint}/{addStoreResource}/{storeResource}");
            }
        }

        /// <summary>
        /// Update a stores credentials in ShipEngine
        /// </summary>
        public Guid? UpdateStoreCredentials(ApiOrderSourceAccountInformationRequest accountInfo, Guid? orderSourceId, string storeEndpoint)
        {
            try
            {
                var orderSourceApi = shipEngineApiFactory.CreateOrderSourceApi();

                //ShipEngine doesn't expose a way to update store credentials so we
                //have to delete the store and add a new one
                orderSourceApi.ApiOrderSourceAccountDeactivate(orderSourceId, apiKey.GetPartnerApiKey(), $"se-{GetAccountID()}");
                return AddStore(accountInfo, storeEndpoint);
            }
            catch (ApiException ex)
            {
                throw new ShipEngineException(GetErrorMessage(ex));
            }
        }

        /// <summary>
        /// Remove a store from ShipEngine
        /// </summary>
        public void DeleteStore(Guid? orderSourceId)
        {
            try
            {
                var orderSourceApi = shipEngineApiFactory.CreateOrderSourceApi();

                orderSourceApi.ApiOrderSourceAccountDeactivate(orderSourceId, apiKey.GetPartnerApiKey(), $"se-{GetAccountID()}");
            }
            catch (ApiException ex)
            {
                throw new ShipEngineException(GetErrorMessage(ex));
            }
        }
    }
}
