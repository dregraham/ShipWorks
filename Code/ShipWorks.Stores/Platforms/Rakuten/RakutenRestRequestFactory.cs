using RestSharp;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    /// <summary>
    /// Class for creating Rakuten IRestRequests
    /// </summary>
    [Component]
    public class RakutenRestRequestFactory : IRakutenRestRequestFactory
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenRestRequestFactory()
        {
        }

        /// <summary>
        /// Create an IRestRequest for the given endpoint
        /// </summary>
        public IRestRequest Create(string resource, Method method)
        {
            return new RestRequest(resource, method);
        }
    }
}
