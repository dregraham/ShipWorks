using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Window for editing the settings of an email account
    /// </summary>
    public partial class EmailAccountEditorDlg : Form
    {
        EmailAccountEntity emailAccount;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailAccountEditorDlg(EmailAccountEntity emailAccount)
        {
            InitializeComponent();

            if (emailAccount == null)
            {
                throw new ArgumentNullException("emailAccount");
            }

            this.emailAccount = emailAccount;

            // Can't be edited on entry
            if (emailAccount.IsDirty)
            {
                // We could get around this be manually saving off a copy of the fields, but for now 
                // we'll play it easy and throw this as a reminder.
                throw new InvalidOperationException("The entity cannot be dirty on entry to support Cancel.");
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            accountSettings.LoadSettings(emailAccount, false);

            LoadDeliverySettings();
        }

        /// <summary>
        /// Load the Delivery section of the UI
        /// </summary>
        private void LoadDeliverySettings()
        {
            autoSend.Checked = emailAccount.AutoSend;
            autoSendMinutes.Value = emailAccount.AutoSendMinutes;

            perConnectionLimit.Checked = emailAccount.LimitMessagesPerConnection;
            messageConnectionLimit.Value = emailAccount.LimitMessagesPerConnectionQuantity;

            perHourLimit.Checked = emailAccount.LimitMessagesPerHour;
            messageHourLimit.Value = emailAccount.LimitMessagesPerHourQuantity;

            intervalLimit.Checked = emailAccount.LimitMessageInterval;
            messageInterval.Value = emailAccount.LimitMessageIntervalSeconds;

            UpdateDeliveryUI();
        }

        /// <summary>
        /// Save the delivery settings from the UI to the entity
        /// </summary>
        private void SaveDeliverySettings()
        {
            emailAccount.AutoSend = autoSend.Checked;
            emailAccount.AutoSendMinutes = (int) autoSendMinutes.Value;

            emailAccount.LimitMessagesPerConnection = perConnectionLimit.Checked;
            emailAccount.LimitMessagesPerConnectionQuantity = (int) messageConnectionLimit.Value;

            emailAccount.LimitMessagesPerHour = perHourLimit.Checked;
            emailAccount.LimitMessagesPerHourQuantity = (int) messageHourLimit.Value;

            emailAccount.LimitMessageInterval = intervalLimit.Checked;
            emailAccount.LimitMessageIntervalSeconds = (int) messageInterval.Value;
        }

        /// <summary>
        /// Changing the option to autosend
        /// </summary>
        private void OnChangeAutoSend(object sender, EventArgs e)
        {
            UpdateDeliveryUI();
        }

        /// <summary>
        /// Changing if there is a per-connection message limit
        /// </summary>
        private void OnChangePerConnectionLimit(object sender, EventArgs e)
        {
            UpdateDeliveryUI();
        }

        /// <summary>
        /// Changing the option to pause between messages
        /// </summary>
        private void OnChangeUsePause(object sender, EventArgs e)
        {
            UpdateDeliveryUI();
        }

        /// <summary>
        /// Changing the option to limit how many messages to send in an hour
        /// </summary>
        private void OnChangeUseHourLimit(object sender, EventArgs e)
        {
            UpdateDeliveryUI();
        }

        /// <summary>
        /// Update the state of the deliver UI section
        /// </summary>
        private void UpdateDeliveryUI()
        {
            autoSendMinutes.Enabled = autoSend.Checked;
            messageConnectionLimit.Enabled = perConnectionLimit.Checked;
            messageHourLimit.Enabled = perHourLimit.Checked;
            messageInterval.Enabled = intervalLimit.Checked;
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (!accountSettings.SaveToEntity())
            {
                return;
            }

            SaveDeliverySettings();

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(emailAccount);
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                emailAccount.RollbackChanges();
            }
        }
    }
}
