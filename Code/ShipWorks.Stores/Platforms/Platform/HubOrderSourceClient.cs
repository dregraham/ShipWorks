using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Stores.Platforms.Amazon.DTO;
using ShipWorks.Stores.Platforms.AmeriCommerce.WebServices;

namespace ShipWorks.Stores.Platforms.Platform
{
    /// <summary>
    /// Client to communicate with SP (Via Platform)
    /// </summary>
    [Component]
    public class HubOrderSourceClient : IHubOrderSourceClient
    {
        private readonly IWarehouseRequestFactory warehouseRequestFactory;
        private readonly IWarehouseRequestClient warehouseRequestClient;

        private const int DefaultDaysBack = 30;
        private const string GetMonoauthInitiateUrl = "GetMonoauthInitiateUrl";
        private const string GetInitiateUpdateOrderSourceUrl = "GetInitiateUpdateOrderSourceUrl";

        /// <summary>
        /// Constructor
        /// </summary>
        public HubOrderSourceClient(IWarehouseRequestFactory warehouseRequestFactory, IWarehouseRequestClient warehouseRequestClient)
        {
            this.warehouseRequestFactory = warehouseRequestFactory;
            this.warehouseRequestClient = warehouseRequestClient;
        }


        /// <summary>
        /// Get the monoauth URL to initiate an order source creation for non Amazon
        /// </summary>
        /// <remarks>
        /// Note that the orderSourceName will be used in both the URL used to communicate with the hub and the
        /// redirectUrl the hub will send on to monoauth
        /// </remarks>
        public async Task<string> GetCreateOrderSourceInitiateUrl(string orderSourceName, int? daysBack, Dictionary<string, string> otherParameters = null)
        {
            string otherParametersString = CreateOtherParametersString(otherParameters);
            var request = warehouseRequestFactory.Create(
                WarehouseEndpoints.GetCreateOrderSourceInitiateUrl(orderSourceName, UpdateLocalUrl(warehouseRequestClient.WarehouseUrl), daysBack ?? DefaultDaysBack, otherParametersString), Method.GET,
                null);

            var result = await warehouseRequestClient.MakeRequest<GetMonauthInitiateUrlResponse>(request, GetMonoauthInitiateUrl)
                .ConfigureAwait(false);

            return result.InitiateUrl;
        }

        private static string CreateOtherParametersString(Dictionary<string, string> otherParameters)
        {
            string otherParametersString = string.Empty;
            if (otherParameters != null && otherParameters.Count > 0)
            {
                var paramaterStrings = otherParameters.Select(op => $"{op.Key}={op.Value}");
                otherParametersString = $"&{string.Join("&", paramaterStrings)}";
            }

            return otherParametersString;
        }

        /// <summary>
        /// Get the Monoauth URL to initiate an order source credential change
        /// </summary>
        /// <remarks>
        /// Note that the orderSourceName will be used in both the URL used to communicate with the hub and the
        /// redirectUrl the hub will send on to monoauth
        /// </remarks>
        public async Task<string> GetUpdateOrderSourceInitiateUrl(string orderSourceName, string orderSourceId, Dictionary<string, string> otherParameters = null)
        {
            var request = warehouseRequestFactory.Create(
                WarehouseEndpoints.GetUpdateOrderSourceInitiateUrl(orderSourceName, UpdateLocalUrl(warehouseRequestClient.WarehouseUrl), orderSourceId, CreateOtherParametersString(otherParameters)), Method.GET,
                null);

            var result = await warehouseRequestClient.MakeRequest<GetMonauthInitiateUrlResponse>(request, GetInitiateUpdateOrderSourceUrl)
                .ConfigureAwait(false);

            return result.InitiateUrl;
        }

        /// <summary>
        /// Call Hub to get a Platform Amazon carrier Id for Buy Shipping
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPlatformAmazonCarrierId(string uniqueIdentifier)
        {
            var request = warehouseRequestFactory.Create(
                WarehouseEndpoints.CreateAmazonCarrierFromAmazonStore,
                Method.POST,
                new { uniqueIdentifier = uniqueIdentifier });

            var result = await warehouseRequestClient.MakeRequest<GetPlatformAmazonCarrierIdResponse>(request, nameof(WarehouseEndpoints.CreateAmazonCarrierFromAmazonStore))
                .ConfigureAwait(false);

            return result.CarrierId;
        }


        /// <summary>
        /// Call Hub to get a Platform Amazon carrier Id for Buy Shipping from a MWS store
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateAmazonCarrierFromMws(string sellingPartnerId, string mwsAuthToken, string countryCode)
        {
            var request = warehouseRequestFactory.Create(
                WarehouseEndpoints.CreateAmazonCarrierFromMws,
                Method.POST,
                new { sellingPartnerId, mwsAuthToken, countryCode });

            var result = await warehouseRequestClient.MakeRequest<GetPlatformAmazonCarrierIdResponse>(request, nameof(WarehouseEndpoints.CreateAmazonCarrierFromMws))
                .ConfigureAwait(false);

            return result.CarrierId;
        }

        /// <summary>
        /// Get the monoauth url to initiate creating a Platform Carrier
        /// </summary>
        public async Task<string> GetCreateCarrierInitiateUrl(string orderSourceName, string apiRegion)
        {
            var request = warehouseRequestFactory.Create(
                WarehouseEndpoints.GetCreateCarrierInitiateUrl(orderSourceName, UpdateLocalUrl(warehouseRequestClient.WarehouseUrl), apiRegion), Method.GET, null);

            var result = await warehouseRequestClient.MakeRequest<GetMonauthInitiateUrlResponse>(request, GetMonoauthInitiateUrl)
                .ConfigureAwait(false);

            return result.InitiateUrl;
        }

        /// <summary>
        /// Get the monoauth url to update a platform carrier
        /// </summary>
        public async Task<string> GetUpdateCarrierInitiateUrl(string orderSourceName, string carrierId, string apiRegion, string sellerId)
        {
            var request = warehouseRequestFactory.Create(
                 WarehouseEndpoints.GetUpdateCarrierInitiateUrl(orderSourceName, UpdateLocalUrl(warehouseRequestClient.WarehouseUrl), apiRegion, sellerId, carrierId), Method.GET,
                 null);

            var result = await warehouseRequestClient.MakeRequest<GetMonauthInitiateUrlResponse>(request, GetInitiateUpdateOrderSourceUrl)
                .ConfigureAwait(false);

            return result.InitiateUrl;
        }

        /// <summary>
        /// Call hub to update the amazon sp FBA criteria
        /// </summary>
        public async Task UpdateAmazonFbaCriteria(string orderSourceId, bool downloadFba, string apiRegion)
        {
            var request = warehouseRequestFactory.Create(WarehouseEndpoints.UpdateAmazonFbaCriteria, Method.POST, new { orderSourceId, downloadFba, apiRegion });
            var result = await warehouseRequestClient.MakeRequest(request, "UpdateAmazonFbaCriteria");

            if (result.Failure)
            {
                throw new Exception(result.Message);
            }
        }

        /// <summary>
        /// Call hub to update the platform/shopify configuration for notify_customer
        /// </summary>
        public async Task UpdateShopifyNotifyCustomer(string orderSourceId, bool notifyCustomer)
        {
            //var request = warehouseRequestFactory.Create(WarehouseEndpoints.UpdateShopifyNotifyCustomer, Method.POST, new { orderSourceId, notifyCustomer });
            //var result = await warehouseRequestClient.MakeRequest(request, "UpdateShopifyNotifyCustomer");

            //if (result.Failure)
            //{
            //    throw new Exception(result.Message);
            //}
        }

        /// <summary>
        /// Updates the port if pointing local
        /// </summary>
        private string UpdateLocalUrl(string warehouseUrl) =>
            warehouseUrl.Replace("http://localhost:4001", "http://localhost:3000");

        public async Task MigrateStoreToPlatform(string warehouseStoreId, string orderSourceId)
        {
            var request = warehouseRequestFactory.Create(WarehouseEndpoints.MigrateStoreToPlatform, Method.POST, new { storeId=warehouseStoreId, orderSourceId});
            var result = await warehouseRequestClient.MakeRequest(request, "MigrateStoreToPlatform");

            if (result.Failure)
            {
                throw new Exception(result.Message);
            }
        }
    }
}