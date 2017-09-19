using System;
using System.Windows.Forms;
using Autofac;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email.Accounts;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// UserControl for loading the automated actions to do when processing\voiding shipments
    /// </summary>
    public partial class ShipmentAutomationControl : UserControl
    {
        ActionEntity actionEmail;
        ActionEntity actionStatusShipped;
        ActionEntity actionStatusVoided;

        EmailTask emailTask;
        SetOrderStatusTask shipStatusTask;
        SetOrderStatusTask voidStatusTask;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentAutomationControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Ensure the control is initialized.  If it has already been initialized, nothing is done.
        /// </summary>
        public void EnsureInitialized(ILifetimeScope lifetimeScope, ShipmentTypeCode code)
        {
            Cursor.Current = Cursors.WaitCursor;

            // Lazy load the builtin actions from the database
            if (actionEmail == null)
            {
                emailActionTemplate.LoadTemplates();

                // Load the actions from the database.  They will be created if they do not yet exist
                actionEmail = ShippingActionUtility.GetEmailAction(code);
                actionStatusShipped = ShippingActionUtility.GetShipStatusAction(code);
                actionStatusVoided = ShippingActionUtility.GetVoidStatusAction(code);

                // Load the checkbox UI state
                emailActionBox.Checked = actionEmail.Enabled;
                shipActionBox.Checked = actionStatusShipped.Enabled;
                voidActionBox.Checked = actionStatusVoided.Enabled;

                // Load the email ui
                emailTask = (EmailTask) ActionManager.LoadTasks(lifetimeScope, actionEmail)[0];
                emailActionTemplate.SelectedTemplateID = emailTask.TemplateID;
                delayDelivery.Checked = emailTask.DelayDelivery;
                delayTimeOfDay.Value = new DateTime(2000, 1, 1, emailTask.DelayTimeOfDay.Hours, emailTask.DelayTimeOfDay.Minutes, 0);
                UpdateEmailActionUI();

                // Load the ship status UI
                shipStatusTask = (SetOrderStatusTask) ActionManager.LoadTasks(lifetimeScope, actionStatusShipped)[0];
                shipActionStatus.Text = shipStatusTask.Status;

                // Load the void status UI
                voidStatusTask = (SetOrderStatusTask) ActionManager.LoadTasks(lifetimeScope, actionStatusVoided)[0];
                voidActionStatus.Text = voidStatusTask.Status;
            }
        }

        /// <summary>
        /// Save the settings the user has edited in the control
        /// </summary>
        public void SaveSettings()
        {
            if (actionEmail == null)
            {
                return;
            }

            // Email action enabled and settings
            actionEmail.Enabled = emailActionBox.Checked;
            emailTask.TemplateID = emailActionTemplate.SelectedTemplateID ?? 0;
            emailTask.DelayDelivery = delayDelivery.Checked;
            emailTask.DelayTimeOfDay = delayTimeOfDay.Value.TimeOfDay;

            // Ship status action enabled and settings
            actionStatusShipped.Enabled = shipActionBox.Checked;
            shipStatusTask.Status = shipActionStatus.Text;

            // Void action enabled and settings
            actionStatusVoided.Enabled = voidActionBox.Checked;
            voidStatusTask.Status = voidActionStatus.Text;

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                adapter.SaveAndRefetch(actionEmail);
                adapter.SaveAndRefetch(actionStatusShipped);
                adapter.SaveAndRefetch(actionStatusVoided);

                emailTask.Save(actionEmail, adapter);
                shipStatusTask.Save(actionStatusShipped, adapter);
                voidStatusTask.Save(actionStatusVoided, adapter);

                adapter.Commit();
            }

            ActionManager.CheckForChangesNeeded();
        }

        /// <summary>
        /// Changing the enabledness of the email action
        /// </summary>
        private void OnChangeActionEmail(object sender, EventArgs e)
        {
            labelTemplate.Enabled = emailActionBox.Checked;
            emailActionTemplate.Enabled = emailActionBox.Checked;

            delayDelivery.Enabled = emailActionBox.Checked;
            delayTimeOfDay.Enabled = emailActionBox.Checked && delayDelivery.Checked;
        }

        /// <summary>
        /// Changing the checkedness of the processing action
        /// </summary>
        private void OnChangeActionProcessing(object sender, EventArgs e)
        {
            labelShipStatus.Enabled = shipActionBox.Checked;
            shipActionStatus.Enabled = shipActionBox.Checked;
        }

        /// <summary>
        /// Changing the checkedness of the voiding action
        /// </summary>
        private void OnChangeActionVoiding(object sender, EventArgs e)
        {
            labelVoidStatus.Enabled = voidActionBox.Checked;
            voidActionStatus.Enabled = voidActionBox.Checked;
        }

        /// <summary>
        /// Changing the setting to delay email delivery
        /// </summary>
        private void OnChangeEmailDelayDelivery(object sender, EventArgs e)
        {
            OnChangeActionEmail(this, EventArgs.Empty);
        }

        /// <summary>
        /// Setup email
        /// </summary>
        private void OnEmailAccounts(object sender, EventArgs e)
        {
            using (EmailAccountManagerDlg dlg = new EmailAccountManagerDlg())
            {
                dlg.ShowDialog(this);
            }

            UpdateEmailActionUI();
        }

        /// <summary>
        /// Update the email action UI depending on if email accounts exist
        /// </summary>
        private void UpdateEmailActionUI()
        {
            panelEmailSettings.Visible = EmailAccountManager.EmailAccounts.Count > 0;
            panelEmailSetup.Visible = EmailAccountManager.EmailAccounts.Count == 0;
        }

    }
}
