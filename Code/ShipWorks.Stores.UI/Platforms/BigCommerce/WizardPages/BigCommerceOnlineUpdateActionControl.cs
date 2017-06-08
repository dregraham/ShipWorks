﻿using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Actions.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Stores.Platforms.BigCommerce.CoreExtensions.Actions;

namespace ShipWorks.Stores.UI.Platforms.BigCommerce.WizardPages
{
    /// <summary>
    /// Control for creating online update actions in the Add Store wizard
    /// </summary>
    [KeyedComponent(typeof(OnlineUpdateActionControlBase), StoreTypeCode.BigCommerce, ExternallyOwned = true)]
    public partial class BigCommerceOnlineUpdateActionControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceOnlineUpdateActionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize for the given store
        /// </summary>
        public override void UpdateForStore(StoreEntity store)
        {
            BigCommerceStoreType storeType = (BigCommerceStoreType) StoreTypeManager.GetType(store);

            string current = status.SelectedItem as string;

            // Add the items
            status.Items.Clear();
            status.Items.AddRange(storeType.GetOnlineStatusChoices().ToArray<object>());

            if (!string.IsNullOrEmpty(current))
            {
                status.SelectedItem = current;
            }

            if (status.SelectedIndex < 0)
            {
                // Default the selection
                if (status.Items.Contains("Complete"))
                {
                    status.SelectedItem = "Complete";
                }
                else if (status.Items.Contains("Shipped"))
                {
                    status.SelectedItem = "Shipped";
                }
            }

            if (status.Items.Count > 0)
            {
                if (status.SelectedIndex < 0)
                {
                    status.SelectedIndex = 0;
                }

                setStatus.Enabled = true;
            }
            else
            {
                setStatus.Enabled = false;
                setStatus.Checked = false;
            }
        }

        /// <summary>
        /// Selection for setting the order status is changing
        /// </summary>
        private void OnChangeSetOrderStatus(object sender, EventArgs e)
        {
            status.Enabled = setStatus.Checked;
        }

        /// <summary>
        /// Create the configured tasks
        /// </summary>
        public override List<ActionTask> CreateActionTasks(StoreEntity store)
        {
            List<ActionTask> tasks = new List<ActionTask>();

            // Create the shipment upload task
            BigCommerceShipmentUploadTask shipmentUploadTask = (BigCommerceShipmentUploadTask) new ActionTaskDescriptorBinding(typeof(BigCommerceShipmentUploadTask), store).CreateInstance();
            tasks.Add(shipmentUploadTask);

            return tasks;
        }
    }
}
