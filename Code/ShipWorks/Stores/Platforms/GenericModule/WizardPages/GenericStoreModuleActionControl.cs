using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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

                GenericStoreStatusCodeProvider statusProvider =
                    ((GenericModuleStoreType) StoreTypeManager.GetType(store)).CreateStatusCodeProvider();
                comboStatus.DataSource =
                    statusProvider.CodeValues.Select(
                        c => new KeyValuePair<string, object>(statusProvider.GetCodeName(c), c)).ToList();

                // Try to revert back to what was selected before
                if (current != null)
                {
                    comboStatus.SelectedValue = current;
                }

                if (comboStatus.Items.Count > 0)
                {
                    if (comboStatus.SelectedIndex < 0)
                    {
                        comboStatus.SelectedIndex = 0;
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
    }
}
