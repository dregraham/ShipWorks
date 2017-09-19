using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.NetworkSolutions.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.WizardPages
{
    /// <summary>
    /// Control for creating online update actions in the Add Store wizard
    /// </summary>
    public partial class NetworkSolutionsOnlineUpdateActionControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsOnlineUpdateActionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize for the given store
        /// </summary>
        public override void UpdateForStore(StoreEntity store)
        {
            NetworkSolutionsStoreType storeType = (NetworkSolutionsStoreType) StoreTypeManager.GetType(store);

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
                if (status.Items.Contains("Shipped"))
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
        public override List<ActionTask> CreateActionTasks(ILifetimeScope lifetimeScope, StoreEntity store)
        {
            List<ActionTask> tasks = new List<ActionTask>();

            if (setStatus.Checked)
            {
                // Create the task instance
                NetworkSolutionsOrderUpdateTask task = (NetworkSolutionsOrderUpdateTask) new ActionTaskDescriptorBinding(typeof(NetworkSolutionsOrderUpdateTask), store).CreateInstance(lifetimeScope);

                // Apply the desired status code
                NetworkSolutionsStatusCodeProvider statusProvider = new NetworkSolutionsStatusCodeProvider((NetworkSolutionsStoreEntity) store);
                task.StatusCode = (long) statusProvider.GetCodeValue(status.Text);

                tasks.Add(task);
            }

            if (uploadTracking.Checked)
            {
                NetworkSolutionsShipmentUploadTask task = (NetworkSolutionsShipmentUploadTask) new ActionTaskDescriptorBinding(typeof(NetworkSolutionsShipmentUploadTask), store).CreateInstance(lifetimeScope);

                tasks.Add(task);
            }

            return tasks;
        }
    }
}
