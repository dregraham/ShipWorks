using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// Factory to create ShopSite web clients
    /// </summary>
    public interface IShopSiteWebClientFactory
    {
        /// <summary>
        /// Create a ShopSite web client with the given store
        /// </summary>
        IShopSiteWebClient Create(IShopSiteStoreEntity store);

        /// <summary>
        /// Create a ShopSite web client with the given store and progress provider
        /// </summary>
        IShopSiteWebClient Create(IShopSiteStoreEntity store, IProgressReporter progress);
    }
}