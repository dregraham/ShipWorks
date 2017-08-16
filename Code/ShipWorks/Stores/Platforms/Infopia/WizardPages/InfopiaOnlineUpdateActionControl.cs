using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Infopia.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.Infopia.WizardPages
{
    /// <summary>
    /// Base for online update actions for the add store wizard
    /// </summary>
    public partial class InfopiaOnlineUpdateActionControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaOnlineUpdateActionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update the UI for the given infopia store
        /// </summary>
        public override void UpdateForStore(StoreEntity store)
        {
            // First time - we only have to do this once for infopia since the list is builtin to ShipWorks and
            // not pulled dynamically from the store like other stores.
            if (comboStatus.Items.Count == 0)
            {
                comboStatus.Items.AddRange(InfopiaUtility.StatusValues.ToArray());
                comboStatus.SelectedItem = "Processed";
            }

            if (comboStatus.SelectedIndex < 0)
            {
                comboStatus.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Create the tasks based on what the user has selected
        /// </summary>
        public override List<ActionTask> CreateActionTasks(ILifetimeScope lifetimeScope, StoreEntity store)
        {
            List<ActionTask> tasks = new List<ActionTask>();

            if (statusUpdate.Checked)
            {
                InfopiaOrderUpdateTask task = (InfopiaOrderUpdateTask) new ActionTaskDescriptorBinding(typeof(InfopiaOrderUpdateTask), store).CreateInstance(lifetimeScope);
                task.Status = comboStatus.SelectedItem.ToString();

                tasks.Add(task);
            }

            if (shipmentUpdate.Checked)
            {
                tasks.Add(new ActionTaskDescriptorBinding(typeof(InfopiaShipmentUploadTask), store).CreateInstance(lifetimeScope));
            }

            return tasks;
        }
    }
}
