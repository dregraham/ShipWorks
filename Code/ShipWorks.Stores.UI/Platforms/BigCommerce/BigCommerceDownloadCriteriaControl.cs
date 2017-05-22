﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Enums;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.UI.Platforms.BigCommerce
{
    /// <summary>
    /// Control for configuring non-credential settings for BigCommerce
    /// </summary>
    public partial class BigCommerceDownloadCriteriaControl : StoreSettingsControlBase
    {
        BigCommerceStoreEntity bigCommerceStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceDownloadCriteriaControl()            
        {
            InitializeComponent();

            for (int i = 1; i < 15; i++)
            {
                downloadModifiedOrdersNumberOfDays.Items.Add(i);
            }
        }

        /// <summary>
        /// Load the UI from the store entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            bigCommerceStore = (BigCommerceStoreEntity)store;


            if (bigCommerceStore.DownloadModifiedNumberOfDaysBack > 0)
            {
                checkBoxDownloadModifiedOrders.Checked = true;
                downloadModifiedOrdersNumberOfDays.SelectedItem = bigCommerceStore.DownloadModifiedNumberOfDaysBack;
            }
            else
            {
                checkBoxDownloadModifiedOrders.Checked = false;
                downloadModifiedOrdersNumberOfDays.SelectedItem = null;
            }

            downloadModifiedOrdersNumberOfDays.Enabled = checkBoxDownloadModifiedOrders.Checked;
            labelDownloadModifiedNumberOfDays.Enabled = checkBoxDownloadModifiedOrders.Checked;
        }

        /// <summary>
        /// Saves the UI to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            bigCommerceStore = (BigCommerceStoreEntity) store;

            if (checkBoxDownloadModifiedOrders.Checked && downloadModifiedOrdersNumberOfDays.SelectedItem == null)
            {
                MessageHelper.ShowError(this, "Please select a number of days back to check.");
                return false;
            }

            if (!checkBoxDownloadModifiedOrders.Checked)
            {
                bigCommerceStore.DownloadModifiedNumberOfDaysBack = 0;
            }
            else
            {
                // Number of days of modified orders to download
                bigCommerceStore.DownloadModifiedNumberOfDaysBack =
                    (int) downloadModifiedOrdersNumberOfDays.SelectedItem;
            }

            return true;
        }

        private void OnCheckBoxDownloadModifiedOrdersCheckedChanged(object sender, EventArgs e)
        {
            downloadModifiedOrdersNumberOfDays.Enabled = checkBoxDownloadModifiedOrders.Checked;
            labelDownloadModifiedNumberOfDays.Enabled = checkBoxDownloadModifiedOrders.Checked;
        }

    }
}
