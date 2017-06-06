using System;
using Autofac;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// Factory to create ShopSite web clients
    /// </summary>
    [Component]
    public class ShopSiteWebClientFactory : IShopSiteWebClientFactory
    {
        private readonly ILifetimeScope lifetimeScope;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteWebClientFactory(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// Create a ShopSite web client with the given store
        /// </summary>
        public IShopSiteWebClient Create(IShopSiteStoreEntity store)
        {
            return lifetimeScope.ResolveKeyed<IShopSiteWebClient>(store.ShopSiteAuthentication, TypedParameter.From(store));
        }

        /// <summary>
        /// Create a ShopSite web client with the given store and progress provider
        /// </summary>
        public IShopSiteWebClient Create(IShopSiteStoreEntity store, IProgressReporter progress)
        {
            IShopSiteWebClient client = Create(store);
            client.ProgressReporter = progress;
            return client;
        }
    }
}
