using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Email.Accounts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
using Divelements.SandGrid;
using ShipWorks.Data.Grid;
using ShipWorks.Properties;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Email.Outlook
{
    /// <summary>
    /// User control for the email sent items
    /// </summary>
    public partial class EmailSentItemsControl : EmailOutboundFolderBase
    {
        static Guid gridSettingsKey = new Guid("{787674DF-E7E8-4ba4-8453-A16F51E06059}");

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailSentItemsControl()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize(Form owner)
        {
            base.Initialize(owner);

            menuEdit.Text = "View";
            menuEdit.Image = Resources.view;

            menuEdit.Click += this.OnViewMessage;
        }

        /// <summary>
        /// Create the query used as the filter for what outbox items to display
        /// </summary>
        protected override IPredicate CreateQueryFilter()
        {
            return
                EmailOutboundFields.SendStatus == (int) EmailOutboundStatus.Sent &
                EmailOutboundFields.Visibility == (int) EmailOutboundVisibility.Visible;
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
            layout.DefaultSortColumnGuid = layout.AllColumns[EmailOutboundFields.SentDate].Definition.ColumnGuid;
            layout.DefaultSortOrder = ListSortDirection.Descending;

            // Turn on the SentDate column for sent items
            layout.AllColumns[EmailOutboundFields.SentDate].Visible = true;
        }

        /// <summary>
        /// Update the UI state of the action buttons
        /// </summary>
        protected override void UpdateActionButtonState()
        {
            base.UpdateActionButtonState();

            view.Enabled =
                view.Visible &&
                entityGrid.Selection.Count == 1 &&
                UserSession.Security.HasPermission(PermissionType.RelatedObjectSendEmail, entityGrid.Selection.Keys.First());
        }

        /// <summary>
        /// View the selected email message
        /// </summary>
        private void OnViewMessage(object sender, EventArgs e)
        {
            ViewMessage(entityGrid.Selection.Keys.First());
        }

        /// <summary>
        /// Activated should be the same as view
        /// </summary>
        private void OnRowActivated(object sender, GridRowEventArgs e)
        {
            long emailID = entityGrid.Selection.Keys.First();

            if (UserSession.Security.HasPermission(PermissionType.RelatedObjectSendEmail, emailID))
            {
                ViewMessage(emailID);
            }
        }

        /// <summary>
        /// View the message with the given ID
        /// </summary>
        private void ViewMessage(long emailID)
        {
            using (EmailSentViewerDlg dlg = new EmailSentViewerDlg(emailID))
            {
                dlg.ShowDialog(this);
            }
        }

    }
}
