using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.ShipEngine.DTOs.Registration;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Class for making requests to ShipEngine through our Hub proxy
    /// </summary>
    [Component]
    public class ProxiedShipEngineWebClient : IProxiedShipEngineWebClient
    {
        private readonly IStoreManager storeManager;
        private readonly WebClientEnvironmentFactory webClientEnvironmentFactory;
        private readonly IShipEngineApiKey shipEngineApiKey;
        private readonly IUpsRegistrationRequestFactory registrationRequestFactory;
        private const string ShipEngineProxyEndpoint = "shipEngine";

        /// <summary>
        /// Constructor
        /// </summary>
        public ProxiedShipEngineWebClient(IStoreManager storeManager,
            WebClientEnvironmentFactory webClientEnvironmentFactory,
            IShipEngineApiKey shipEngineApiKey,
            IUpsRegistrationRequestFactory registrationRequestFactory)
        {
            this.storeManager = storeManager;
            this.webClientEnvironmentFactory = webClientEnvironmentFactory;
            this.shipEngineApiKey = shipEngineApiKey;
            this.registrationRequestFactory = registrationRequestFactory;
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
                string accountId = await GetAccountID();

                AmazonShippingUsAccountSettingsDTO updateRequest = new AmazonShippingUsAccountSettingsDTO(
                    email: amazonSwaAccount.Email,
                    merchantSellerId: store.MerchantID,
                    mwsAuthToken: store.AuthToken);

                IRestClient restClient = new RestClient(ShipEngineProxyUrl);

                IRestRequest request = new RestRequest();
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("SW-on-behalf-of", $"se-{accountId}");
                request.AddHeader("SW-originalRequestUrl", $"https://platform.shipengine.com/v1/connections/carriers/amazon_shipping_us/{amazonSwaAccount.ShipEngineCarrierId}/settings");
                request.AddHeader("SW-originalRequestMethod", Method.PUT.ToString());
                request.Method = Method.POST;
                request.RequestFormat = DataFormat.Json;
                request.JsonSerializer = new RestSharpJsonNetSerializer();

                request.AddJsonBody(updateRequest);

                ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.ShipEngine, "UpdateAmazonAccount");
                logEntry.LogRequest(request, restClient, "txt");

                IRestResponse response = await restClient.ExecuteTaskAsync(request).ConfigureAwait(false);

                logEntry.LogResponse(response, "txt");

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return Result.FromSuccess();
                }

                JObject responseBody = JObject.Parse(response.Content);

                JToken error = responseBody["errors"]?.FirstOrDefault() ?? responseBody["message"];

                return GenericResult.FromError<string>(error.ToString());
            }
            catch (Exception ex)
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
        public async Task<GenericResult<string>> ConnectAmazonShippingAccount(string authCode, Func<Task<GenericResult<string>>> GetAmazonShippingCarrierID)
        {
            try
            {
                // first we have to get the account id
                string accountId = await GetAccountID();

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

                IRestClient restClient = new RestClient(ShipEngineProxyUrl);

                IRestRequest request = new RestRequest();
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("SW-on-behalf-of", $"se-{accountId}");
                request.AddHeader("SW-originalRequestUrl", $"https://platform.shipengine.com/v1/connections/carriers/amazon_shipping_us");
                request.AddHeader("SW-originalRequestMethod", Method.POST.ToString());
                request.Method = Method.POST;
                request.RequestFormat = DataFormat.Json;
                request.JsonSerializer = new RestSharpJsonNetSerializer();

                request.AddJsonBody(amazonAccountInfo);

                ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.ShipEngine, "ConnectAmazonAccount");
                logEntry.LogRequest(request, restClient, "txt");

                IRestResponse response = await restClient.ExecuteTaskAsync(request).ConfigureAwait(false);

                logEntry.LogResponse(response, "txt");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    ConnectAccountResponseDTO result = JsonConvert.DeserializeObject<ConnectAccountResponseDTO>(response.Content);
                    return result.CarrierId;
                }

                return GenericResult.FromError<string>(JObject.Parse(response.Content)["errors"].FirstOrDefault()?["message"].ToString());
            }
            catch (Exception ex)
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
        /// Update the given stamps account with the username and password
        /// </summary>
        public async Task<Result> UpdateStampsAccount(string carrierId, string username, string password)
        {
            try
            {
                StampsAccountInformationDTO updateRequest = new StampsAccountInformationDTO()
                {
                    Nickname = username,
                    Username = username,
                    Password = password
                };

                string accountId = await GetAccountID();

                IRestClient restClient = new RestClient(ShipEngineProxyUrl);
                IRestRequest request = new RestRequest();
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("SW-on-behalf-of", $"se-{accountId}");
                request.AddHeader("SW-originalRequestUrl", $"https://api.shipengine.com/v1/connections/carriers/stamps_com/{carrierId}");
                request.AddHeader("SW-originalRequestMethod", Method.PUT.ToString());
                request.Method = Method.POST;
                request.RequestFormat = DataFormat.Json;
                request.JsonSerializer = new RestSharpJsonNetSerializer();

                request.AddJsonBody(updateRequest);

                ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.ShipEngine, "UpdateStamps");
                logEntry.LogRequest(request, restClient, "txt");

                IRestResponse response = await restClient.ExecuteTaskAsync(request).ConfigureAwait(false);

                logEntry.LogResponse(response, "txt");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Result.FromSuccess();
                }

                JObject responseBody = JObject.Parse(response.Content);

                JToken error = responseBody["errors"]?.FirstOrDefault() ?? responseBody["message"];

                return GenericResult.FromError<string>(error.ToString());
            }
            catch (Exception ex)
            {
                string error = GetErrorMessage(ex);
                return Result.FromError(error);
            }
        }

        /// <summary>
        /// Register a UPS account with One Balance
        /// </summary>
        public async Task<GenericResult<string>> RegisterUpsAccount(PersonAdapter person, string deviceIdentity)
        {
            try
            {
                IRestClient restClient = new RestClient(ShipEngineProxyUrl);

                IRestRequest request = new RestRequest();
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("SW-on-behalf-of", await GetApiKey().ConfigureAwait(false));
                request.AddHeader("SW-originalRequestUrl", "https://platform.shipengine.com/v1/registration/ups");
                request.AddHeader("SW-originalRequestMethod", Method.POST.ToString());
                request.Method = Method.POST;
                request.RequestFormat = DataFormat.Json;
                request.JsonSerializer = new RestSharpJsonNetSerializer();

                UpsRegistrationRequest registration = registrationRequestFactory.Create(person, deviceIdentity);
                request.AddJsonBody(registration);

                ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.ShipEngine, "RegisterUpsAccount");
                logEntry.LogRequest(request, restClient, "txt");

                IRestResponse response = await restClient.ExecuteTaskAsync(request).ConfigureAwait(false);

                logEntry.LogResponse(response, "txt");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JObject responseObject = JObject.Parse(response.Content);

                    return responseObject["carrier_id"].ToString();
                }

                return GenericResult.FromError<string>(JObject.Parse(response.Content)["errors"].FirstOrDefault()?["message"].ToString());
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<string>("Unable to register the UPS Account", ex);
            }
        }

        /// <summary>
        /// Get the api key
        /// </summary>
        public async Task<string> GetApiKey()
        {
            if (string.IsNullOrWhiteSpace(shipEngineApiKey.Value))
            {
                await shipEngineApiKey.Configure().ConfigureAwait(false);
            }

            return shipEngineApiKey.Value;
        }

        /// <summary>
        /// Create an Asendia Manifest for the given label IDs, retrying if necessary
        /// </summary>
        public async Task<Result> CreateAsendiaManifest(IEnumerable<string> labelIDs)
        {
            try
            {
                var response = await SubmitAsendiaManifest(labelIDs).ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var alreadyManifestedIDs = JObject.Parse(response.Content)["errors"]
                        .Where(x => x["message"].ToString().Equals("Label has been manifested.", StringComparison.OrdinalIgnoreCase))
                        .Select(x => x["label_id"].ToString());

                    if (alreadyManifestedIDs.Any())
                    {
                        var newIDs = labelIDs.Except(alreadyManifestedIDs);
                        response = await SubmitAsendiaManifest(newIDs);
                    }
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Result.FromSuccess();
                }

                return Result.FromError(JObject.Parse(response.Content)["errors"].FirstOrDefault()?["message"].ToString());
            }
            catch (Exception ex)
            {
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Submit the Asendia Manifest creation request to ShipEngine
        /// </summary>
        private async Task<IRestResponse> SubmitAsendiaManifest(IEnumerable<string> labelIDs)
        {
            IRestClient restClient = new RestClient(ShipEngineProxyUrl);

            IRestRequest request = new RestRequest();
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("SW-on-behalf-of", await GetApiKey().ConfigureAwait(false));
            request.AddHeader("SW-originalRequestUrl", "https://platform.shipengine.com/v1/manifests");
            request.AddHeader("SW-originalRequestMethod", Method.POST.ToString());
            request.Method = Method.POST;
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new RestSharpJsonNetSerializer();

            request.AddJsonBody(
                new
                {
                    label_ids = labelIDs
                });

            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.ShipEngine, "CreateAsendiaManifest");
            logEntry.LogRequest(request, restClient, "txt");

            IRestResponse response = await restClient.ExecuteTaskAsync(request).ConfigureAwait(false);

            logEntry.LogResponse(response, "txt");

            return response;
        }

        /// <summary>
        /// Get an account ID from a WhoAmI request
        /// </summary>
        private async Task<string> GetAccountID()
        {
            HttpJsonVariableRequestSubmitter whoAmIRequest = new HttpJsonVariableRequestSubmitter();
            whoAmIRequest.Headers.Add($"Content-Type: application/json");
            whoAmIRequest.Headers.Add($"api-key: {await GetApiKey()}");
            whoAmIRequest.Verb = HttpVerb.Get;
            whoAmIRequest.Uri = new Uri("https://platform.shipengine.com/v1/environment/whoami");

            EnumResult<HttpStatusCode> result =
                whoAmIRequest.ProcessRequest(new ApiLogEntry(ApiLogSource.ShipEngine, "WhoAmI"), typeof(ShipEngineException));
            return JObject.Parse(result.Message)["data"]["username"].ToString();
        }

        /// <summary>
        /// Gets the proxy url for ShipEngine
        /// </summary>
        private string ShipEngineProxyUrl => new Uri(webClientEnvironmentFactory.SelectedEnvironment.ProxyUrl).AddToPath(ShipEngineProxyEndpoint).ToString();

        /// <summary>
        /// Get the error message from an ApiException
        /// </summary>
        private static string GetErrorMessage(Exception ex)
        {
            throw new Exception("We're getting rid of this.");
        }
    }
}
