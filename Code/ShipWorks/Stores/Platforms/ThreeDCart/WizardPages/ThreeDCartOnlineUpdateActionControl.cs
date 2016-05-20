using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.ThreeDCart.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.ThreeDCart.WizardPages
{
    /// <summary>
    /// Control for creating online update actions in the Add Store wizard
    /// </summary>
    public partial class ThreeDCartOnlineUpdateActionControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartOnlineUpdateActionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize for the given store
        /// </summary>
        public override void UpdateForStore(StoreEntity store)
        {
            ThreeDCartStoreType storeType = (ThreeDCartStoreType) StoreTypeManager.GetType(store);

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
                ThreeDCartOrderUpdateTask orderUpdateTask = (ThreeDCartOrderUpdateTask) new ActionTaskDescriptorBinding(typeof(ThreeDCartOrderUpdateTask), store).CreateInstance();

                if (((ThreeDCartStoreEntity) store).RestUser)
                {
                    orderUpdateTask.StatusCode = (int) EnumHelper.GetEnumByApiValue<Enums.ThreeDCartOrderStatus>(status.Text);
                }
                else
                {
                    // Apply the desired status code
                    ThreeDCartStatusCodeProvider statusProvider = new ThreeDCartStatusCodeProvider((ThreeDCartStoreEntity)store);
                    orderUpdateTask.StatusCode = statusProvider.GetCodeValue(status.Text);
                }

                tasks.Add(orderUpdateTask);
            }

            // Create the shipment upload task
            ThreeDCartShipmentUploadTask shipmentUploadTask = (ThreeDCartShipmentUploadTask) new ActionTaskDescriptorBinding(typeof(ThreeDCartShipmentUploadTask), store).CreateInstance();

            tasks.Add(shipmentUploadTask);

            return tasks;
        }
    }
}
