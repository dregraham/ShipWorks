using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Core.UI.Controls.StoreSettings;

namespace ShipWorks.Stores.UI.Platforms.BigCommerce
{
    /// <summary>
    /// Control for configuring non-credential settings for BigCommerce
    /// </summary>
    public class BigCommerceDownloadCriteriaControl : DownloadModifiedDaysBackControl
    {
        /// <summary>
        /// Max number of days back allowed
        /// </summary>
        public override int MaxDaysBack => 15;

        /// <summary>
        /// Load the days back from the store entity
        /// </summary>
        public override int LoadDaysBack(StoreEntity store) => 
            ((BigCommerceStoreEntity) store).DownloadModifiedNumberOfDaysBack;

        /// <summary>
        /// Save the days back to the store entity
        /// </summary>
        public override void SaveDaysBack(StoreEntity store, int daysBack) => 
            ((BigCommerceStoreEntity) store).DownloadModifiedNumberOfDaysBack = daysBack;
    }
}
