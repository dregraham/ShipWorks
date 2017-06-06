using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.ShopSite.AccountSettings
{
    /// <summary>
    /// Verify a connection to ShopSite
    /// </summary>
    public interface IShopSiteConnectionVerifier
    {
        /// <summary>
        /// Verify the connection
        /// </summary>
        IResult Verify(ShopSiteStoreEntity store, IShopSiteAuthenticationPersistenceStrategy persistenceStrategy);
    }
}