using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Stores.Platforms.Amazon.DTO;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Client to communicate with SP (Via Platform)
    /// </summary>
    [Component]
    public class AmazonSPClient : IAmazonSpClient
    {
        private const string orderSource = "amazon";
        
        private readonly IWarehouseRequestFactory warehouseRequestFactory;
        private readonly IWarehouseRequestClient warehouseRequestClient;
    
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSPClient(IWarehouseRequestFactory warehouseRequestFactory, IWarehouseRequestClient warehouseRequestClient)
        {
            this.warehouseRequestFactory = warehouseRequestFactory;
            this.warehouseRequestClient = warehouseRequestClient;
        }

        /// <summary>
        /// Get the URL to initiate Monoauth
        /// </summary>
        public async Task<string> GetMonauthInitiateUrl()
        {
            var request = warehouseRequestFactory.Create(
                WarehouseEndpoints.GetInitiateMonoauthUrl(orderSource, warehouseRequestClient.WarehouseUrl), Method.GET,
                null);

            var result = await warehouseRequestClient.MakeRequest<GetMonauthInitiateUrlResponse>(request, "GetMonoauthInitiateUrl")
                .ConfigureAwait(false);

            return result.InitiateUrl;
        }
    }
}