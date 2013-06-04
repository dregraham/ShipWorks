using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Volusion.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.Volusion.WizardPages
{
    /// <summary>
    /// Control for configuring actions to be created during the Add Store wizard.
    /// </summary>
    public partial class VolusionOnlineUpdateActionControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionOnlineUpdateActionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create the tasks that were configured on the control
        /// </summary>
        public override List<ActionTask> CreateActionTasks(StoreEntity store)
        {
            if (createTask.Checked)
            {
                VolusionShipmentUploadTask task = (VolusionShipmentUploadTask)new ActionTaskDescriptorBinding(typeof(VolusionShipmentUploadTask), store).CreateInstance();
                task.SendEmail = sendEmail.Checked;

                return new List<ActionTask> { task };
            }

            return null;
        }

        /// <summary>
        /// Enable disable task options.
        /// </summary>
        private void OnCreateTaskChecked(object sender, EventArgs e)
        {
            sendEmail.Enabled = createTask.Checked;
        }
    }
}
