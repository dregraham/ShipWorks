using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Stores.Platforms.Amazon.DTO;

namespace ShipWorks.Stores.Platforms.Platform
{
    /// <summary>
    /// Client to communicate with SP (Via Platform)
    /// </summary>
    [Component]
    public class HubMonoauthClient : IHubMonoauthClient
    {
        private readonly IWarehouseRequestFactory warehouseRequestFactory;
        private readonly IWarehouseRequestClient warehouseRequestClient;
        
        private const string GetMonoauthInitiateUrl = "GetMonoauthInitiateUrl";
        private const string GetInitiateUpdateOrderSourceUrl = "GetInitiateUpdateOrderSourceUrl";
    
        /// <summary>
        /// Constructor
        /// </summary>
        public HubMonoauthClient(IWarehouseRequestFactory warehouseRequestFactory, IWarehouseRequestClient warehouseRequestClient)
        {
            this.warehouseRequestFactory = warehouseRequestFactory;
            this.warehouseRequestClient = warehouseRequestClient;
        }

        /// <summary>
        /// Get the monoauth URL to initiate an order source creation
        /// </summary>
        /// <remarks>
        /// Note that the orderSourceName will be used in both the URL used to communicate with the hub and the
        /// redirectUrl the hub will send on to monoauth
        /// </remarks>
        public async Task<string> GetCreateOrderSourceInitiateUrl(string orderSourceName, string apiRegion)
        {
            var request = warehouseRequestFactory.Create(
                WarehouseEndpoints.GetCreateOrderSourceInitiateUrl(orderSourceName, warehouseRequestClient.WarehouseUrl, apiRegion), Method.GET,
                null);

            var result = await warehouseRequestClient.MakeRequest<GetMonauthInitiateUrlResponse>(request, GetMonoauthInitiateUrl)
                .ConfigureAwait(false);

            return result.InitiateUrl;
        }

        /// <summary>
        /// Get the Monoauth URL to initiate an order source credential change
        /// </summary>
        /// <remarks>
        /// Note that the orderSourceName will be used in both the URL used to communicate with the hub and the
        /// redirectUrl the hub will send on to monoauth
        /// </remarks>
        public async Task<string> GetUpdateOrderSourceInitiateUrl(string orderSourceName, string orderSourceId, string apiRegion)
        {
            var request = warehouseRequestFactory.Create(
                WarehouseEndpoints.GetUpdateOrderSourceInitiateUrl(orderSourceName, warehouseRequestClient.WarehouseUrl, orderSourceId, apiRegion), Method.GET,
                null);

            var result = await warehouseRequestClient.MakeRequest<GetMonauthInitiateUrlResponse>(request, GetInitiateUpdateOrderSourceUrl)
                .ConfigureAwait(false);

            return result.InitiateUrl;
        }
    }
}