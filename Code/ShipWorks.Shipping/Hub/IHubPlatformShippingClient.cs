using System.Net.Http;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.Hub
{
    /// <summary>
    /// Client to communicate with SP (Via Platform)
    /// </summary>
    public interface IHubPlatformShippingClient
    {
        /// <summary>
        /// Call Hub to pass through  call to Platform
        /// </summary>
        Task<object> CallViaPassthrough(object obj, string platformEndpoint, HttpMethod method);
    }
}