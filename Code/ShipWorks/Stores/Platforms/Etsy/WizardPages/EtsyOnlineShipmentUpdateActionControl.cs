using System;
using System.Collections.Generic;
using Autofac;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Etsy.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.Etsy.WizardPages
{
    /// <summary>
    /// Action to update Etsy shipping information.
    /// </summary>
    public partial class EtsyOnlineShipmentUpdateActionControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyOnlineShipmentUpdateActionControl()
        {
            InitializeComponent();

            MakeEnabledOrDisabled();
        }

        /// <summary>
        /// Create the tasks the user has selected from the UI
        /// </summary>
        public override List<ActionTask> CreateActionTasks(ILifetimeScope lifetimeScope, StoreEntity store)
        {
            List<ActionTask> tasks = new List<ActionTask>();

            // Shipment update task
            if (paidCheckBox.Checked || shippedCheckBox.Checked)
            {
                EtsyUploadTask statusTask = (EtsyUploadTask) new ActionTaskDescriptorBinding(typeof(EtsyUploadTask), store).CreateInstance(lifetimeScope);

                statusTask.WithComment = shippedCheckBox.Checked && withComment.Checked;
                statusTask.Comment = shippedCheckBox.Checked ? tokenBox.Text : string.Empty;

                statusTask.MarkAsShipped = shippedCheckBox.Checked;
                statusTask.MarkAsPaid = paidCheckBox.Checked;

                tasks.Add(statusTask);
            }

            return tasks;
        }

        /// <summary>
        /// Disable comment box if withComment is unchecked.
        /// </summary>
        private void OnWithCommentCheckedChanged(object sender, EventArgs e)
        {
            MakeEnabledOrDisabled();
        }

        /// <summary>
        /// Handles ShippedCheckBoxCheckChanged.
        /// </summary>
        private void OnShippedCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            MakeEnabledOrDisabled();
        }

        /// <summary>
        /// Enables and Disables controls based on checkbox states
        /// </summary>
        private void MakeEnabledOrDisabled()
        {
            tokenBox.Enabled = withComment.Checked && shippedCheckBox.Checked;
            withComment.Enabled = shippedCheckBox.Checked;
        }
    }
}