using Interapptive.Shared.ComponentRegistration;
using RestSharp;

namespace Interapptive.Shared.Net.RestSharp
{
    /// <summary>
    /// Factory for creating IRestClients
    /// </summary>
    [Component]
    public class RestClientFactory : IRestClientFactory
    {
        /// <summary>
        /// Create an IRestClient
        /// </summary>
        public IRestClient Create()
        {
            return new RestClient();
        }

        /// <summary>
        /// Create an IRestClient with the given base url
        /// </summary>
        public IRestClient Create(string baseUrl)
        {
            return new RestClient(baseUrl);
        }
    }
}
