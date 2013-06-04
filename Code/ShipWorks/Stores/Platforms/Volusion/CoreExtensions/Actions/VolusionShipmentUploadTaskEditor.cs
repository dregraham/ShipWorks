using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Stores.Platforms.Volusion.CoreExtensions.Actions
{
    /// <summary>
    /// Task Editor for configuring the volusion shipmetn detials upload
    /// </summary>
    public partial class VolusionShipmentUploadTaskEditor : ActionTaskEditor
    {
        // task being configured
        VolusionShipmentUploadTask task;

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionShipmentUploadTaskEditor(VolusionShipmentUploadTask task)
        {
            InitializeComponent();

            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            this.task = task;
        }

        /// <summary>
        /// Load the UI
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            sendEmailCheckBox.Checked = task.SendEmail;
        }

        /// <summary>
        /// Toggles the Send Email option
        /// </summary>
        private void OnSendEmailChecked(object sender, EventArgs e)
        {
            task.SendEmail = sendEmailCheckBox.Checked;
        }
    }
}
