using RestSharp;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Interface for creating Overstock IRestClients
    /// </summary>
    public interface IOverstockRestClientFactory
    {
        /// <summary>
        /// Create an IRestClient for the given store
        /// </summary>
        IRestClient Create(IOverstockStoreEntity store);
    }
}
