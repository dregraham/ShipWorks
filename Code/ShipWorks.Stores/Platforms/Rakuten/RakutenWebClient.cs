using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using RestSharp;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Rakuten.DTO;
using ShipWorks.Stores.Platforms.Rakuten.DTO.Requests;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    /// <summary>
    /// The client for communicating with the Rakuten API
    /// </summary>
    [Component]
    public class RakutenWebClient : IRakutenWebClient
    {
        private readonly LruCache<string, RakutenProductsResponse> productCache;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IRakutenRestClientFactory clientFactory;
        private readonly IRakutenRestRequestFactory requestFactory;
        private readonly IInterapptiveOnly interapptiveOnly;
        private readonly ILogEntryFactory logFactory;
        private readonly RestSharpJsonNetSerializer jsonSerializer;

        private const string defaultEndpointBase = "https://openapi-rms.global.rakuten.com/2.0";
        private const string shippingPath = "/orders/{0}/{1}/{2}/shipping/{3}";
        private const string ordersResource = "ordersearch";
        private const string shippingResource = "orders/";
        private const string productResource = "products/{0}";
        private const string testResource = "configurations/{0}/labels/";
        private const string liveRegKey = "RakutenLiveServer";
        private readonly string endpointBase;

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenWebClient(IEncryptionProviderFactory encryptionProviderFactory,
            IRakutenRestClientFactory clientFactory, IRakutenRestRequestFactory requestFactory,
            IInterapptiveOnly interapptiveOnly,
            ILogEntryFactory logFactory)
        {
            this.encryptionProviderFactory = encryptionProviderFactory;
            this.clientFactory = clientFactory;
            this.requestFactory = requestFactory;
            this.interapptiveOnly = interapptiveOnly;
            this.logFactory = logFactory;
            endpointBase = GetEndpointBase();

            jsonSerializer = new RestSharpJsonNetSerializer(new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ"
            });

            productCache = new LruCache<string, RakutenProductsResponse>(1000);
        }

        /// <summary>
        /// Get the base endpoint for Rakuten requests
        /// </summary>
        private string GetEndpointBase()
        {
            if (interapptiveOnly.UseFakeAPI(liveRegKey))
            {
                var endpointOverride = InterapptiveOnly.Registry.GetValue("RakutenEndpoint", string.Empty);
                if (!string.IsNullOrWhiteSpace(endpointOverride))
                {
                    return endpointOverride.TrimEnd('/');
                }
            }

            return defaultEndpointBase;
        }

        /// <summary>
        /// Get a list of orders from Rakuten
        /// </summary>
        public async Task<RakutenOrdersResponse> GetOrders(IRakutenStoreEntity store, DateTime startDate)
        {
            var requestObject = new RakutenGetOrdersRequest(store, DateTime.UtcNow.AddDays(7), new DateTime(1970, 1, 1), startDate);

            return await SubmitRequest<RakutenOrdersResponse>(store.AuthKey, ordersResource, Method.POST, requestObject, "GetOrders").ConfigureAwait(false);
        }

        /// <summary>
        /// Get a product's variants and details from Rakuten
        /// </summary>
        public async Task<RakutenProductsResponse> GetProduct(IRakutenStoreEntity store, string baseSKU)
        {
            if (productCache.Contains(baseSKU))
            {
                return productCache[baseSKU];
            }

            var requestObject = new RakutenGetProductsRequest(baseSKU);

            var resource = string.Format(productResource, baseSKU);

            var product = await SubmitRequest<RakutenProductsResponse>(store.AuthKey, resource, Method.GET, requestObject, "GetProduct").ConfigureAwait(false);

            productCache[baseSKU] = product;

            return product;
        }

        /// <summary>
        /// Mark order as shipped and upload tracking number
        /// </summary>
        public async Task<RakutenBaseResponse> ConfirmShipping(IRakutenStoreEntity store, ShipmentEntity shipment)
        {
            var rakutenOrder = shipment.Order as RakutenOrderEntity;

            if (rakutenOrder == null)
            {
                throw new ArgumentException("A non-Rakuten shipment was passed to the ConfirmShipping method.");
            }

            var path = string.Format(shippingPath, store.MarketplaceID, store.ShopURL, rakutenOrder.OrderNumberComplete, rakutenOrder.RakutenPackageID);

            var shippingInfo = new RakutenShippingInfo
            {
                CarrierName = shipment.ShipmentTypeCode == ShipmentTypeCode.Other ?
                    ShippingManager.GetOtherCarrierDescription(shipment).Name :
                    ShippingManager.GetCarrierName(shipment.ShipmentTypeCode),
                ShippingStatus = "Shipped",
                TrackingNumber = shipment.TrackingNumber
            };

            var requestObject = new List<RakutenPatchOperation>();

            requestObject.Add(new RakutenPatchOperation
            {
                Operation = "replace",
                Path = path,
                Value = shippingInfo
            });

            return await SubmitRequest<RakutenBaseResponse>(store.AuthKey, shippingResource, Method.PATCH, requestObject, "ConfirmShipping").ConfigureAwait(false);
        }

        /// <summary>
        /// Verify we can connect with Rakuten
        /// </summary>
        public async Task<GenericResult<bool>> TestConnection(RakutenStoreEntity testStore)
        {
            if (interapptiveOnly.UseFakeAPI(liveRegKey))
            {
                if (!testStore.ShopURL.Equals("fake"))
                {
                    return new ArgumentException("Shop Name must be \"fake\" when using fake stores.");
                }

                return true;
            }

            try
            {
                var resource = string.Format(testResource, testStore.MarketplaceID);

                await SubmitRequest<RakutenBaseResponse>(testStore.AuthKey, resource, Method.GET, null, "TestConnection").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return ex;
            }

            return true;
        }

        /// <summary>
        /// Send a request to Rakuten
        /// </summary>
        private async Task<T> SubmitRequest<T>(string encryptedAuthKey, string resource, Method method, object body, string action) where T : RakutenBaseResponse, new()
        {
            IApiLogEntry log = logFactory.GetLogEntry(ApiLogSource.Rakuten, action, LogActionType.Other);
            IRestClient client = CreateClient(endpointBase);
            IRestRequest request = requestFactory.Create(resource, method);
            request.JsonSerializer = jsonSerializer;

            if (body != null)
            {
                request.AddJsonBody(body);
            }

            log.LogRequest(request, client, "json");

            var authKey = encryptionProviderFactory.CreateRakutenEncryptionProvider().Decrypt(encryptedAuthKey);

            request.AddHeader("Authorization", $"ESA {authKey}");

            var response = await client.ExecuteTaskAsync<T>(request).ConfigureAwait(false);

            log.LogResponse(response, "json");

            if (response?.Data == null || response.Data.Errors != null)
            {
                ThrowError(response?.Data?.Errors);
            }

            return response?.Data;
        }

        /// <summary>
        /// Parse the Rakuten errors
        /// </summary>
        private void ThrowError(RakutenErrors errors)
        {
            RakutenError error = null;

            // Use the common error first
            if (errors?.Common != null)
            {
                error = errors.Common.FirstOrDefault();
            }
            else if (errors?.Specific != null)
            {
                error = errors.Specific.SelectMany(x => x.Value)?.FirstOrDefault();
            }

            if (error != null)
            {
                throw new WebException($"An error occured when communicating with Rakuten: {error.ShortMessage} ({error.ErrorCode}) - {error.LongMessage}");
            }
            else
            {
                throw new WebException("An error occured when communicating with Rakuten");
            }
        }

        /// <summary>
        /// Create a RestClient that uses our Json Serializer
        /// </summary>
        private IRestClient CreateClient(string baseUrl)
        {
            var client = clientFactory.Create(baseUrl);

            // Override with Newtonsoft JSON Handler
            client.AddHandler("application/json", () => jsonSerializer);
            client.AddHandler("text/json", () => jsonSerializer);
            client.AddHandler("text/x-json", () => jsonSerializer);
            client.AddHandler("text/javascript", () => jsonSerializer);
            client.AddHandler("*+json", () => jsonSerializer);

            return client;
        }
    }
}
