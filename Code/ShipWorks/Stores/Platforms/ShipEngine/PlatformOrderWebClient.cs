using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ShipEngine
{
    /// <summary>
    /// Web client for order interactions with Platform
    /// </summary>
    [Component]
    public class PlatformOrderWebClient : IPlatformOrderWebClient
    {
        private readonly IWarehouseRequestClient warehouseRequestClient;

        /// <summary>
        /// Constuctor
        /// </summary>
        public PlatformOrderWebClient(IWarehouseRequestClient warehouseRequestClient)
        {
            this.warehouseRequestClient = warehouseRequestClient;
        }

        /// <summary>
        /// Progress reporter that will be used for requests
        /// </summary>
        public IProgressReporter Progress { get; set; }

        /// <summary>
        /// Get a page of orders from a platform order source
        /// </summary>
        public async Task<PaginatedPlatformServiceResponseOfOrderSourceApiSalesOrder> GetOrders(string orderSourceId, string continuationToken)
        {
            var request = new RestRequest($"api/ordersource/{orderSourceId}", Method.GET);
            request.AddQueryParameter("ContinuationToken", continuationToken);

            // There's an issue with the deserialization and casting to an interface so we're casting manually
            var result = await warehouseRequestClient.MakeRequest(request, "PlatformGetOrders").ConfigureAwait(false);
            var deserializedResult = JsonConvert.DeserializeObject<PaginatedPlatformServiceResponseOfOrderSourceApiSalesOrder>(result.Value.Content);
            return deserializedResult;
        }
    }
}
