using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.LemonStand;
using ShipWorks.Stores.Platforms.LemonStand.CoreExtensions.Actions;

namespace ShipWorks.Stores.UI.Platforms.LemonStand
{
    /// <summary>
    /// Control for creating online update actions in the Add Store wizard
    /// </summary>
    public partial class LemonStandOnlineUpdateActionControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LemonStandOnlineUpdateActionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize for the given store
        /// </summary>
        public override void UpdateForStore(StoreEntity store)
        {
            LemonStandStoreType storeType = (LemonStandStoreType) StoreTypeManager.GetType(store);

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
                if (status.Items.Contains("Shipped"))
                {
                    status.SelectedItem = "Shipped";
                }
                else if (status.Items.Contains("Complete"))
                {
                    status.SelectedItem = "Complete";
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
            LemonStandShipmentUploadTask shipmentUploadTask = (LemonStandShipmentUploadTask)new ActionTaskDescriptorBinding(typeof(LemonStandShipmentUploadTask), store).CreateInstance();
            tasks.Add(shipmentUploadTask);

            if (setStatus.Checked)
            {
                // Create the task instance
                LemonStandOrderUpdateTask orderUpdateTask = (LemonStandOrderUpdateTask)new ActionTaskDescriptorBinding(typeof(LemonStandOrderUpdateTask), store).CreateInstance();

                // Apply the desired status code
                LemonStandStatusCodeProvider statusProvider = new LemonStandStatusCodeProvider((LemonStandStoreEntity)store);
                orderUpdateTask.StatusCode = statusProvider.GetCodeValue(status.Text);

                tasks.Add(orderUpdateTask);
            }

            return tasks;
        }
    }
}
