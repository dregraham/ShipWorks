using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls.StoreSettings;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Channel Advisor implementation of DownloadModifiedDaysBackControl
    /// </summary>
    public class ChannelAdvisorDownloadModifiedDaysBackControl : DownloadModifiedDaysBackControl
    {
        /// <summary>
        /// Max number of days back allowed
        /// </summary>
        public override int MaxDaysBack => 7;

        /// <summary>
        /// Load the days back from the store entity
        /// </summary>
        public override int LoadDaysBack(StoreEntity store)
        {
            return ((ChannelAdvisorStoreEntity) store).DownloadModifiedNumberOfDaysBack;
        }

        /// <summary>
        /// Save the days back to the store entity
        /// </summary>
        public override void SaveDaysBack(StoreEntity store, int daysBack)
        {
            ((ChannelAdvisorStoreEntity) store).DownloadModifiedNumberOfDaysBack = daysBack;
        }
    }
}
