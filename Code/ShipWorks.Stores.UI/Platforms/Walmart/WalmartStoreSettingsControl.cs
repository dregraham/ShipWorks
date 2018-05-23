using System;
using System.Linq;
using Interapptive.Shared.UI;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI.Controls.StoreSettings;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.UI.Platforms.Walmart
{
    /// <summary>
    /// Store settings control for Walmart
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Management.StoreSettingsControlBase" />
    [KeyedComponent(typeof(StoreSettingsControlBase), StoreTypeCode.Walmart)]
    public class WalmartStoreSettingsControl : DownloadModifiedDaysBackControl
    {
        /// <summary>
        /// Max number of days back allowed
        /// </summary>
        public override int MaxDaysBack => 30;

        /// <summary>
        /// Load the days back from the store entity
        /// </summary>
        public override int LoadDaysBack(StoreEntity store) =>
            ((WalmartStoreEntity) store).DownloadModifiedNumberOfDaysBack;

        /// <summary>
        /// Save the days back to the store entity
        /// </summary>
        public override void SaveDaysBack(StoreEntity store, int daysBack) =>
            ((WalmartStoreEntity) store).DownloadModifiedNumberOfDaysBack = daysBack;
    }
}
