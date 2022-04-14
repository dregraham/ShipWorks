using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;

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
        /// Get a page of orders from a platform order source
        /// </summary>
        public async Task<object> GetOrders(string orderSourceId, string continuationToken)
        {
            var request = new RestRequest($"api/ordersource/{orderSourceId}", Method.GET);
            request.AddQueryParameter("ContinuationToken", continuationToken);

            return await warehouseRequestClient.MakeRequest<object>(request, "PlatformGetOrders").ConfigureAwait(false);
        }
    }
}
