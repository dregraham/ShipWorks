using System;
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

            var response = await MakeRequest<CarrierAccountCreationResponse>(
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

            var response = await MakeRequest<CarrierAccountCreationResponse>(
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

            var asendiaAccountInfo = new AsendiaAccountRegistrationRequest
            {
                AccountNumber = accountNumber,
                Nickname = accountNumber,
                FtpUsername = username,
                FtpPassword = password
            };

            var response = await MakeRequest<CarrierAccountCreationResponse>(
            ShipEngineEndpoints.AsendiaAccountCreation, Method.POST, asendiaAccountInfo, "ConnectAsendiaAccount");

            if (response.Failure)
            {
                // Asendia returns a cryptic error when the username or password are wrong, clean it up
                if (response.Message.Contains("(530) Not logged in"))
                {
                    return GenericResult.FromError<string>("Unable to connect to Asendia. Please check your account information and try again.");
                }

                return GenericResult.FromError<string>(response.Message);
            }

            return response.Value.CarrierId;
        }

        /// <summary>
        /// Get the CarrierId for the given accountNumber
        /// </summary>
        private async Task<GenericResult<string>> GetCarrierId(string accountNumber)
        {
            var response = await MakeRequest<CarrierListResponse>(
            ShipEngineEndpoints.ListCarriers, Method.GET, null, "ListCarriers");

            if (response.Failure)
            {
                return GenericResult.FromError<string>(response.Message);
            }

            var carrierId = response.Value?.Carriers?.FirstOrDefault(c => c.AccountNumber == accountNumber)?.CarrierId ?? string.Empty;

            if (!carrierId.IsNullOrWhiteSpace())
            {
                return GenericResult.FromSuccess(carrierId);
            }

            return GenericResult.FromError<string>("Unable to find carrier");
        }

        /// <summary>
        /// Purchases a label from ShipEngine using the given rateid
        /// </summary>
        public async Task<Label> PurchaseLabelWithRate(string rateId, PurchaseLabelWithoutShipmentRequest request, ApiLogSource apiLogSource)
        {
            ILabelsApi labelsApi = shipEngineApiFactory.CreateLabelsApi();

            try
            {
                return await labelsApi.LabelsPurchaseLabelWithRateAsync(rateId, request, await GetApiKey());
            }
            catch (Exception ex)
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

            try
            {
                string localApiKey = await GetApiKey();

                Label label = await telemetricResult.RunTimedEventAsync(TelemetricEventType.GetLabel,
                        () => labelsApi.LabelsPurchaseLabelAsync(request, localApiKey)
                        )
                    .ConfigureAwait(false);

                return label;
            }
            catch (Exception ex)
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

            try
            {
                return await ratesApi.RatesRateShipmentAsync(request, await GetApiKey());
            }
            catch (Exception ex)
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

            try
            {
                return await labelsApi.LabelsVoidLabelAsync(labelId, await GetApiKey());
            }
            catch (Exception ex)
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

            try
            {
                return await labelsApi.LabelsTrackAsync(labelId, await GetApiKey());
            }
            catch (Exception ex)
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

            try
            {
                return await trackingApi.TrackingTrackAsync(await GetApiKey(), carrier, trackingNumber);
            }
            catch (Exception ex)
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
        /// Get the error message from an ApiException
        /// </summary>
        private static string GetErrorMessage(Exception ex)
        {
            throw new Exception("We're getting rid of this.");
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
            var response = await MakeRequest<CarrierListResponse>(
            ShipEngineEndpoints.ListCarriers, Method.GET, null, "ListCarriers");

            if (response.Failure)
            {
                return GenericResult.FromError<string>(response.Message);
            }

            var carrierId = response.Value?.Carriers?.FirstOrDefault(c => c.CarrierCode.Equals("amazon_shipping_us", StringComparison.OrdinalIgnoreCase))?.CarrierId ?? string.Empty;

            if (!carrierId.IsNullOrWhiteSpace())
            {
                return GenericResult.FromSuccess(carrierId);
            }

            return GenericResult.FromError<string>("Unable to find carrier");
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

            var stampsAccountInfo = new StampsAccountRegistrationRequest
            {
                Nickname = username,
                Username = username,
                Password = password
            };

            var response = await MakeRequest<CarrierAccountCreationResponse>(
            ShipEngineEndpoints.StampsAccountCreation, Method.POST, stampsAccountInfo, "ConnectStampsAccount");

            if (response.Failure)
            {
                // Stamps returns a cryptic error when the username or password are wrong, clean it up
                if (response.Message.Contains("(530) Not logged in"))
                {
                    return GenericResult.FromError<string>("Unable to connect to Stamps. Please check your account information and try again.");
                }

                return GenericResult.FromError<string>(response.Message);
            }

            return response.Value.CarrierId;
        }

        /// <summary>
        /// Disconnect the given stamps account
        /// </summary>
        public async Task<Result> DisconnectStampsAccount(string carrierId) =>
            await MakeRequest(ShipEngineEndpoints.DisconnectStampsAccount(carrierId),
                Method.DELETE,
                "DisconnectStamps",
                new List<HttpStatusCode> { HttpStatusCode.NoContent, HttpStatusCode.NotFound });

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
        /// Make a request with no body and a base response
        /// </summary>
        private async Task<GenericResult<BaseShipEngineResponse>> MakeRequest(string endpoint, Method method, string logName, List<HttpStatusCode> allowedStatusCodes = null) =>
            await MakeRequest<BaseShipEngineResponse>(endpoint, method, null, logName, allowedStatusCodes);

        /// <summary>
        /// Make a request to ShipEngine
        /// </summary>
        private async Task<GenericResult<TResponse>> MakeRequest<TResponse>(string endpoint,
            Method method,
            object body,
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
