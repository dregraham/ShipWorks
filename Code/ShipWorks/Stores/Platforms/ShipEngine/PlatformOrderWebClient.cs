using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;

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
            var result = await warehouseRequestClient.MakeRequest(request, "PlatformGetOrders", ApiLogSource.Amazon).ConfigureAwait(false);

            // Check that the call returned valid information
            PaginatedPlatformServiceResponseOfOrderSourceApiSalesOrder returnObject;
            if (result.Value?.Content?.IsNullOrWhiteSpace() ?? true)
            {
                returnObject = new PaginatedPlatformServiceResponseOfOrderSourceApiSalesOrder();
            }
            else
            {
                returnObject = JsonConvert.DeserializeObject<PaginatedPlatformServiceResponseOfOrderSourceApiSalesOrder>(result.Value.Content);
            }

            // Make sure the return data isn't null values
            if (returnObject.Data == null)
            {
                returnObject.Data = Array.Empty<OrderSourceApiSalesOrder>();
            }
            if (returnObject.Errors == null)
            {
                returnObject.Errors = Array.Empty<PlatformError>();
            }
            if (returnObject.ContinuationToken == null)
            {
                returnObject.ContinuationToken = string.Empty;
            }

            return returnObject;
        }
    }
}
