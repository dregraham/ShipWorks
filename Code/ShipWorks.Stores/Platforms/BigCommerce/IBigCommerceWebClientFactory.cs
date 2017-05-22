using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Factory to create BigCommerce web clients
    /// </summary>
    public interface IBigCommerceWebClientFactory
    {
        /// <summary>
        /// Create a BigCommerce web client with the given store
        /// </summary>
        IBigCommerceWebClient Create(IBigCommerceStoreEntity store);

        /// <summary>
        /// Create a BigCommerce web client with the given store and progress provider
        /// </summary>
        IBigCommerceWebClient Create(IBigCommerceStoreEntity store, IProgressReporter progress);
    }
}