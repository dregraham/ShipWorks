using RestSharp;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Creates a warehouse reques
    /// </summary>
    public interface IWarehouseRequestFactory
    {
        /// <summary>
        /// Create an initilized request for a warehouse
        /// </summary>
        IRestRequest Create(string endpoint, Method method, object payload);
    }
}