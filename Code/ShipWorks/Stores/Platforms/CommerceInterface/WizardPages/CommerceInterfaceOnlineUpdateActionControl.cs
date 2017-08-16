using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Autofac;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.CommerceInterface.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.CommerceInterface.WizardPages
{
    /// <summary>
    /// Control for configuring tasks during the setup wizard
    /// </summary>
    public partial class CommerceInterfaceOnlineUpdateActionControl : OnlineUpdateActionControlBase
    {
        public CommerceInterfaceOnlineUpdateActionControl()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Initialize the UI for the given store
        /// </summary>
        public override void UpdateForStore(StoreEntity store)
        {
            comboStatus.DisplayMember = "Key";
            comboStatus.ValueMember = "Value";

            object current = comboStatus.SelectedValue;

            GenericStoreStatusCodeProvider statusProvider = ((GenericModuleStoreType) StoreTypeManager.GetType(store)).CreateStatusCodeProvider();
            comboStatus.DataSource = statusProvider.CodeValues.Select(c => new KeyValuePair<string, object>(statusProvider.GetCodeName(c), c)).ToList();

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
            }
        }

        /// <summary>
        /// Toggle status
        /// </summary>
        private void OnUploadCheckedChanged(object sender, EventArgs e)
        {
            comboStatus.Enabled = shipmentUpdate.Checked;
        }

        /// <summary>
        /// Create the tasks the user has selected from the UI
        /// </summary>
        public override List<ShipWorks.Actions.Tasks.ActionTask> CreateActionTasks(ILifetimeScope lifetimeScope, StoreEntity store)
        {
            List<ActionTask> tasks = new List<ActionTask>();

            if (shipmentUpdate.Checked)
            {
                // create the task
                CommerceInterfaceShipmentUploadTask task = (CommerceInterfaceShipmentUploadTask) new ActionTaskDescriptorBinding(typeof(CommerceInterfaceShipmentUploadTask), store).CreateInstance(lifetimeScope);
                task.StatusCode = Convert.ToInt32(comboStatus.SelectedValue);

                tasks.Add(task);
            }

            return tasks;
        }
    }
}
