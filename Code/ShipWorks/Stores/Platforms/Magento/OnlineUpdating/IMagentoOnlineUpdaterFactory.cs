using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Magento.OnlineUpdating
{
    /// <summary>
    /// Factory for creating the correct Magento online updater
    /// </summary>
    public interface IMagentoOnlineUpdaterFactory
    {
        /// <summary>
        /// Create the correct updater
        /// </summary>
        IMagentoOnlineUpdater Create(MagentoStoreEntity store);
    }
}
