using RestSharp;

namespace Interapptive.Shared.Net.RestSharp
{
    /// <summary>
    /// Factory for creating IRestRequests
    /// </summary>
    public interface IRestRequestFactory
    {
        /// <summary>
        /// Create a rest request with the given url and http method
        /// </summary>
        IRestRequest Create(string url, Method method);
    }
}
