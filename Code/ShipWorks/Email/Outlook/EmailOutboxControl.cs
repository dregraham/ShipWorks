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
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
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

            EmailCommunicator.StartEmailingMessages(entityGrid.Selection.OrderedKeys.Select(key => (EmailOutboundEntity) entityGrid.EntityGateway.GetEntityFromKey(key)));
            EmailCommunicator.ShowProgressDlg(this);
        }

        /// <summary>
        /// Send all messages
        /// </summary>
        private void OnSendAll(object sender, EventArgs e)
        {
            if (EmailCommunicator.StartEmailingAccounts())
            {
                EmailCommunicator.ShowProgressDlg(this);
            }
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
