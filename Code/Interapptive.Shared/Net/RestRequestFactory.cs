using RestSharp;
using Interapptive.Shared.ComponentRegistration;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Class for creating Rakuten IRestRequests
    /// </summary>
    [Component]
    public class RestRequestFactory : IRestRequestFactory
    {
        /// <summary>
        /// Create an IRestRequest for the given endpoint
        /// </summary>
        public IRestRequest Create(string resource, Method method) => new RestRequest(resource, method);
    }
}
