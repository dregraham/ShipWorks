using RestSharp;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Interface for creating IRestRequests
    /// </summary>
    public interface IRestRequestFactory
    {
        /// <summary>
        /// Create an IRestClient for the given endpoint
        /// </summary>
        IRestRequest Create(string resource, Method method);
    }
}
