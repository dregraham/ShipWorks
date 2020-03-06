using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using RestSharp;
using ShipWorks.Common.Net;
using ShipWorks.Data;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Factory for creating Warehouse Product request objets
    /// </summary>
    [Component]
    public class WarehouseProductRequestFactory : IWarehouseProductRequestFactory
    {
        private readonly IRestRequestFactory restRequestFactory;
        private readonly IConfigurationData configurationData;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseProductRequestFactory(IRestRequestFactory restRequestFactory, IConfigurationData configurationData)
        {
            this.restRequestFactory = restRequestFactory;
            this.configurationData = configurationData;
        }

        /// <summary>
        /// Create a request object for making a warehouse product request
        /// </summary>
        /// <param name="endpoint">Url endpoint of the request</param>
        /// <param name="method">HTTP method of the request</param>
        /// <param name="payload">Data payload that will be serialized to JSON</param>
        /// <returns>Request object for use by the rest client</returns>
        public IRestRequest Create(string endpoint, Method method, IWarehouseProductRequestData payload)
        {
            payload.WarehouseId = configurationData.FetchReadOnly().WarehouseID;

            IRestRequest request = restRequestFactory.Create(endpoint, method);

            request.JsonSerializer = RestSharpJsonNetSerializer.CreateHubDefault();
            request.RequestFormat = DataFormat.Json;

            request.AddJsonBody(payload);

            return request;
        }
    }
}