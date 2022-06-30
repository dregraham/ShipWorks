using System.Net.Http;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Hub
{
    /// <summary>
    /// Client to communicate with SP (Via Platform)
    /// </summary>
    [Component]
    public class HubPlatformShippingClient : IHubPlatformShippingClient
    {
        private readonly IWarehouseRequestFactory warehouseRequestFactory;
        private readonly IWarehouseRequestClient warehouseRequestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubPlatformShippingClient(IWarehouseRequestFactory warehouseRequestFactory, IWarehouseRequestClient warehouseRequestClient)
        {
            this.warehouseRequestFactory = warehouseRequestFactory;
            this.warehouseRequestClient = warehouseRequestClient;
        }

        /// <summary>
        /// Call Hub to pass through  call to Platform
        /// </summary>
        public async Task<object> CallViaPassthrough(object obj, string platformEndpoint, HttpMethod method)
        {
            var request = warehouseRequestFactory.Create(
                WarehouseEndpoints.PlatformPassthrough,
                Method.POST,
                obj);

            request.AddHeader("PlatformEndpoint", platformEndpoint);
            request.AddHeader("PlatformMethod", method.Method);
            
            var result = await warehouseRequestClient.MakeRequest<object>(request, nameof(WarehouseEndpoints.PlatformPassthrough))
                .ConfigureAwait(false);

            return result;
        }
    }
}
