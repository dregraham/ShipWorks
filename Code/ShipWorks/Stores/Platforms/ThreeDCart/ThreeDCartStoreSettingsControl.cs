using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Control for configuring non-credential settings for ThreeDCart
    /// </summary>
    public partial class ThreeDCartStoreSettingsControl : StoreSettingsControlBase
    {
        ThreeDCartStoreEntity threeDCartStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartStoreSettingsControl()            
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the UI from the store entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            threeDCartStore = (ThreeDCartStoreEntity) store;

            // If this is the first time loading a converted legacy store, the TimeZoneID will be null.
            // So we will assume that the customer has never changed the time zone using the 3dcart admin,
            // and that it is still the default of Eastern Standard Time.
            if (string.IsNullOrWhiteSpace(threeDCartStore.TimeZoneID))
            {
                threeDCartStore.TimeZoneID = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").Id;
            }

            // timezone
            timeZoneControl.SelectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById(threeDCartStore.TimeZoneID);

            threeDCartDownloadCriteriaControl.LoadStore(store);
        }

        /// <summary>
        /// Saves the UI to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            threeDCartStore = (ThreeDCartStoreEntity) store;

            // timezone
            threeDCartStore.TimeZoneID = timeZoneControl.SelectedTimeZone.Id;

            return threeDCartDownloadCriteriaControl.SaveToEntity(store);
        }
    }
}
