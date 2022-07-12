using System.Net.Http;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using RestSharp;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Client to communicate with SP (Via Platform)
    /// </summary>
    [Component]
    public class HubPlatformClient : IHubPlatformClient
    {
        private readonly IWarehouseRequestFactory warehouseRequestFactory;
        private readonly IWarehouseRequestClient warehouseRequestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubPlatformClient(IWarehouseRequestFactory warehouseRequestFactory, IWarehouseRequestClient warehouseRequestClient)
        {
            this.warehouseRequestFactory = warehouseRequestFactory;
            this.warehouseRequestClient = warehouseRequestClient;
        }

        /// <summary>
        /// Call Hub to pass through  call to Platform
        /// </summary>
        public async Task<object> CallViaPassthrough(object obj, string platformEndpoint, HttpMethod method, string logName)
        {
            var request = warehouseRequestFactory.Create(
                WarehouseEndpoints.PlatformPassthrough,
                Method.POST,
                obj);

            request.AddHeader("PlatformEndpoint", platformEndpoint);
            request.AddHeader("PlatformMethod", method.Method);

            var result = await warehouseRequestClient.MakeRequest<object>(request, logName)
                .ConfigureAwait(false);

            return result;
        }
    }
}
