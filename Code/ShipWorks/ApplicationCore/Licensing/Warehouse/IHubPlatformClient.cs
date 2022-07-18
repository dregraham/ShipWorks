using System.Net.Http;
using System.Threading.Tasks;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Client to communicate with SP (Via Platform)
    /// </summary>
    public interface IHubPlatformClient
    {
        /// <summary>
        /// Call Hub to pass through  call to Platform
        /// </summary>
        Task<object> CallViaPassthrough(object obj, string platformEndpoint, HttpMethod method, string logName);
    }
}