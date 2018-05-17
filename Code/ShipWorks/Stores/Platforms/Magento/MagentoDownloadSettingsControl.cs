using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Magento
{
    public partial class MagentoDownloadSettingsControl : UserControl, IDownloadSettingsControl
    {
        private MagentoStoreEntity magentoStore;

        public MagentoDownloadSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the store
        /// </summary>
        public void LoadStore(StoreEntity store)
        {
            downloadSettingsControl.LoadStore(store);

            magentoStore = (MagentoStoreEntity) store;

            downloadOrderStatus.Checked = magentoStore.UpdateSplitOrderOnlineStatus;
        }

        /// <summary>
        /// Save the settings to the store
        /// </summary>
        public void Save()
        {
            downloadSettingsControl.Save();
        }

        private void OnDownloadOrderStatusCheckedChanged(object sender, EventArgs e)
        {
            magentoStore.UpdateSplitOrderOnlineStatus = downloadOrderStatus.Checked;
        }
    }
}
