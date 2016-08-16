using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Tasks;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.GenericModule.WizardPages
{
    /// <summary>
    /// User control for configuring online update actions for generic stores
    /// </summary>
    public partial class GenericStoreModuleActionControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStoreModuleActionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the UI for the given store
        /// </summary>
        [NDependIgnoreLongMethod]
        public override void UpdateForStore(StoreEntity store)
        {
            GenericModuleStoreEntity generic = (GenericModuleStoreEntity) store;

            // Update shipment details?
            panelShipmentUpdate.Visible = generic.ModuleOnlineShipmentDetails;

            // Status support
            GenericOnlineStatusSupport statusSupport = (GenericOnlineStatusSupport) generic.ModuleOnlineStatusSupport;

            // See if its available
            if (statusSupport == GenericOnlineStatusSupport.StatusOnly ||
                statusSupport == GenericOnlineStatusSupport.StatusWithComment)
            {
                panelOrderStatus.Visible = true;

                comboStatus.DisplayMember = "Key";
                comboStatus.ValueMember = "Value";

                labelComment.Visible = statusSupport == GenericOnlineStatusSupport.StatusWithComment;
                commentToken.Visible = statusSupport == GenericOnlineStatusSupport.StatusWithComment;

                object current = comboStatus.SelectedValue;

                GenericStoreStatusCodeProvider statusProvider = ((GenericModuleStoreType) StoreTypeManager.GetType(store)).CreateStatusCodeProvider();
                List<KeyValuePair<string, object>> statuses = statusProvider.CodeValues.Select(c => new KeyValuePair<string, object>(statusProvider.GetCodeName(c), c)).ToList();

                // Set before we add the please select an order status entry.
                bool hasStatuses = statuses.Any();

                KeyValuePair<string, object> pleaseSelectStatus = new KeyValuePair<string, object>("Please select an order status.", -1);
                statuses.Insert(0, pleaseSelectStatus);

                int selectedIndex = 0;

                // Try to revert back to what was selected before
                if (current != null)
                {
                    comboStatus.SelectedValue = current;
                    selectedIndex = comboStatus.SelectedIndex;
                }
                else if (hasStatuses)
                {
                    KeyValuePair<string, object> shippedKeyValuePair = statuses.FirstOrDefault(kv => String.Equals(kv.Key, "Shipped", StringComparison.OrdinalIgnoreCase));

                    // If we can't find a "shipped" status, add an list item asking the user to select a status
                    if (shippedKeyValuePair.Equals(default(KeyValuePair<string, object>)))
                    {
                        selectedIndex = 0;
                    }
                    else
                    {
                        // We found a shipped status, so use it.
                        selectedIndex = statuses.IndexOf(shippedKeyValuePair);
                    }

                    if (selectedIndex < 0)
                    {
                        selectedIndex = 0;
                    }

                    statusUpdate.Enabled = true;
                }
                else
                {
                    statusUpdate.Enabled = false;
                    statusUpdate.Checked = false;
                }

                if (!commentToken.Visible)
                {
                    Height = panelOrderStatus.Top + commentToken.Top;
                }
                else
                {
                    Height = panelOrderStatus.Bottom;
                }
                // If there were any statuses to chose from, set the combo data source and selected index.
                if (hasStatuses)
                {
                    comboStatus.DataSource = statuses;
                    comboStatus.SelectedIndex = selectedIndex;
                }
            }
            else
            {
                panelOrderStatus.Visible = false;
                Height = panelOrderStatus.Top + comboStatus.Top;
            }
        }

        /// <summary>
        /// The checkbox has changed for status updates
        /// </summary>
        private void OnStatusUpdateCheckChanged(object sender, EventArgs e)
        {
            comboStatus.Enabled = statusUpdate.Checked;
            labelComment.Enabled = statusUpdate.Checked;
            commentToken.Enabled = statusUpdate.Checked;
        }

        /// <summary>
        /// Create the tasks the user has selected from the UI
        /// </summary>
        public override List<ActionTask> CreateActionTasks(StoreEntity store)
        {
            // Validate settings, and throw if they are not valid.
            ValidateUi(DoesStoreSupportOnlineStatusUpdates((GenericModuleStoreEntity)store));

            List<ActionTask> tasks = new List<ActionTask>();

            // Shipment update task
            if (panelShipmentUpdate.Visible && shipmentUpdate.Checked)
            {
                tasks.Add(new ActionTaskDescriptorBinding(typeof(GenericStoreShipmentUploadTask), store).CreateInstance());
            }

            // Order status update
            if (panelOrderStatus.Visible && statusUpdate.Checked)
            {
                GenericStoreOrderUpdateTask statusTask = (GenericStoreOrderUpdateTask) new ActionTaskDescriptorBinding(typeof(GenericStoreOrderUpdateTask), store).CreateInstance();
                statusTask.StatusCode = comboStatus.SelectedValue.ToString();

                if (commentToken.Visible)
                {
                    statusTask.Comment = commentToken.Text;
                }
                else
                {
                    statusTask.Comment = "";
                }

                tasks.Add(statusTask);
            }

            return tasks;
        }

        /// <summary>
        /// Check to make sure settings are valid.  If any are not, an OnlineUpdateActionCreateException is thrown.
        /// </summary>
        private void ValidateUi(bool supportsOnlineStatusUpdates)
        {
            if (supportsOnlineStatusUpdates && statusUpdate.Checked && comboStatus.SelectedIndex <= 0)
            {
                throw new OnlineUpdateActionCreateException("Please select an order status for shipped orders.\n\nNormally this is a 'Shipped' or 'Completed' status.");
            }
        }

        /// <summary>
        /// Does the specified store support online status updates
        /// </summary>
        private static bool DoesStoreSupportOnlineStatusUpdates(GenericModuleStoreEntity store)
        {
            return store.ModuleOnlineStatusSupport == (int) GenericOnlineStatusSupport.StatusOnly ||
                   store.ModuleOnlineStatusSupport == (int) GenericOnlineStatusSupport.StatusWithComment;
        }
    }
}
