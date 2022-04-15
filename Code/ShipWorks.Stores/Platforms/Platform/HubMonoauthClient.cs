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
    
        /// <summary>
        /// Constructor
        /// </summary>
        public HubMonoauthClient(IWarehouseRequestFactory warehouseRequestFactory, IWarehouseRequestClient warehouseRequestClient)
        {
            this.warehouseRequestFactory = warehouseRequestFactory;
            this.warehouseRequestClient = warehouseRequestClient;
        }

        /// <summary>
        /// Get the URL to initiate Monoauth
        /// </summary>
        /// <remarks>
        /// Note that the orderSourceName will be used in both the URL used to communicate with the hub and the
        /// redirectUrl the hub will send on to monoauth
        /// </remarks>
        public async Task<string> GetMonauthInitiateUrl(string orderSourceName)
        {
            var request = warehouseRequestFactory.Create(
                WarehouseEndpoints.GetInitiateMonoauthUrl(orderSourceName, warehouseRequestClient.WarehouseUrl), Method.GET,
                null);

            var result = await warehouseRequestClient.MakeRequest<GetMonauthInitiateUrlResponse>(request, "GetMonoauthInitiateUrl")
                .ConfigureAwait(false);

            return result.InitiateUrl;
        }
    }
}