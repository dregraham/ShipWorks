using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Data;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Email;
using ShipWorks.Email.Outlook;
using ShipWorks.Filters;
using ShipWorks.Properties;
using ShipWorks.Templates.Controls;
using ShipWorks.Templates.Emailing;
using ShipWorks.UI;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// User control for displaying the sent email of an order
    /// </summary>
    public partial class EmailOutboundPanel : SingleSelectPanelBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EmailOutboundPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize(Guid settingsKey, GridColumnDefinitionSet definitionSet, Action<GridColumnLayout> layoutInitializer)
        {
            base.Initialize(settingsKey, definitionSet, layoutInitializer);

            // Load the copy menu
            menuCopy.DropDownItems.AddRange(entityGrid.CreateCopyMenuItems(false));
        }

        /// <summary>
        /// EntityType displayed by this panel
        /// </summary>
        public override EntityType EntityType
        {
            get { return EntityType.EmailOutboundEntity; }
        }

        /// <summary>
        /// The targets this supports
        /// </summary>
        public override FilterTarget[] SupportedTargets
        {
            get { return new FilterTarget[] { FilterTarget.Orders, FilterTarget.Customers }; }
        }

        /// <summary>
        /// Update the gateway query filter and reload the grid
        /// </summary>
        protected override IEntityGateway CreateGateway(long entityID)
        {
            addLink.Visible = UserSession.Security.HasPermission(PermissionType.EntityTypeSendEmail, entityID);

            RelationPredicateBucket bucket = new RelationPredicateBucket(
                EmailOutboundRelationFields.RelationType == (int) EmailOutboundRelationType.RelatedObject &
                EmailOutboundFields.Visibility == (int) EmailOutboundVisibility.Visible);
            bucket.Relations.Add(EmailOutboundEntity.Relations.EmailOutboundRelationEntityUsingEmailOutboundID);

            PredicateExpression objectKeyExpression = new PredicateExpression(EmailOutboundRelationFields.EntityID == entityID);

            if (EntityUtility.GetEntityType(entityID) == EntityType.CustomerEntity)
            {
                FieldCompareSetPredicate predicate = new FieldCompareSetPredicate(
                    EmailOutboundRelationFields.EntityID, null, OrderFields.OrderID, null, SetOperator.In, OrderFields.CustomerID == entityID);

                objectKeyExpression.AddWithOr(predicate);
            }

            bucket.PredicateExpression.AddWithAnd(objectKeyExpression);

            return new QueryableEntityGateway(EntityType.EmailOutboundEntity, bucket);
        }

        /// <summary>
        /// Compose a new email
        /// </summary>
        private void OnComposeEmail(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker) delegate
            {
                Debug.Assert(EntityID != null);
                if (EntityID == null)
                {
                    return;
                }

                // Create the template tree that will be popped up
                TemplateTreeControl templateTree = new TemplateTreeControl();
                templateTree.BorderStyle = BorderStyle.None;
                templateTree.HotTracking = true;

                // Create the drop-down
                PopupController popupController = new PopupController(templateTree);
                popupController.Animate = PopupAnimation.Off;

                templateTree.LoadTemplates();
                templateTree.ApplyFolderState(new FolderExpansionState(UserSession.User.Settings.TemplateExpandedFolders));
                templateTree.SelectedTemplateNodeChanged += (object _sender, TemplateNodeChangedEventArgs _e) =>
                    {
                        popupController.Close(DialogResult.OK);

                        ComposeEmail(EntityID.Value, _e.NewNode.Template);
                    };

                // Show the popup
                popupController.ShowDropDown(addLink.Bounds, this);
            });
        }

        /// <summary>
        /// Compose a new email message for the given entity and template
        /// </summary>
        private void ComposeEmail(long entityID, TemplateEntity template)
        {
            using (EmailComposerDlg dlg = new EmailComposerDlg(template, new long[] { entityID }))
            {
                dlg.ShowDialog(this);

                if (dlg.EmailsGenerated.Count > 0)
                {
                    EmailCommunicator.StartEmailingMessages(dlg.EmailsGenerated);
                }
            }
        }

        /// <summary>
        /// An action has occurred in one of the cells, like a hyperlink click.
        /// </summary>
        private void OnGridCellLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            EntityGridRow row = (EntityGridRow) e.Row;

            if (row.Entity == null)
            {
                MessageHelper.ShowMessage(this, "The charge data is not yet loaded.");
                return;
            }

            EmailOutboundEntity email = row.Entity as EmailOutboundEntity;

            GridLinkAction action = (GridLinkAction) ((GridActionDisplayType) e.Column.DisplayType).ActionData;

            if (action == GridLinkAction.Delete)
            {
                DeleteEmail(email.EmailOutboundID);
            }

            if (action == GridLinkAction.Edit)
            {
                if (email.SendStatus == (int) EmailOutboundStatus.Sent)
                {
                    ViewEmail(email.EmailOutboundID);
                }
                else
                {
                    EditEmail(email.EmailOutboundID);
                }
            }
        }

        /// <summary>
        /// Edit the selected detail
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                EditView(entityGrid.Selection.Keys.First());
            }
        }

        /// <summary>
        /// Delete the selected detail
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                DeleteEmail(entityGrid.Selection.Keys.First());
            }
        }

        /// <summary>
        /// An entity row has been double clicked
        /// </summary>
        protected override void OnEntityDoubleClicked(long entityID)
        {
            EditView(entityID);
        }

        /// <summary>
        /// Edit or view the given email, depending on if it has been sent yet or not
        /// </summary>
        private void EditView(long emailID)
        {
            EmailOutboundEntity email = (EmailOutboundEntity) DataProvider.GetEntity(emailID);
            if (email != null)
            {
                if (UserSession.Security.HasPermission(PermissionType.RelatedObjectSendEmail, emailID))
                {
                    if (email.SendStatus == (int) EmailOutboundStatus.Sent)
                    {
                        ViewEmail(email.EmailOutboundID);
                    }
                    else
                    {
                        EditEmail(email.EmailOutboundID);
                    }
                }
            }
        }

        /// <summary>
        /// View the email with the given ID
        /// </summary>
        private void ViewEmail(long emailID)
        {
            using (EmailSentViewerDlg dlg = new EmailSentViewerDlg(emailID))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Edit the email with the given ID
        /// </summary>
        private void EditEmail(long emailID)
        {
            using (EmailEditorDlg dlg = new EmailEditorDlg(emailID))
            {
                dlg.ShowDialog(this);

                // Not checking the DialogResult since Email Accounts can change without OKing the actual editor
                ReloadContent();
            }
        }

        /// <summary>
        /// Delete the given email
        /// </summary>
        private void DeleteEmail(long emailID)
        {
            EmailUtility.DeleteOutboundEmail(
                new long[] { emailID },
                this,
                delegate
                    {
                        ReloadContent();
                    });
        }

        /// <summary>
        /// The context menu is opening
        /// </summary>
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            menuEdit.Enabled = entityGrid.Selection.Count == 1;
            menuDelete.Enabled = entityGrid.Selection.Count == 1;
            menuCopy.Enabled = entityGrid.Selection.Count >= 1;

            bool hasPermission = true;

            if (entityGrid.Selection.Count == 1)
            {
                EmailOutboundEntity email = (EmailOutboundEntity) DataProvider.GetEntity(entityGrid.Selection.Keys.First());
                if (email != null)
                {
                    hasPermission = UserSession.Security.HasPermission(PermissionType.RelatedObjectSendEmail, email.EmailOutboundID);

                    if (email.SendStatus == (int) EmailOutboundStatus.Sent)
                    {
                        menuEdit.Text = "View";
                        menuEdit.Image = Resources.view;
                    }
                    else
                    {
                        menuEdit.Text = "Edit";
                        menuEdit.Image = Resources.edit16;
                    }
                }
            }
            else
            {
                if (EntityID != null)
                {
                    hasPermission = UserSession.Security.HasPermission(PermissionType.EntityTypeSendEmail, EntityID.Value);
                }
            }

            menuEdit.Available = hasPermission;
            menuDelete.Available = UserSession.Security.HasPermission(PermissionType.ManageEmailAccounts);

            menuSep.Available = menuEdit.Available || menuDelete.Available;
        }
    }
}
