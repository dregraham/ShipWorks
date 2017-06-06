using System;
using Interapptive.Shared.Threading;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Factory to create BigCommerce web clients
    /// </summary>
    [Component]
    public class BigCommerceWebClientFactory : IBigCommerceWebClientFactory
    {
        private readonly Func<IBigCommerceStoreEntity, IBigCommerceWebClient> createWebClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceWebClientFactory(Func<IBigCommerceStoreEntity, IBigCommerceWebClient> createWebClient)
        {
            this.createWebClient = createWebClient;
        }

        /// <summary>
        /// Create a BigCommerce web client with the given store
        /// </summary>
        public IBigCommerceWebClient Create(IBigCommerceStoreEntity store) => createWebClient(store);

        /// <summary>
        /// Create a BigCommerce web client with the given store and progress provider
        /// </summary>
        public IBigCommerceWebClient Create(IBigCommerceStoreEntity store, IProgressReporter progress)
        {
            IBigCommerceWebClient client = createWebClient(store);
            client.ProgressReporter = progress;
            return client;
        }
    }
}
