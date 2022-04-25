﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using ShipEngine.CarrierApi.Client;
using ShipEngine.CarrierApi.Client.Api;
using ShipEngine.CarrierApi.Client.Model;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Net;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Shipping.ShipEngine.DTOs.CarrierAccount;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Client for interacting with ShipEngine
    /// </summary>
    [Component]
    public class ShipEngineWebClient : IShipEngineWebClient, IShipEngineResourceDownloader
    {
        private readonly ILogEntryFactory apiLogEntryFactory;
        private readonly IShipEngineApiFactory shipEngineApiFactory;
        private readonly IProxiedShipEngineWebClient proxiedShipEngineWebClient;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public ShipEngineWebClient(ILogEntryFactory apiLogEntryFactory,
            IShipEngineApiFactory shipEngineApiFactory,
            IProxiedShipEngineWebClient proxiedShipEngineWebClient)
        {
            this.apiLogEntryFactory = apiLogEntryFactory;
            this.shipEngineApiFactory = shipEngineApiFactory;
            this.proxiedShipEngineWebClient = proxiedShipEngineWebClient;
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

            var dhlAccountInfo = new DhlExpressAccountRegistrationRequest { AccountNumber = accountNumber, Nickname = accountNumber };

            var response = await MakeRequest<DhlExpressAccountRegistrationRequest, CarrierAccountCreationResponse>(
                ShipEngineEndpoints.DhlExpressAccountCreation, Method.POST, dhlAccountInfo, "ConnectDHLExpressAccount");

            if (response.Failure)
            {
                return GenericResult.FromError<string>(response.Message);
            }

            return response.Value.CarrierId;
        }

        /// <summary>
        /// Connect the given DHL eCommerce account to ShipEngine
        /// </summary>
        public async Task<GenericResult<string>> ConnectDhlEcommerceAccount(DhlEcommerceRegistrationRequest dhlRequest)
        {
            // Check to see if the carrier already exists in ShipEngine (they use clientId-pickupNumber for the accountId)
            var existingAccount = await GetCarrierId($"{dhlRequest.ClientId}-{dhlRequest.PickupNumber}").ConfigureAwait(false);

            if (existingAccount.Success)
            {
                return existingAccount;
            }

            var response = await MakeRequest<DhlEcommerceRegistrationRequest, CarrierAccountCreationResponse>(
            ShipEngineEndpoints.DhlEcommerceAccountCreation, Method.POST, dhlRequest, "ConnectDHLEcommerceAccount");

            if (response.Failure)
            {
                return GenericResult.FromError<string>(response.Message);
            }

            return response.Value.CarrierId;
        }

        /// <summary>
        /// Disconnect AmazonShipping
        /// </summary>
        public async Task<Result> DisconnectAmazonShippingAccount(string accountId) =>
            await MakeRequest(ShipEngineEndpoints.DisconnectAmazonShippingAccount(accountId),
                Method.DELETE,
                "DisconnectAmazonShipping",
                new List<HttpStatusCode> { HttpStatusCode.NoContent });

        /// <summary>
        /// Update an Amazon Accounts info
        /// </summary>
        public async Task<Result> UpdateAmazonAccount(IAmazonSWAAccountEntity amazonSwaAccount) =>
            await proxiedShipEngineWebClient.UpdateAmazonAccount(amazonSwaAccount);

        /// <summary>
        /// Connect an Amazon Shipping Account
        /// </summary>
        /// <remarks>
        /// unlike the other methods in this class we are manually interacting
        /// with the ShipEngine API because they have not added connecting to
        /// Amazon to their DLL yet
        /// </remarks>
        public async Task<GenericResult<string>> ConnectAmazonShippingAccount(string authCode) =>
            await proxiedShipEngineWebClient.ConnectAmazonShippingAccount(authCode, GetAmazonShippingCarrierID);

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
                return await ConnectCarrierAccount(apiInstance, ApiLogSource.Asendia, "ConnectAsendiaAccount",
                apiInstance.AsendiaAccountCarrierConnectAccountAsync(ascendiaAccountInfo, await GetApiKey()));
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
        /// Get the CarrierId for the given accountNumber
        /// </summary>
        private async Task<GenericResult<string>> GetCarrierId(string accountNumber)
        {
            try
            {
                string key = await GetApiKey();

                // If for some reason the key is blank show an error because we have to have the key to make the request
                if (string.IsNullOrWhiteSpace(key))
                {
                    return GenericResult.FromError<string>("Unable to find carrier. Api Key was blank.");
                }

                ICarriersApi carrierApi = shipEngineApiFactory.CreateCarrierApi();
                ConfigureLogging(carrierApi, ApiLogSource.ShipEngine, $"FindAccount{accountNumber}", LogActionType.Other);

                CarrierListResponse result = await carrierApi.CarriersListAsync(key).ConfigureAwait(false);
                var carrierId = result?.Carriers?.FirstOrDefault(c => c.AccountNumber == accountNumber)?.CarrierId ?? string.Empty;

                if (!carrierId.IsNullOrWhiteSpace())
                {
                    return GenericResult.FromSuccess(carrierId);
                }

                return GenericResult.FromError<string>("Unable to find carrier");
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<string>($"Unable to find carrier: {ex.Message}");
            }
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
        /// Purchases a label from ShipEngine using the given rateid
        /// </summary>
        public async Task<Label> PurchaseLabelWithRate(string rateId, PurchaseLabelWithoutShipmentRequest request, ApiLogSource apiLogSource)
        {
            ILabelsApi labelsApi = shipEngineApiFactory.CreateLabelsApi();
            ConfigureLogging(labelsApi, apiLogSource, "PurchaseLabel", LogActionType.Other);
            try
            {
                return await labelsApi.LabelsPurchaseLabelWithRateAsync(rateId, request, await GetApiKey());
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
                string localApiKey = await GetApiKey();

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
                return await ratesApi.RatesRateShipmentAsync(request, await GetApiKey());
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
                return await labelsApi.LabelsVoidLabelAsync(labelId, await GetApiKey());
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
                return await labelsApi.LabelsTrackAsync(labelId, await GetApiKey());
            }
            catch (ApiException ex)
            {
                throw new ShipEngineException(GetErrorMessage(ex));
            }
        }

        /// <summary>
        /// Track a shipment using the carrier code and tracking number
        /// </summary>
        public async Task<TrackingInformation> Track(string carrier, string trackingNumber, ApiLogSource apiLogSource)
        {
            ITrackingApi trackingApi = shipEngineApiFactory.CreateTrackingApi();

            ConfigureLogging(trackingApi, apiLogSource, "Track", LogActionType.GetRates);
            try
            {
                return await trackingApi.TrackingTrackAsync(await GetApiKey(), carrier, trackingNumber);
            }
            catch (ApiException ex)
            {
                throw new ShipEngineException(GetErrorMessage(ex));
            }
        }

        /// <summary>
        /// Get the api key
        /// </summary>
        private async Task<string> GetApiKey() =>
            await proxiedShipEngineWebClient.GetApiKey();

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
                throw new ShipEngineException($"An error occured while attempting to download resource.", ex);
            }
        }

        /// <summary>
        /// Get the Amazon Shipping carrier ID. There can only ever be one connected per api key.
        /// </summary>
        private async Task<GenericResult<string>> GetAmazonShippingCarrierID()
        {
            string key = await GetApiKey();
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
        /// Connects the given stamps.com account to the users ShipEngine account
        /// </summary>
        public async Task<GenericResult<string>> ConnectStampsAccount(string username, string password)
        {
            // Check to see if the account already exists in ShipEngine
            GenericResult<string> existingAccount = await GetCarrierId(username).ConfigureAwait(false);

            if (existingAccount.Success)
            {
                return existingAccount;
            }

            StampsAccountInformationDTO stampsAccountInfo = new StampsAccountInformationDTO
            {
                Nickname = username,
                Username = username,
                Password = password
            };

            ICarrierAccountsApi apiInstance = shipEngineApiFactory.CreateCarrierAccountsApi();

            try
            {
                string apiKey = await GetApiKey().ConfigureAwait(false);

                return await ConnectCarrierAccount(apiInstance, ApiLogSource.Usps, "ConnectStampsAccount",
                                                   apiInstance.StampsAccountCarrierConnectAccountAsync(stampsAccountInfo, apiKey)).ConfigureAwait(false);
            }
            catch (ApiException ex)
            {
                string error = GetErrorMessage(ex);

                // Stamps returns a cryptic error when the username or password are wrong, clean it up
                if (error.Contains("(530) Not logged in"))
                {
                    return GenericResult.FromError<string>("Unable to connect to Stamps. Please check your account information and try again.");
                }

                return GenericResult.FromError<string>(error);
            }
        }

        /// <summary>
        /// Disconnect the given stamps account
        /// </summary>
        public async Task<Result> DisconnectStampsAccount(string carrierId)
        {
            try
            {
                HttpJsonVariableRequestSubmitter submitter = new HttpJsonVariableRequestSubmitter();
                submitter.Headers.Add($"Content-Type: application/json");
                submitter.Headers.Add($"api-key: {await GetApiKey()}");
                submitter.Verb = HttpVerb.Delete;
                submitter.Uri = new Uri($"https://api.shipengine.com/v1/connections/carriers/stamps_com/{carrierId}");

                // Delete request returns no content, this is not an error
                submitter.AllowHttpStatusCodes(HttpStatusCode.NoContent, HttpStatusCode.NotFound);

                submitter.ProcessRequest(new ApiLogEntry(ApiLogSource.ShipEngine, "DisconnectStamps"), typeof(ShipEngineException));
                return Result.FromSuccess();
            }
            catch (Exception ex)
            {
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Update the given stamps account with the username and password
        /// </summary>
        public async Task<Result> UpdateStampsAccount(string carrierId, string username, string password) =>
            await proxiedShipEngineWebClient.UpdateStampsAccount(carrierId, username, password);

        /// <summary>
        /// Register a UPS account with One Balance
        /// </summary>
        public async Task<GenericResult<string>> RegisterUpsAccount(PersonAdapter person, string deviceIdentity) =>
            await proxiedShipEngineWebClient.RegisterUpsAccount(person, deviceIdentity);

        /// <summary>
        /// Create an Asendia Manifest for the given label IDs, retrying if necessary
        /// </summary>
        public async Task<Result> CreateAsendiaManifest(IEnumerable<string> labelIDs) =>
            await proxiedShipEngineWebClient.CreateAsendiaManifest(labelIDs);

        /// <summary>
        /// Make a request with no body
        /// </summary>
        private async Task<GenericResult<BaseShipEngineResponse>> MakeRequest(string endpoint, Method method, string logName, List<HttpStatusCode> allowedStatusCodes = null) =>
            await MakeRequest<object, BaseShipEngineResponse>(endpoint, method, null, logName, allowedStatusCodes);

        /// <summary>
        /// Make a request to ShipEngine
        /// </summary>
        private async Task<GenericResult<TResponse>> MakeRequest<TRequest, TResponse>(string endpoint,
            Method method,
            TRequest body,
            string logName,
            List<HttpStatusCode> allowedStatusCodes = null) where TResponse : BaseShipEngineResponse
        {
            try
            {
                if (allowedStatusCodes == null)
                {
                    allowedStatusCodes = new List<HttpStatusCode>();
                }

                // Always allow a 200 response
                allowedStatusCodes.Add(HttpStatusCode.OK);

                var apiKey = await GetApiKey();

                var client = new RestClient(ShipEngineEndpoints.BaseUrl);

                var request = new RestRequest(endpoint, method);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("api-key", apiKey);

                request.JsonSerializer = new RestSharpJsonNetSerializer(new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy(),
                    },
                });

                if (body != null)
                {
                    request.AddJsonBody(body);
                }

                ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.ShipEngine, logName);
                logEntry.LogRequest(request, client, "txt");

                IRestResponse response = await client.ExecuteTaskAsync(request).ConfigureAwait(false);

                logEntry.LogResponse(response, "txt");

                var responseObject = JsonConvert.DeserializeObject<TResponse>(response.Content);

                if (allowedStatusCodes.Contains(response.StatusCode))
                {
                    return responseObject;
                }

                return GenericResult.FromError<TResponse>(responseObject?.Errors?.FirstOrDefault()?.Message ?? response.ErrorMessage, responseObject);
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<TResponse>("An error occurred communicating with ShipEngine", ex);
            }
        }
    }
}
