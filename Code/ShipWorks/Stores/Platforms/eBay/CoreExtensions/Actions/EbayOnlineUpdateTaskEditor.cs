using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Actions
{
    /// <summary>
    /// UI for the Ebay Online Update Task
    /// </summary>
    public partial class EbayOnlineUpdateTaskEditor : ActionTaskEditor
    {
        // the task being edited
        EbayOnlineUpdateTask task;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayOnlineUpdateTaskEditor(EbayOnlineUpdateTask task)
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
            shippedCheckBox.Checked = task.MarkShipped;
            paidCheckBox.Checked = task.MarkPaid;
        }

        /// <summary>
        /// Shipped has been toggled
        /// </summary>
        private void OnShippedCheckedChanged(object sender, EventArgs e)
        {
            task.MarkShipped = shippedCheckBox.Checked;
        }

        /// <summary>
        /// Paid as toggled
        /// </summary>
        private void OnPaidCheckedChanged(object sender, EventArgs e)
        {
            task.MarkPaid = paidCheckBox.Checked;
        }
    }
}
