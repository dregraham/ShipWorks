using System;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls.StoreSettings;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Control for setting days back to check for modified orders for 3dCart
    /// </summary>
    public class ThreeDCartDownloadCriteriaControl : DownloadModifiedDaysBackControl
    {
        private const int HubDaysBack = 5;
        private const int NonHubDaysBack = 15;

        /// <summary>
        /// Max number of days back allowed
        /// </summary>
        public override int MaxDaysBack
        {
            get
            {
                using (var scope = IoC.BeginLifetimeScope())
                {
                    LicenseService licenseService = scope.Resolve<LicenseService>();

                    return !licenseService.IsHub ? NonHubDaysBack : HubDaysBack;
                }
            }
        }
        /// <summary>
        /// Load the days back from the store entity
        /// </summary>
        public override int LoadDaysBack(StoreEntity store)
        {
            /// Return the min value between MaxDaysBack and DownloadModifiedNumberOfDaysBack
            /// as the list varies between Hub and non-Hub customers. 
            return Math.Min(MaxDaysBack, ((ThreeDCartStoreEntity) store).DownloadModifiedNumberOfDaysBack);
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
