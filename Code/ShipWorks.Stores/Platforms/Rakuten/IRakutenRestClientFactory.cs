using RestSharp;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    /// <summary>
    /// Interface for creating Rakuten IRestClients
    /// </summary>
    public interface IRakutenRestClientFactory
    {
        /// <summary>
        /// Create an IRestClient for the given endpoint
        /// </summary>
        IRestClient Create(string endpointBase);
    }
}
