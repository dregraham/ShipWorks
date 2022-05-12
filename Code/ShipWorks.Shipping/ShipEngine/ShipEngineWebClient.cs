using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Net.RestSharp;
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
        private readonly IProxiedShipEngineWebClient proxiedShipEngineWebClient;
        private readonly IRestClientFactory restClientFactory;
        private readonly IRestRequestFactory restRequestFactory;
        private readonly ILogEntryFactory logEntryFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public ShipEngineWebClient(IProxiedShipEngineWebClient proxiedShipEngineWebClient,
            IRestClientFactory restClientFactory,
            IRestRequestFactory restRequestFactory,
            ILogEntryFactory logEntryFactory)
        {
            this.proxiedShipEngineWebClient = proxiedShipEngineWebClient;
            this.restClientFactory = restClientFactory;
            this.restRequestFactory = restRequestFactory;
            this.logEntryFactory = logEntryFactory;
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

            var dhlAccountInfo = new DHLExpressAccountInformationDTO { AccountNumber = accountNumber, Nickname = accountNumber };

            var response = await MakeRequest<ConnectAccountResponseDTO>(
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

            var response = await MakeRequest<ConnectAccountResponseDTO>(
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

            var asendiaAccountInfo = new AsendiaAccountInformationDTO
            {
                AccountNumber = accountNumber,
                Nickname = accountNumber,
                FtpUsername = username,
                FtpPassword = password
            };

            var response = await MakeRequest<ConnectAccountResponseDTO>(
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
            var response = await MakeRequest<CarrierListResponse>(ShipEngineEndpoints.ListCarriers, Method.GET, null, "ListCarriers");

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
            var response = await MakeRequest<Label>(
                ShipEngineEndpoints.PurchaseLabelWithRate(rateId), Method.POST, request, "PurchaseLabelWithRate", null, apiLogSource);

            if (response.Failure)
            {
                throw new ShipEngineException($"An error occurred purchasing a label: {response.Message}");
            }

            return response.Value;
        }

        /// <summary>
        /// Purchases a label from ShipEngine using the given request
        /// </summary>
        public async Task<Label> PurchaseLabel(PurchaseLabelRequest request, ApiLogSource apiLogSource, TelemetricResult<IDownloadedLabelData> telemetricResult)
        {
            var response = await telemetricResult.RunTimedEventAsync(TelemetricEventType.GetLabel,
                () => MakeRequest<Label>(ShipEngineEndpoints.PurchaseLabel, Method.POST, request, "PurchaseLabel", null, apiLogSource)
                )
                .ConfigureAwait(false);

            if (response.Failure)
            {
                throw new ShipEngineException($"An error occurred purchasing a label: {response.Message}");
            }

            return response.Value;
        }

        /// <summary>
        /// Gets rates from ShipEngine using the given request
        /// </summary>
        /// <param name="request">The rate shipment request</param>
        /// <returns>The rate shipment response</returns>
        public async Task<RateShipmentResponse> RateShipment(RateShipmentRequest request, ApiLogSource apiLogSource)
        {
            var response = await MakeRequest<RateShipmentResponse>(
                ShipEngineEndpoints.RateShipment, Method.POST, request, "RateShipment", null, apiLogSource);

            if (response.Failure)
            {
                throw new ShipEngineException($"An error occurred fetching rates: {response.Message}");
            }

            return response.Value;
        }

        /// <summary>
        /// Void a ShipEngine Label
        /// </summary>
        public async Task<VoidLabelResponse> VoidLabel(string labelId, ApiLogSource apiLogSource)
        {
            var response = await MakeRequest<VoidLabelResponse>(
                ShipEngineEndpoints.VoidLabel(labelId), Method.PUT, null, "VoidLabel", null, apiLogSource);

            if (response.Failure)
            {
                throw new ShipEngineException($"An error occurred voiding a label: {response.Message}");
            }

            return response.Value;
        }

        /// <summary>
        /// Track a shipment using the label ID
        /// </summary>
        public async Task<TrackingInformation> Track(string labelId, ApiLogSource apiLogSource)
        {
            var response = await MakeRequest<TrackingInformation>(
                ShipEngineEndpoints.TrackLabel(labelId), Method.GET, null, "TrackShipment", null, apiLogSource);

            if (response.Failure)
            {
                throw new ShipEngineException($"An error occurred tracking a label: {response.Message}");
            }

            return response.Value;
        }

        /// <summary>
        /// Track a shipment using the carrier code and tracking number
        /// </summary>
        public async Task<TrackingInformation> Track(string carrier, string trackingNumber, ApiLogSource apiLogSource)
        {
            var response = await MakeRequest<TrackingInformation>(
                ShipEngineEndpoints.Track, Method.GET, null, "TrackShipment", null, apiLogSource);

            if (response.Failure)
            {
                throw new ShipEngineException($"An error occurred getting tracking information: {response.Message}");
            }

            return response.Value;
        }

        /// <summary>
        /// Get the api key
        /// </summary>
        private async Task<string> GetApiKey() =>
            await proxiedShipEngineWebClient.GetApiKey();

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

            var stampsAccountInfo = new StampsAccountInformationDTO
            {
                Nickname = username,
                Username = username,
                Password = password
            };

            var response = await MakeRequest<ConnectAccountResponseDTO>(
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
        /// Create a manifest for the given label IDs, retrying if necessary
        /// </summary>
        public async Task<GenericResult<CreateManifestResponse>> CreateManifest(List<string> labelIDs)
        {
            var response = await MakeRequest<CreateManifestResponse>(ShipEngineEndpoints.CreateManifest, Method.POST,
                new CreateManifestRequest { LabelIds = labelIDs }, "CreateManifest");

            if (response.Failure)
            {
                var alreadyManifestedIDs = response.Value?.Errors?
                    .Where(x => x.Message.Equals("Label has been manifested.", StringComparison.OrdinalIgnoreCase))?
                    .Select(x => x.LabelId);

                if (alreadyManifestedIDs.Any())
                {
                    var newIDs = labelIDs.Except(alreadyManifestedIDs).ToList();
                    if (newIDs.Any())
                    {
                        response = await MakeRequest<CreateManifestResponse>(ShipEngineEndpoints.CreateManifest, Method.POST,
                            new CreateManifestRequest { LabelIds = newIDs }, "CreateManifest");
                    }
                    else
                    {
                        return GenericResult.FromError<CreateManifestResponse>("All labels have already been added to a manifest.");
                    }
                }
            }

            return response;
        }

        /// <summary>
        /// Make a request with no body and a base response
        /// </summary>
        private async Task<GenericResult<BaseShipEngineResponse>> MakeRequest(string endpoint,
            Method method,
            string logName,
            List<HttpStatusCode> allowedStatusCodes = null,
            ApiLogSource logSource = ApiLogSource.ShipEngine) =>
            await MakeRequest<BaseShipEngineResponse>(endpoint, method, null, logName, allowedStatusCodes, logSource);

        /// <summary>
        /// Make a request to ShipEngine
        /// </summary>
        private async Task<GenericResult<TResponse>> MakeRequest<TResponse>(string endpoint,
            Method method,
            object body,
            string logName,
            List<HttpStatusCode> allowedStatusCodes = null,
            ApiLogSource logSource = ApiLogSource.ShipEngine) where TResponse : BaseShipEngineResponse
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

                var client = restClientFactory.Create(ShipEngineEndpoints.BaseUrl);

                var request = restRequestFactory.Create(endpoint, method);
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

                var logEntry = logEntryFactory.GetLogEntry(logSource, logName, LogActionType.Other);
                logEntry.LogRequest(request, client, "txt");

                IRestResponse response = await client.ExecuteTaskAsync(request).ConfigureAwait(false);

                logEntry.LogResponse(response, "txt");

                var responseObject = JsonConvert.DeserializeObject<TResponse>(response.Content);

                if (allowedStatusCodes.Contains(response.StatusCode))
                {
                    return responseObject;
                }

                return GenericResult.FromError<TResponse>(responseObject?.Errors?.FirstOrDefault()?.Message ?? response.ErrorMessage ?? "An error occurred communicating with ShipEngine.", responseObject);
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<TResponse>("An error occurred communicating with ShipEngine.", ex);
            }
        }
    }
}
