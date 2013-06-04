﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.AmeriCommerce.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.AmeriCommerce.WizardPages
{
    /// <summary>
    /// Control for creating online update actions in the Add Store wizard
    /// </summary>
    public partial class AmeriCommerceOnlineUpdateActionControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceOnlineUpdateActionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize for the given store
        /// </summary>
        public override void UpdateForStore(StoreEntity store)
        {
            AmeriCommerceStoreType storeType = (AmeriCommerceStoreType) StoreTypeManager.GetType(store);

            string current = status.SelectedItem as string;

            // Add the items
            status.Items.Clear();
            status.Items.AddRange(storeType.GetOnlineStatusChoices().ToArray());

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

            if (setStatus.Checked)
            {
                // Create the task instance
                AmeriCommerceOrderUpdateTask task = (AmeriCommerceOrderUpdateTask) new ActionTaskDescriptorBinding(typeof(AmeriCommerceOrderUpdateTask), store).CreateInstance();
           
                // Apply the desired status code
                AmeriCommerceStatusCodeProvider statusProvider = new AmeriCommerceStatusCodeProvider((AmeriCommerceStoreEntity) store);
                task.StatusCode = (int) statusProvider.GetCodeValue(status.Text);

                tasks.Add(task);
            }

            if (uploadTracking.Checked)
            {
                AmeriCommerceShipmentUploadTask task = (AmeriCommerceShipmentUploadTask) new ActionTaskDescriptorBinding(typeof(AmeriCommerceShipmentUploadTask), store).CreateInstance();

                tasks.Add(task);
            }

            return tasks;
        }
    }
}
