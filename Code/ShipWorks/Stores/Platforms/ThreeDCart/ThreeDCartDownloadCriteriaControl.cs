using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls.StoreSettings;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Control for setting days back to check for modified orders for 3dCart
    /// </summary>
    public class ThreeDCartDownloadCriteriaControl : DownloadModifiedDaysBackControl
    {
        /// <summary>
        /// Max number of days back allowed
        /// </summary>
        public override int MaxDaysBack => 15;

        /// <summary>
        /// Load the days back from the store entity
        /// </summary>
        public override int LoadDaysBack(StoreEntity store)
        {
            return ((ThreeDCartStoreEntity) store).DownloadModifiedNumberOfDaysBack;
        }

        /// <summary>
        /// Save the days back to the store entity
        /// </summary>
        public override void SaveDaysBack(StoreEntity store, int daysBack)
        {
            ((ThreeDCartStoreEntity) store).DownloadModifiedNumberOfDaysBack = daysBack;
        }
    }
}
