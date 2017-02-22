using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Divelements.SandGrid;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Caching;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Email.Outlook
{
    /// <summary>
    /// Base for outbound email folders
    /// </summary>
    public partial class EmailOutboundFolderBase : EmailFolderControlBase
    {
        // Used to be the entity cache, monitor for changes, and notify us when the grid needs refreshed
        EntityCacheEntityProvider entityProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailOutboundFolderBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize(Form owner)
        {
            base.Initialize(owner);

            // Hide delete based on security
            delete.Visible = UserSession.Security.HasPermission(PermissionType.ManageEmailAccounts);
            menuDelete.Available = UserSession.Security.HasPermission(PermissionType.ManageEmailAccounts);

            // Load the copy menu
            menuCopy.DropDownItems.AddRange(entityGrid.CreateCopyMenuItems(false));

            // Error display
            entityGrid.ErrorProvider = new EntityGridRowErrorFieldProvider(EmailOutboundFields.SendAttemptLastError);

            // Create the entity provider for caching and refreshing
            entityProvider = new EntityCacheEntityProvider(EntityType.EmailOutboundEntity);
            entityProvider.EntityChangesDetected += new EntityCacheChangeMonitoredChangedEventHandler(OnEntityProviderChangeDetected);

            // Prepare the grid
            entityGrid.InitializeGrid();

            // Prepare configurable columns
            entityGrid.InitializeColumns(CreateGridColumnStrategy());
            entityGrid.SaveColumnsOnClose(owner);

            // Open the gateway
            entityGrid.OpenGateway(new QueryableEntityGateway(entityProvider, new RelationPredicateBucket(CreateQueryFilter())));

            UpdateActionButtonState();

            // Also refresh any time the local emailer gets done
            EmailCommunicator.EmailCommunicationComplete += new EmailCommunicationCompleteEventHandler(OnEmailCommunicationComplete);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                EmailCommunicator.EmailCommunicationComplete -= new EmailCommunicationCompleteEventHandler(OnEmailCommunicationComplete);

                if (entityProvider != null)
                {
                    entityProvider.Dispose();
                    entityProvider = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Provide the query filter for filtering the items to display from the outbox
        /// </summary>
        protected virtual IPredicate CreateQueryFilter()
        {
            throw new NotImplementedException("Must be implemented by derived class to provide filtering for outbox items.");
        }

        /// <summary>
        /// Create the grid column strategy
        /// </summary>
        protected virtual EntityGridColumnStrategy CreateGridColumnStrategy()
        {
            throw new NotImplementedException("Must be implemented by derived class to provide column strategy.");
        }

        /// <summary>
        /// Delete selected emails
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            // Delete the selected emails, and reload the grid when done
            EmailUtility.DeleteOutboundEmail(entityGrid.Selection.OrderedKeys, this, delegate { ReloadGridRows(); });
        }

        /// <summary>
        /// The entity provider detected changes to the underlying data.  The grid needs updated.
        /// </summary>
        private void OnEntityProviderChangeDetected(object sender, EntityCacheChangeMonitoredChangedEventArgs e)
        {
            // We always reload, not update - b\c the content can change grids as it goes from unsent to sent
            entityGrid.ReloadGridRows();

            UpdateActionButtonState();
        }

        /// <summary>
        /// Reload the rows to ensure they are showing the latest state
        /// </summary>
        protected void ReloadGridRows()
        {
            entityGrid.ReloadGridRows();

            UpdateActionButtonState();
        }

        /// <summary>
        /// Grid selection has changed
        /// </summary>
        private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateActionButtonState();
        }

        /// <summary>
        /// Update the state of the action buttons based on what is selected
        /// </summary>
        protected virtual void UpdateActionButtonState()
        {
            delete.Enabled = entityGrid.Selection.Count > 0;
        }

        /// <summary>
        /// An email communication session has completed
        /// </summary>
        void OnEmailCommunicationComplete(object sender, EmailCommunicationCompleteEventArgs e)
        {
            BeginInvoke((MethodInvoker) ReloadGridRows);
        }

        /// <summary>
        /// The context menu is opening
        /// </summary>
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            menuEdit.Enabled = entityGrid.Selection.Count == 1;
            menuDelete.Enabled = entityGrid.Selection.Count >= 1;
            menuCopy.Enabled = entityGrid.Selection.Count >= 1;

            StorePermissionCoverage coverage = UserSession.Security.GetRelatedObjectPermissionCoverage(PermissionType.RelatedObjectSendEmail);

            bool hasEditPermission;

            if (coverage == StorePermissionCoverage.All)
            {
                hasEditPermission = true;
            }
            else if (coverage == StorePermissionCoverage.None)
            {
                hasEditPermission = false;
            }
            else
            {
                if (entityGrid.Selection.Count == 1)
                {
                    hasEditPermission = UserSession.Security.HasPermission(PermissionType.RelatedObjectSendEmail, entityGrid.Selection.Keys.First());
                }
                else
                {
                    hasEditPermission = true;
                }
            }

            menuEdit.Available = hasEditPermission;
            menuSep.Available = menuEdit.Available || menuDelete.Available;
        }
    }
}
