using RestSharp;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;

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
        public IRestClient Create(string endpointBase)
        {
            return new RestClient(endpointBase);
        }
    }
}
