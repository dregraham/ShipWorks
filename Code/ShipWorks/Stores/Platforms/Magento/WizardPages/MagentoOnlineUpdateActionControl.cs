using System;
using System.Collections.Generic;
using Autofac;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Magento.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.Magento.WizardPages
{
    /// <summary>
    /// Control for creating online update actions from the add store wizard
    /// </summary>
    public partial class MagentoOnlineUpdateActionControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoOnlineUpdateActionControl(bool hideEmail)
        {
            InitializeComponent();

            if (hideEmail)
            {
                sendEmail.Hide();
                labelEmail.Hide();
            }
        }

        /// <summary>
        /// Create tasks based on what the user has selected
        /// </summary>
        public override List<ActionTask> CreateActionTasks(ILifetimeScope lifetimeScope, StoreEntity store)
        {
            if (shipmentUpdate.Checked)
            {
                MagentoShipmentUploadTask task = (MagentoShipmentUploadTask) new ActionTaskDescriptorBinding(typeof(MagentoShipmentUploadTask), store).CreateInstance(lifetimeScope);
                task.Comment = commentToken.Text;
                task.MagentoSendEmail = sendEmail.Checked;

                return new List<ActionTask> { task };
            }

            return null;
        }

        /// <summary>
        /// Change if the task is enabled
        /// </summary>
        private void OnChangeTaskEnabled(object sender, EventArgs e)
        {
            labelComments.Enabled = shipmentUpdate.Checked;
            commentToken.Enabled = shipmentUpdate.Checked;

            labelEmail.Enabled = shipmentUpdate.Checked;
            sendEmail.Enabled = shipmentUpdate.Checked;
        }
    }
}
