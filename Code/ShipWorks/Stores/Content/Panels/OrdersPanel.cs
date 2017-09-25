using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// User control for displaying the orders of a customer
    /// </summary>
    public partial class OrdersPanel : SingleSelectPanelBase
    {
        public event EventHandler OrderDeleted;

        public event EventHandler AddOrderClicked;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrdersPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize(Guid settingsKey, GridColumnDefinitionSet definitionSet, Action<GridColumnLayout> layoutInitializer)
        {
            base.Initialize(settingsKey, definitionSet, layoutInitializer);

            // Edition
            editionGuiHelper.RegisterElement(addLink, Editions.EditionFeature.AddOrderCustomer);

            // Load the copy menu
            menuCopy.DropDownItems.AddRange(entityGrid.CreateCopyMenuItems(false));
        }

        /// <summary>
        /// EntityType displayed by this panel
        /// </summary>
        public override EntityType EntityType
        {
            get { return EntityType.OrderEntity; }
        }

        /// <summary>
        /// The targets this supports
        /// </summary>
        public override FilterTarget[] SupportedTargets
        {
            get { return new FilterTarget[] { FilterTarget.Customers }; }
        }

        /// <summary>
        /// Update the contents of the menu for setting status
        /// </summary>
        private void UpdateOrderStatusMenu()
        {
            // Get all the store keys
            List<long> storeKeys = entityGrid.Selection.Keys.Select(orderID => DataProvider.GetOrderHeader(orderID).StoreID).Distinct().ToList();

            menuLocalStatus.DropDownItems.Clear();
            menuLocalStatus.DropDownItems.AddRange(MenuCommandConverter.ToToolStripItems(
                StatusPresetCommands.CreateMenuCommands(StatusPresetTarget.Order, storeKeys),
                OnSetStatus));
        }

        /// <summary>
        /// Execute an invoked menu command
        /// </summary>
        private async void OnSetStatus(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            IMenuCommand command = MenuCommandConverter.ExtractMenuCommand(sender);

            // Execute the command
            await command.ExecuteAsync(this, entityGrid.Selection.OrderedKeys, OnAsyncSetStatusCompleted).ConfigureAwait(false);
        }

        /// <summary>
        /// Called when an async entity operation has completed
        /// </summary>
        private void OnAsyncSetStatusCompleted(object sender, MenuCommandCompleteEventArgs e)
        {
            Debug.Assert(!InvokeRequired);

            ReloadContent();

            // Show the message, if any
            e.ShowMessage(this);
        }

        /// <summary>
        /// Update the gateway query filter and reload the grid
        /// </summary>
        protected override IEntityGateway CreateGateway(long entityID)
        {
            RelatedKeysEntityGateway gateway = new RelatedKeysEntityGateway(entityID, EntityType.OrderEntity);

            // Add link is available if the user can add orders for any store
            addLink.Visible = StoreManager.GetAllStores().Any(s => UserSession.Security.HasPermission(PermissionType.OrdersModify, s.StoreID));

            return gateway;
        }

        /// <summary>
        /// An action has occurred in one of the cells, like a hyperlink click.
        /// </summary>
        private void OnGridCellLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            EntityGridRow row = (EntityGridRow) e.Row;

            if (row.EntityID == null)
            {
                MessageHelper.ShowMessage(this, "The order data is not yet loaded.");
                return;
            }

            long entityID = row.EntityID.Value;

            GridLinkAction action = (GridLinkAction) ((GridActionDisplayType) e.Column.DisplayType).ActionData;

            if (action == GridLinkAction.Delete)
            {
                DeleteOrders(new long[] { entityID });
            }

            if (action == GridLinkAction.Edit)
            {
                EditOrder(entityID);
            }
        }

        /// <summary>
        /// An entity row has been double clicked
        /// </summary>
        protected override void OnEntityDoubleClicked(long entityID)
        {
            EditOrder(entityID);
        }

        /// <summary>
        /// Edit the selected order
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                EditOrder(entityGrid.Selection.Keys.First());
            }
        }

        /// <summary>
        /// Delete the selected detail
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            DeleteOrders(entityGrid.Selection.OrderedKeys);
        }

        /// <summary>
        /// Add a new order
        /// </summary>
        private void OnAddOrder(object sender, EventArgs e)
        {
            Debug.Assert(EntityID != null);
            if (EntityID == null)
            {
                return;
            }

            // We let the MainForm handle adding the order, so it can select it after its created
            if (AddOrderClicked != null)
            {
                AddOrderClicked(this, EventArgs.Empty);
            }
            else
            {
                throw new InvalidOperationException("Did not handle the add link!");
            }
        }

        /// <summary>
        /// Edit the order with the given ID
        /// </summary>
        private void EditOrder(long orderID)
        {
            OrderEditorDlg.Open(orderID, this);

            ReloadContent();
        }

        /// <summary>
        /// Delete the given order
        /// </summary>
        private void DeleteOrders(IEnumerable<long> orderKeys)
        {
            DialogResult result = MessageHelper.ShowQuestion(this, "Delete the selected orders?");

            if (result == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;

                PermissionAwareDeleter deleter = new PermissionAwareDeleter(this, PermissionType.OrdersModify);
                deleter.ExecuteCompleted += (sender, e) =>
                    {
                        ReloadContent();

                        OrderDeleted?.Invoke(this, EventArgs.Empty);
                    };

                deleter.DeleteAsync(orderKeys);
            }
        }

        /// <summary>
        /// The context menu is opening
        /// </summary>
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            menuEdit.Enabled = entityGrid.Selection.Count == 1;
            menuDelete.Enabled = entityGrid.Selection.Count >= 1;
            menuCopy.Enabled = entityGrid.Selection.Count >= 1;
            menuLocalStatus.Enabled = entityGrid.Selection.Count >= 1;

            bool anyCanDelete = entityGrid.Selection.Keys.Any(k => UserSession.Security.HasPermission(PermissionType.OrdersModify, k));
            bool anyCanStatus = entityGrid.Selection.Keys.Any(k => UserSession.Security.HasPermission(PermissionType.OrdersEditStatus, k));

            menuDelete.Available = anyCanDelete;
            menuLocalStatus.Available = anyCanStatus;
            menuSepStatus.Available = menuLocalStatus.Available;

            UpdateOrderStatusMenu();
        }
    }
}
