using RestSharp;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Factory for creating Warehouse Product request objets
    /// </summary>
    public interface IWarehouseProductRequestFactory
    {
        /// <summary>
        /// Create a request object for making a warehouse product request
        /// </summary>
        /// <param name="endpoint">Url endpoint of the request</param>
        /// <param name="method">HTTP method of the request</param>
        /// <param name="payload">Data payload that will be serialized to JSON</param>
        /// <returns>Request object for use by the rest client</returns>
        IRestRequest Create(string endpoint, Method method, IWarehouseProductRequestData payload);

        /// <summary>
        /// Create a request object for making a warehouse product request
        /// </summary>
        /// <param name="endpoint">Url endpoint of the request</param>
        /// <param name="method">HTTP method of the request</param>
        /// <returns>Request object for use by the rest client</returns>
        IRestRequest Create(string endpoint, Method method);
    }
}