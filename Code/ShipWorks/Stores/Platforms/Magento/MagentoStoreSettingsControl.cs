using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Magento.Enums;

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

            if (magentoStore.MagentoVersion == (int) MagentoVersion.MagentoTwo)
            {
                sectionHeader.Hide();
                sendMailCheckBox.Hide();
                Height = 0;
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
