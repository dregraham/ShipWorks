using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Grid.Columns.Definitions;
using ShipWorks.Email.Accounts;
using Divelements.SandGrid;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Connection;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Email.Outlook
{
    /// <summary>
    /// User control for the email outbox
    /// </summary>
    public partial class EmailOutboxControl : EmailOutboundFolderBase
    {
        static Guid gridSettingsKey = new Guid("{0862FE22-60C2-4a33-BCFE-18F75845F483}");

        bool emailsSent = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailOutboxControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public override void Initialize(Form owner)
        {
            base.Initialize(owner);

            menuEdit.Click += this.OnEdit;

            // We want to know when the owner is closing
            owner.FormClosing += new FormClosingEventHandler(OnClosing);
        }

        /// <summary>
        /// Create the query used as the filter for what outbox items to display
        /// </summary>
        protected override IPredicate CreateQueryFilter()
        {
            return
                EmailOutboundFields.SendStatus != (int) EmailOutboundStatus.Sent &
                EmailOutboundFields.Visibility != (int) EmailOutboundVisibility.Internal;
        }

        /// <summary>
        /// Create the column strategy for loading and saving columns
        /// </summary>
        protected override EntityGridColumnStrategy CreateGridColumnStrategy()
        {
            return new StandardGridColumnStrategy(gridSettingsKey, GridColumnDefinitionSet.EmailOutbound, InitializeDefaultGridLayout);
        }

        /// <summary>
        /// Create the default column layout to use for the grid
        /// </summary>
        private void InitializeDefaultGridLayout(GridColumnLayout layout)
        {
            layout.DefaultSortColumnGuid = layout.AllColumns[EmailOutboundFields.ComposedDate].Definition.ColumnGuid;
            layout.DefaultSortOrder = ListSortDirection.Descending;

            // Turn on the Composed column for Outbox
            layout.AllColumns[EmailOutboundFields.ComposedDate].Visible = true;
        }

        /// <summary>
        /// Update the state of the action button UI
        /// </summary>
        protected override void UpdateActionButtonState()
        {
            base.UpdateActionButtonState();

            edit.Enabled =
                edit.Visible &&
                entityGrid.Selection.Count == 1 &&
                UserSession.Security.HasPermission(PermissionType.RelatedObjectSendEmail, entityGrid.Selection.Keys.First());

            send.Enabled = entityGrid.Selection.Count > 0;
        }

        /// <summary>
        /// Edit the selected message
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            EditMessage(entityGrid.Selection.Keys.First());
        }

        /// <summary>
        /// A row is being activated
        /// </summary>
        private void OnRowActivated(object sender, GridRowEventArgs e)
        {
            long emailID = entityGrid.Selection.Keys.First();

            if (UserSession.Security.HasPermission(PermissionType.RelatedObjectSendEmail, emailID))
            {
                EditMessage(emailID);
            }
        }

        /// <summary>
        /// Edit the message with the given id
        /// </summary>
        private void EditMessage(long emailID)
        {
            using (EmailEditorDlg dlg = new EmailEditorDlg(emailID))
            {
                dlg.ShowDialog(this);
            }

            ReloadGridRows();
        }

        /// <summary>
        /// Send the selected messages
        /// </summary>
        private void OnSendSelected(object sender, EventArgs e)
        {
            emailsSent = true;

            EmailCommunicator.StartEmailingMessages(GetMessagesToSend(true));
            EmailCommunicator.ShowProgressDlg(this);
        }

        /// <summary>
        /// Get the list of messages to send.  
        /// 
        /// If any message is set to be sent in the future, the user will be given a message asking them if they want to send them now.  If yes, those messages will be updated
        /// to be able to be sent.  If the user says No, the future messages will be ignored.
        /// </summary>
        /// <param name="onlySelected">If true, only messages that are selected in the grid will be returned.  Otherwise all messages in the grid will be returned.</param>
        /// <returns></returns>
        private List<EmailOutboundEntity> GetMessagesToSend(bool onlySelected)
        {
            List<EmailOutboundEntity> messages = new List<EmailOutboundEntity>();

            messages = onlySelected ? entityGrid.Selection.OrderedKeys.Select(key => (EmailOutboundEntity) entityGrid.EntityGateway.GetEntityFromKey(key)).ToList() : 
                entityGrid.EntityGateway.GetOrderedKeys().Select(key => (EmailOutboundEntity)entityGrid.EntityGateway.GetEntityFromKey(key)).ToList();

            // Get the number of messages that are marked as future sends
            int delayedMessageCount = messages.Count(m => m.DontSendBefore != null && m.DontSendBefore > DateTime.UtcNow);

            if (delayedMessageCount > 0)
            {
                // Ask the user if they want to send future messages now.
                DialogResult answer = MessageHelper.ShowQuestion(this,
                    MessageBoxIcon.Warning,
                    MessageBoxButtons.YesNo,
                    "One or more of the selected messages are scheduled to be delivered in the future.  Do you want to send these messages now?");

                if (answer == DialogResult.No)
                {
                    // The user does not want to send delayed messages now, so filter those out.
                    messages = messages.Where(m => m.DontSendBefore == null || m.DontSendBefore <= DateTime.UtcNow).ToList();
                }
                else
                {
                    // The user does want to send these messages now, so update their DontSendBefore to null and save to the database.
                    // We need to save to the db because the email sender actually re-reads them from the db, so anyting in memory would be ingored.
                    using (SqlAdapter adapter = SqlAdapter.Create(false))
                    {
                        foreach (EmailOutboundEntity emailOutboundEntity in messages.Where(m => m.DontSendBefore != null && m.DontSendBefore > DateTime.UtcNow))
                        {
                            emailOutboundEntity.DontSendBefore = null;
                            adapter.SaveAndRefetch(emailOutboundEntity);
                        }
                    }
                }
            }

            return messages;
        }

        /// <summary>
        /// Send all messages
        /// </summary>
        private void OnSendAll(object sender, EventArgs e)
        {
            emailsSent = true;

            EmailCommunicator.StartEmailingMessages(GetMessagesToSend(false));
            EmailCommunicator.ShowProgressDlg(this);
        }

        /// <summary>
        /// The window is closing
        /// </summary>
        void OnClosing(object sender, FormClosingEventArgs e)
        {
            // If email's have been sent, then there is a case where future-dated emails could have tried
            // to be sent, and thus have errors.  We don't want these errors to stick around, we we clear them
            // out when we close.
            if (emailsSent)
            {
                EmailOutboundEntity prototype = new EmailOutboundEntity();
                prototype.SendStatus = (int) EmailOutboundStatus.Ready;
                prototype.SendAttemptLastError = "";

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.UpdateEntitiesDirectly(prototype, new RelationPredicateBucket(
                        EmailOutboundFields.SendStatus == (int) EmailOutboundStatus.Failed &
                        EmailOutboundFields.DontSendBefore != DBNull.Value &
                        EmailOutboundFields.DontSendBefore > DateTime.UtcNow));
                }
            }
        }
    }
}
