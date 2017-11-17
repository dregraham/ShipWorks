using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Core.UI.Controls.StoreSettings;

namespace ShipWorks.Core.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Channel Advisor implementation of DownloadModifiedDaysBackControl
    /// </summary>
    public class ChannelAdvisorDownloadModifiedDaysBackControl : DownloadModifiedDaysBackControl
    {
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
