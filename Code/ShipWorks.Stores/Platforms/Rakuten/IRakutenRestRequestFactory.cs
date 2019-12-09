using RestSharp;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    /// <summary>
    /// Interface for creating Rakuten IRestClients
    /// </summary>
    public interface IRakutenRestRequestFactory
    {
        /// <summary>
        /// Create an IRestClient for the given endpoint
        /// </summary>
        IRestRequest Create(string resource, Method method);
    }
}
