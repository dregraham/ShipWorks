using Interapptive.Shared.ComponentRegistration;
using RestSharp;

namespace Interapptive.Shared.Net.RestSharp
{
    /// <summary>
    /// Factory for creating IRestRequests
    /// </summary>
    [Component]
    public class RestRequestFactory : IRestRequestFactory
    {
        /// <summary>
        /// Create a rest request with the given url and http method
        /// </summary>
        public IRestRequest Create(string url, Method method)
        {
            return new RestRequest(url, method);
        }
    }
}
