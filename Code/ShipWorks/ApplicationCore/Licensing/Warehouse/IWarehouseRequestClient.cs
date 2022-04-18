using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using RestSharp;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Represents the WarehouseRequestclient
    /// </summary>
    public interface IWarehouseRequestClient
    {
        /// <summary>
        /// Make the given request
        /// </summary>
        Task<GenericResult<IRestResponse>> MakeRequest(IRestRequest restRequest, string logName);

        /// <summary>
        /// Make an authenticated request
        /// </summary>
        Task<T> MakeRequest<T>(IRestRequest restRequest, string logName);

        /// <summary>
        /// Make an authenticated request
        /// </summary>
        Task<T> MakeRequest<T>(IRestRequest restRequest, string logName, CancellationToken cancellationToken);
        
        /// <summary>
        /// Get the WarehouseUrl
        /// </summary>
        string WarehouseUrl { get; }
    }
}