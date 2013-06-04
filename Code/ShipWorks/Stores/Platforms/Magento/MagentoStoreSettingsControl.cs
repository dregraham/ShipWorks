using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// More magento configuration for store options that aren't account info-related
    /// </summary>
    public partial class MagentoStoreSettingsControl : StoreSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoStoreSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the store settings
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            MagentoStoreEntity magentoStore = store as MagentoStoreEntity;
            if (magentoStore == null)
            {
                throw new InvalidOperationException("A non-Magento store was passed to the Magento store settings control.");
            }

            sendMailCheckBox.Checked = magentoStore.MagentoTrackingEmails;
        }

        /// <summary>
        /// Save from UI
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            MagentoStoreEntity magentoStore = store as MagentoStoreEntity;
            if (magentoStore == null)
            {
                throw new InvalidOperationException("A non-Magento store was passed to the Magento store settings control.");
            }

            magentoStore.MagentoTrackingEmails = sendMailCheckBox.Checked;

            return true;
        }
    }
}
