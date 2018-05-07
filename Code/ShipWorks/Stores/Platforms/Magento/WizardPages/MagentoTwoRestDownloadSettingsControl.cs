using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Magento.WizardPages
{
    /// <summary>
    /// Control for creating online update actions from the add store wizard
    /// </summary>
    public partial class MagentoTwoRestDownloadSettingsControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoTwoRestDownloadSettingsControl()
        {
            InitializeComponent();
        }

        public void Save(object sender, EventArgs e)
        {
            MagentoStoreEntity store = new MagentoStoreEntity();

            if (downloadOrderStatus.Checked)
            {
                store.UpdateSplitOrderOnlineStatus = true;
            }
        }

        public void Load(MagentoStoreEntity store)
        {
            if (store.UpdateSplitOrderOnlineStatus)
            {
                downloadOrderStatus.Checked = true;
            }
        }
    }
}
