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
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Templates.Tokens;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// UserControl for viewing and editing order items
    /// </summary>
    public partial class OrderItemsPanel : SingleSelectPanelBase
    {
        PanelDataMode dataMode;

        // Only valid if the data mode is local
        List<OrderItemEntity> localItems;

        // Needs to be set and only valid if the data mode is local
        long? localStoreID = null;

        /// <summary>
        /// Raised when an item has been added, edited, or deleted
        /// </summary>
        public event EventHandler ItemsChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderItemsPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize(Guid settingsKey, GridColumnDefinitionSet definitionSet, Action<GridColumnLayout> layoutInitializer)
        {
            Initialize(settingsKey, definitionSet, PanelDataMode.LiveDatabase, layoutInitializer);
        }
        /// <summary>
        /// Initialization
        /// </summary>
        public void Initialize(Guid settingsKey, GridColumnDefinitionSet definitionSet, PanelDataMode dataMode, Action<GridColumnLayout> layoutInitializer)
        {
            this.dataMode = dataMode;

            entityGrid.PrimaryGrid.NewRowType = typeof(OrderItemGridRow);

            base.Initialize(settingsKey, definitionSet, layoutInitializer);

            // Load the copy menu
            menuCopy.DropDownItems.AddRange(entityGrid.CreateCopyMenuItems(false));

            if (dataMode == PanelDataMode.LocalPending)
            {
                localItems = new List<OrderItemEntity>();
            }
        }

        /// <summary>
        /// The PaleDataMode the panel is in
        /// </summary>
        public PanelDataMode DataMode
        {
            get { return dataMode; }
        }

        /// <summary>
        /// The list of local items that have been added.  Only valid if data mode is local.
        /// </summary>
        public List<OrderItemEntity> LocalItems
        {
            get { return localItems; }
        }

        /// <summary>
        /// Set's the storeID that the local collection of items goes with. Only valid if the data mode is local.  Use to populate
        /// the item status menu.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(null)]
        public long? LocalStoreID
        {
            get
            {
                return localStoreID;
            }
            set
            {
                localStoreID = value;

                UpdateItemStatusMenu();
            }
        }

        /// <summary>
        /// EntityType displayed by this panel
        /// </summary>
        public override EntityType EntityType
        {
            get { return EntityType.OrderItemEntity; }
        }

        /// <summary>
        /// The targets this supports
        /// </summary>
        public override FilterTarget[] SupportedTargets
        {
            get { return new FilterTarget[] { FilterTarget.Orders }; }
        }

        /// <summary>
        /// Update the item status menu to reflect the current options
        /// </summary>
        private void UpdateItemStatusMenu()
        {
            menuItemStatus.DropDownItems.Clear();

            // Address FB #266809: EntityID was null below and I think it was a race condition
            // Because of that, we get the value once, then check that the copy is valid
            long localEntityValue = EntityID.GetValueOrDefault(long.MinValue);
            Debug.Assert(localEntityValue != long.MinValue);

            if (localEntityValue == long.MinValue)
            {
                return;
            }

            List<long> storeKeys = new List<long>();

            // For live database use the selected store keys
            if (dataMode == PanelDataMode.LiveDatabase)
            {
                storeKeys = DataProvider.GetRelatedKeys(localEntityValue, EntityType.StoreEntity);
            }
            // For local mode use the specified storeID
            else
            {
                if (localStoreID != null)
                {
                    storeKeys.Add(localStoreID.Value);
                }
            }

            menuItemStatus.DropDownItems.AddRange(MenuCommandConverter.ToToolStripItems(
                StatusPresetCommands.CreateMenuCommands(StatusPresetTarget.OrderItem, storeKeys),
                OnSetLocalStatus));
        }

        /// <summary>
        /// Execute an invoked menu command
        /// </summary>
        private async void OnSetLocalStatus(object sender, EventArgs e)
        {
            IMenuCommand command = MenuCommandConverter.ExtractMenuCommand(sender);

            if (dataMode == PanelDataMode.LiveDatabase)
            {
                Cursor.Current = Cursors.WaitCursor;

                // Execute the command
                await command.ExecuteAsync(this, entityGrid.Selection.OrderedKeys, OnAsyncSetStatusCompleted).ConfigureAwait(true);
            }
            else
            {
                StatusPresetEntity preset = (StatusPresetEntity) command.Tag;

                foreach (OrderItemEntity orderItem in entityGrid.Selection.Keys.Select(k => entityGrid.EntityGateway.GetEntityFromKey(k)))
                {
                    orderItem.LocalStatus = preset.StatusText;
                }

                await ReloadContent().ConfigureAwait(true);

                if (TemplateTokenProcessor.HasTokens(preset.StatusText))
                {
                    MessageHelper.ShowInformation(this, "ShipWorks will process the tokens in the status text when the order is saved.");
                }
            }
        }

        /// <summary>
        /// Called when an async entity operation has completed
        /// </summary>
        private void OnAsyncSetStatusCompleted(object sender, EventArgs e)
        {
            Debug.Assert(!InvokeRequired);

            ReloadContent();

            RaiseItemsChanged();
        }

        /// <summary>
        /// Update the gateway query filter and reload the grid
        /// </summary>
        protected override IEntityGateway CreateGateway(long entityID)
        {
            if (dataMode == PanelDataMode.LiveDatabase)
            {
                RelatedKeysEntityGateway gateway = new RelatedKeysEntityGateway(entityID, EntityType.OrderItemEntity);

                // Update our ability to add based on what entity we are now displaying
                addLink.Visible = UserSession.Security.HasPermission(PermissionType.OrdersModify, entityID);

                return gateway;
            }
            else
            {
                return new LocalCollectionEntityGateway<OrderItemEntity>(localItems);
            }
        }

        /// <summary>
        /// Edit the selected item
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                EditItem(entityGrid.Selection.Keys.First());
            }
        }

        /// <summary>
        /// Delete the selected item
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            if (entityGrid.Selection.Count == 1)
            {
                DeleteItem(entityGrid.Selection.Keys.First());
            }
        }

        /// <summary>
        /// An entity row has been double clicked
        /// </summary>
        protected override void OnEntityDoubleClicked(long entityID)
        {
            if (UserSession.Security.HasPermission(PermissionType.OrdersModify, entityID))
            {
                EditItem(entityID);
            }
        }

        /// <summary>
        /// An action has occurred in one of the cells, like a hyperlink click.
        /// </summary>
        private void OnGridCellLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            EntityGridRow row = (EntityGridRow) e.Row;

            if (row.EntityID == null)
            {
                MessageHelper.ShowMessage(this, "The item is not yet loaded.");
                return;
            }

            long entityID = row.EntityID.Value;

            GridLinkAction action = (GridLinkAction) ((GridActionDisplayType) e.Column.DisplayType).ActionData;

            if (action == GridLinkAction.Delete)
            {
                DeleteItem(entityID);
            }

            if (action == GridLinkAction.Edit)
            {
                EditItem(entityID);
            }
        }

        /// <summary>
        /// Add a new item to the order
        /// </summary>
        private void OnAddItem(object sender, EventArgs e)
        {
            Debug.Assert(EntityID != null);
            if (EntityID == null)
            {
                return;
            }

            OrderItemEntity item = new OrderItemEntity();
            item.OrderID = EntityID.Value;
            item.Quantity = 1;
            item.IsManual = true;
            item.InitializeNullsToDefault();

            // For local ones, they have to have a fakeID until they are ready to save
            if (dataMode == PanelDataMode.LocalPending)
            {
                item.OrderItemID = localItems.Count == 0 ?
                    -EntityUtility.GetEntitySeed(EntityType.OrderItemEntity) :
                    localItems.Min(i => i.OrderItemID) - 1000;
            }

            using (EditItemDlg dlg = new EditItemDlg(item, GetEffectiveStoreID(), dataMode))
            {
                DialogResult result = dlg.ShowDialog(this);

                if (result == DialogResult.OK && dataMode == PanelDataMode.LocalPending)
                {
                    localItems.Add(item);
                }

                // OK and Abort both could mean data has changed - reload as long as not canceled
                if (result != DialogResult.Cancel)
                {
                    ReloadContent();

                    RaiseItemsChanged();
                }
            }
        }

        /// <summary>
        /// Get the StoreID of the items we are displaying
        /// </summary>
        private long GetEffectiveStoreID()
        {
            long? storeID = (dataMode == PanelDataMode.LocalPending) ? localStoreID : DataProvider.GetRelatedKeys(EntityID.Value, EntityType.StoreEntity).FirstOrDefault();

            return storeID.Value;
        }

        /// <summary>
        /// Delete the given order item
        /// </summary>
        private void DeleteItem(long itemID)
        {
            DialogResult result = MessageHelper.ShowQuestion(this, "Delete the selected item?");

            if (result == DialogResult.OK)
            {
                if (dataMode == PanelDataMode.LiveDatabase)
                {
                    OrderUtility.DeleteItem(itemID);
                }
                else
                {
                    localItems.Remove(localItems.Single(i => i.OrderItemID == itemID));
                }

                ReloadContent();

                RaiseItemsChanged();
            }
        }

        /// <summary>
        /// Edit the given order item
        /// </summary>
        private void EditItem(long itemID)
        {
            OrderItemEntity item;

            if (dataMode == PanelDataMode.LiveDatabase)
            {
                item = (OrderItemEntity) DataProvider.GetEntity(itemID);

                if (item == null)
                {
                    MessageHelper.ShowMessage(this, "The item has been deleted.");
                }
            }

            else
            {
                item = localItems.Single(i => i.OrderItemID == itemID);
            }

            if (item != null)
            {
                using (EditItemDlg dlg = new EditItemDlg(item, GetEffectiveStoreID(), dataMode))
                {
                    var result = dlg.ShowDialog(this);

                    if (result != DialogResult.Cancel)
                    {
                        ReloadContent();

                        RaiseItemsChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Raise the items changed event to notify that an item has been added edited or deleted
        /// </summary>
        private void RaiseItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The context menu is opening
        /// </summary>
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            menuEdit.Enabled = entityGrid.Selection.Count == 1;
            menuDelete.Enabled = entityGrid.Selection.Count == 1;
            menuCopy.Enabled = entityGrid.Selection.Count >= 1;
            menuItemStatus.Enabled = entityGrid.Selection.Count >= 1;

            menuEdit.Available = EntityID == null || dataMode == PanelDataMode.LocalPending || UserSession.Security.HasPermission(PermissionType.OrdersModify, (long) EntityID);
            menuDelete.Available = menuEdit.Available;
            menuSep.Available = menuEdit.Available;

            menuItemStatus.Available = EntityID == null || dataMode == PanelDataMode.LocalPending || UserSession.Security.HasPermission(PermissionType.OrdersEditStatus, (long) EntityID);
            menuSepStatus.Available = menuItemStatus.Available;

            UpdateItemStatusMenu();
        }
    }
}
