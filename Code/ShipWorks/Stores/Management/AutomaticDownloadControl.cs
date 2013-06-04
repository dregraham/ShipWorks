using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// User control for editing automatic download settings
    /// </summary>
    public partial class AutomaticDownloadControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AutomaticDownloadControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the settings for the given store
        /// </summary>
        public void LoadStore(StoreEntity store)
        {
            StoreType storeType = StoreTypeManager.GetType(store);

            autoDownload.Checked = store.AutoDownload;
            autoDownloadMinutes.Value = (int) Math.Max(storeType.AutoDownloadMinimumMinutes, store.AutoDownloadMinutes);
            autoDownloadWhenAway.Checked = store.AutoDownloadOnlyAway;

            autoDownloadMinutes.Minimum = storeType.AutoDownloadMinimumMinutes;

            UpdateAutoDownloadUI();
        }

        /// <summary>
        /// Save the settings to the given entity. (Not to the database)
        /// </summary>
        public void SaveToStoreEntity(StoreEntity store)
        {
            store.AutoDownload = autoDownload.Checked;
            store.AutoDownloadMinutes = (int) autoDownloadMinutes.Value;
            store.AutoDownloadOnlyAway = autoDownloadWhenAway.Checked;
        }

        /// <summary>
        /// Update the auto download UI based on the state
        /// </summary>
        private void UpdateAutoDownloadUI()
        {
            autoDownloadMinutes.Enabled = autoDownload.Checked;
            autoDownloadWhenAway.Enabled = autoDownload.Checked;
        }

        /// <summary>
        /// The auto-download setting is changing.
        /// </summary>
        private void OnChangeAutoDownload(object sender, EventArgs e)
        {
            UpdateAutoDownloadUI();
        }
    }
}
