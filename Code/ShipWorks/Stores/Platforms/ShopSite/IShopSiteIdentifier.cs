using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// Allows interaction with the ShopSite identifier
    /// </summary>
    public interface IShopSiteIdentifier
    {
        /// <summary>
        /// Get the identifier from the given store
        /// </summary>
        string Get(IShopSiteStoreEntity typedStore);

        /// <summary>
        /// Set the identifier on the given store
        /// </summary>
        ShopSiteStoreEntity Set(ShopSiteStoreEntity store, string apiUrl);
    }
}
