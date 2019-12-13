using Interapptive.Shared.ComponentRegistration;
using RestSharp;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    /// <summary>
    /// Class for creating Rakuten IRestClients
    /// </summary>
    [Component]
    public class RakutenRestClientFactory : IRakutenRestClientFactory
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenRestClientFactory()
        {
        }

        /// <summary>
        /// Create an IRestClient for the given endpoint
        /// </summary>
        public IRestClient Create(string endpointBase) => new RestClient(endpointBase);
    }
}
