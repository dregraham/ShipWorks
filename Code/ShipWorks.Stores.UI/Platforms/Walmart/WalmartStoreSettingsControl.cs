using System;
using System.Linq;
using Interapptive.Shared.UI;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.UI.Platforms.Walmart
{
    /// <summary>
    /// Store settings control for Walmart
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Management.StoreSettingsControlBase" />
    [KeyedComponent(typeof(StoreSettingsControlBase), StoreTypeCode.Walmart)]
    public partial class WalmartStoreSettingsControl : StoreSettingsControlBase
    {
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartStoreSettingsControl"/> class.
        /// </summary>
        public WalmartStoreSettingsControl(IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            InitializeComponent();

            downloadModifiedOrdersNumberOfDays.Items.AddRange(Enumerable.Range(1, 30).Cast<object>().ToArray());
        }

        /// <summary>
        /// Load the store settings from the store entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            WalmartStoreEntity walmartStore = (WalmartStoreEntity) store;

            if (walmartStore.DownloadModifiedNumberOfDaysBack > 0)
            {
                checkBoxDownloadModifiedOrders.Checked = true;
                downloadModifiedOrdersNumberOfDays.SelectedItem = walmartStore.DownloadModifiedNumberOfDaysBack;
            }
            else
            {
                checkBoxDownloadModifiedOrders.Checked = false;
                downloadModifiedOrdersNumberOfDays.SelectedItem = null;
            }

            downloadModifiedOrdersNumberOfDays.Enabled = checkBoxDownloadModifiedOrders.Checked;
        }

        /// <summary>
        /// Saves the store settings to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            WalmartStoreEntity walmartStore = (WalmartStoreEntity) store;

            if (checkBoxDownloadModifiedOrders.Checked && downloadModifiedOrdersNumberOfDays.SelectedItem == null)
            {
                messageHelper.ShowError(this, "Please select a number of days back to check.");
                return false;
            }

            if (!checkBoxDownloadModifiedOrders.Checked)
            {
                walmartStore.DownloadModifiedNumberOfDaysBack = 0;
            }
            else
            {
                // Number of days of modified orders to download
                walmartStore.DownloadModifiedNumberOfDaysBack =
                    (int)downloadModifiedOrdersNumberOfDays.SelectedItem;
            }

            return true;
        }

        /// <summary>
        /// Enable or disable the days back combo box based on the check box value
        /// </summary>
        private void OnCheckBoxDownloadModifiedOrdersCheckedChanged(object sender, EventArgs e)
        {
            downloadModifiedOrdersNumberOfDays.Enabled = checkBoxDownloadModifiedOrders.Checked;
        }
    }
}
