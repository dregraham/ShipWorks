using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Shipping.Editing;
using ShipWorks.UI.Utility;
using ShipWorks.Data.Grid.Columns;
using Divelements.SandGrid;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Filters;
using Divelements.SandGrid.Rendering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.UI;
using ShipWorks.Data.Controls;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Grid;
using System.Collections;
using Interapptive.Shared.Utility;
using System.Diagnostics;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Stores;
using ShipWorks.Data;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.CoreExtensions.Grid;
using ShipWorks.ApplicationCore;
using System.Threading;
using Interapptive.Shared;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// The control to select shipments for the main shipping grid
    /// </summary>
    [ToolboxItem(false)]
    [NDependIgnoreLongTypes]
    public partial class ShipmentGridControl : UserControl
    {
        // Maps a shipment to its row
        Dictionary<long, ShipmentGridRow> shipmentRowMap = new Dictionary<long, ShipmentGridRow>();

        // Custom column used for hidden secondary sort
        ShipmentGridHiddenSortColumn shipmentNumberSorter = new ShipmentGridHiddenSortColumn(r => ShippingManager.GetSiblingData(r.Shipment).ShipmentNumber );
        ShipmentGridHiddenSortColumn orderGridPositionSorter = new ShipmentGridHiddenSortColumn(r => r.SortIndex);

        static Guid gridSettingsKey = new Guid("{F933A7D5-33D3-460b-9EA8-EA6D9D9285F3}");

        /// <summary>
        /// Track the previous selection so we know when it changes, and so we can tell the consumer what it used to be
        /// </summary>
        Dictionary<long, ShipmentGridRow> previousSelection = new Dictionary<long, ShipmentGridRow>();

        // Temporarily suspends processing selection changes
        int suspendSelectionProcessing = 0;

        /// <summary>
        /// The grid selection has changed
        /// </summary>
        public event ShipmentSelectionChangedEventHandler SelectionChanged;
        
        /// <summary>
        /// Raised when shipments are added to the grid
        /// </summary>
        public event EventHandler ShipmentsAdded;

        /// <summary>
        /// Raised when shipments are removed from the grid
        /// </summary>
        public event EventHandler ShipmentsRemoved;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreComplexMethodAttribute]
        public ShipmentGridControl()
        {
            InitializeComponent();

            shipmentsToolbar.Renderer = new NoBorderToolStripRenderer();
            ordersToolbar.Renderer = new NoBorderToolStripRenderer();

            // Add the copy menu items
            menuCopy.DropDownItems.AddRange(entityGrid.CreateCopyMenuItems(true));

            // Apply grid theming
            ApplyGridTheme();

            entityGrid.Columns.Clear();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public void Initialize()
        {
            StandardGridColumnStrategy columnStrategy = new StandardGridColumnStrategy(
                gridSettingsKey,
                GridColumnDefinitionSet.ShipmentStandard,
                null,
                new List<GridColumn> { shipmentNumberSorter, orderGridPositionSorter });

            // Allow detail view
            columnStrategy.DetailViewEnabled = true;

            // Prepare configurable columns
            entityGrid.InitializeColumns(columnStrategy);
            entityGrid.SaveColumnsOnClose((Form) TopLevelControl);

            UpdateSecurityUI();
        }

        /// <summary>
        /// Apply the look we want for the grid
        /// </summary>
        private void ApplyGridTheme()
        {
            // Apply the same selection colors as the order grid renderer
            ISandGridRenderer renderer;

            if (InterapptiveOnly.MagicKeysDown)
            {
                renderer = AppearanceHelper.CreateSandGridRenderer(FilterTarget.Orders);
                kryptonStatusPanel.GroupBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderColumnSheet;
            }
            else
            {
                renderer = AppearanceHelper.CreateWindowsRenderer();
            }

            entityGrid.Renderer = renderer;
        }

        /// <summary>
        /// Field source for nested error display.  If null then nested error display is off.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(null)]
        public EntityGridRowErrorProvider ErrorProvider
        {
            get
            {
                return entityGrid.ErrorProvider;
            }
            set
            {
                entityGrid.ErrorProvider = value;
            }
        }

        /// <summary>
        /// The selection in the grid has changed
        /// </summary>
        private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (suspendSelectionProcessing > 0)
            {
                return;
            }

            if (!HasSelectionChanged())
            {
                return;
            }

            UpdateSelectionDependentUI();

            if (SelectionChanged != null)
            {
                SelectionChanged(this, new ShipmentSelectionChangedEventArgs(previousSelection.Values.ToList()));
            }

            previousSelection = SelectedRows.ToDictionary(r => r.Shipment.ShipmentID);
        }

        /// <summary>
        /// Determines if the selection has changed from the last time we captured it
        /// </summary>
        private bool HasSelectionChanged()
        {
            if (previousSelection.Count != entityGrid.SelectedElements.Count)
            {
                return true;
            }

            foreach (ShipmentEntity shipment in SelectedShipments)
            {
                if (!previousSelection.ContainsKey(shipment.ShipmentID))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Update the UI based on the current state of the selection
        /// </summary>
        public void UpdateSelectionDependentUI()
        {
            UpdateShipmentButtons();
            UpdateStatusBar();

            entityGrid.Update();
        }

        /// <summary>
        /// Update the UI based on the rows that are in the grid to determine what secured features are available
        /// </summary>
        private void UpdateSecurityUI()
        {
            // Security
            newShipmentButton.Available = AllRows.Any(r => UserSession.Security.HasPermission(PermissionType.ShipmentsCreateEditProcess, r.EntityID.Value));
            deleteShipmentButton.Available = AllRows.Any(r => UserSession.Security.HasPermission(PermissionType.ShipmentsVoidDelete, r.EntityID.Value));
            menuItemCopyShipment.Available = AllRows.Any(r => UserSession.Security.HasPermission(PermissionType.ShipmentsCreateEditProcess, r.EntityID.Value));

            menuCreateNewShipment.Available = newShipmentButton.Available;
            menuDeleteShipment.Available = deleteShipmentButton.Available;
            menuSep1.Available = newShipmentButton.Available | deleteShipmentButton.Available;
        }

        /// <summary>
        /// Suspend all reactions to selection changes
        /// </summary>
        private void SuspendSelectionProcessing()
        {
            suspendSelectionProcessing++;
        }

        /// <summary>
        /// Resume selection change processing, and do the processing if the selection has changed since the call to suspend.
        /// </summary>
        private void ResumeSelectionProcessing()
        {
            suspendSelectionProcessing--;

            if (suspendSelectionProcessing == 0)
            {
                OnGridSelectionChanged(entityGrid, null);
            }
        }

        /// <summary>
        /// Update the UI of the shipment buttons
        /// </summary>
        private void UpdateShipmentButtons()
        {
            OrderEntity singleOrder = GetSingleSelectedOrder();

            ShipmentEntity singleShipment = (SelectedShipments.Count() == 1) ? SelectedShipments.First() : null;

            newShipmentButton.Enabled = singleOrder != null && UserSession.Security.HasPermission(PermissionType.ShipmentsCreateEditProcess, singleOrder.StoreID);
            deleteShipmentButton.Enabled = entityGrid.SelectedElements.Count > 0 && SelectedShipments.Any(s => UserSession.Security.HasPermission(PermissionType.ShipmentsVoidDelete, s.OrderID));
            removeShipmentButton.Enabled = entityGrid.SelectedElements.Count > 0;
            menuItemCopyShipment.Enabled = entityGrid.SelectedElements.Count > 0 && SelectedShipments.Any(s => UserSession.Security.HasPermission(PermissionType.ShipmentsCreateEditProcess, s.OrderID));
            menuCopyShipmentReturn.Enabled = entityGrid.SelectedElements.Count > 0 && singleShipment != null && 
                                             UserSession.Security.HasPermission(PermissionType.ShipmentsCreateEditProcess, singleOrder.StoreID) && 
                                             ShipmentTypeManager.GetType(singleShipment).SupportsReturns;
        }

        /// <summary>
        /// Update the status bar display
        /// </summary>
        private void UpdateStatusBar()
        {
            labelTotalShipments.Text = string.Format("Shipments: {0}", entityGrid.Rows.Count);
            labelSelectedShipments.Text = string.Format("Selected: {0}", entityGrid.SelectedElements.Count);

            labelStatusEtch.Left = labelTotalShipments.Right + 2;
            labelSelectedShipments.Left = labelStatusEtch.Right + 3;
        }

        /// <summary>
        /// Used to inform the control that some data has changed, and its necessary to resort and refresh the displayed data. No data is pulled from the database,
        /// its just refreshed based on what's in memory.
        /// </summary>
        public void RefreshAndResort()
        {
            // See if there are any deleted shipments we need to get rid of
            Debug.Assert(!InvokeRequired);

            // If there are going to be some to remove, do them all at once, don't let any processing happen as they go, in the case
            // that some where selected.
            SuspendSelectionProcessing();

            bool removed = false;

            List<ShipmentEntity> shipmentsDeleted = new List<ShipmentEntity>();

            // Go through each one and remove the grid row for any shipment that is now deleted
            foreach (ShipmentGridRow row in AllRows.ToList())
            {
                if (row.Shipment.DeletedFromDatabase)
                {
                    RemoveShipmentRow(row.Shipment.ShipmentID);
                    removed = true;
                    shipmentsDeleted.Add(row.Shipment);
                }
            }

            // Resume selection processing
            ResumeSelectionProcessing();

            // Force the sort and refresh of displayed data
            Resort();

            if (removed)
            {
                ShipmentGridShipmentsChangedEventArgs eventArgs = new ShipmentGridShipmentsChangedEventArgs(null, shipmentsDeleted);
                RaiseShipmentsRemoved(eventArgs);
            }
        }

        /// <summary>
        /// Raise the ShipmentsAdded event
        /// </summary>
        private void RaiseShipmentsAdded(ShipmentGridShipmentsChangedEventArgs eventArgs)
        {
            if (ShipmentsAdded != null)
            {
                ShipmentsAdded(this, eventArgs);
            }
        }

        /// <summary>
        /// Raise the ShipmentsRemoved event
        /// </summary>
        private void RaiseShipmentsRemoved(ShipmentGridShipmentsChangedEventArgs eventArgs)
        {
            if (ShipmentsRemoved != null)
            {
                ShipmentsRemoved(this, eventArgs);
            }
        }

        #region Shipment Management

        /// <summary>
        /// The total number of selected rows
        /// </summary>
        public int SelectedRowCount
        {
            get { return entityGrid.SelectedElements.Count; }
        }

        /// <summary>
        /// Get the strongly typed collection of selected shipment rows
        /// </summary>
        public IEnumerable<ShipmentGridRow> SelectedRows
        {
            get
            {
                // The SelectedElements collection is not ordered, so we have to do it this way
                return entityGrid.Rows.Cast<ShipmentGridRow>().Where(r => r.Selected);
            }
        }

        /// <summary>
        /// Get all shipments selected in the grid
        /// </summary>
        public IEnumerable<ShipmentEntity> SelectedShipments
        {
            get
            {
                return SelectedRows.Select(r => r.Shipment);
            }
        }

        /// <summary>
        /// Get the strongly typed collection of all shipment rows
        /// </summary>
        public IEnumerable<ShipmentGridRow> AllRows
        {
            get
            {
                return entityGrid.Rows.Cast<ShipmentGridRow>();
            }
        }

        /// <summary>
        /// Find the row corresponding to the given ShipmentID, or null if not found
        /// </summary>
        public ShipmentGridRow FindRow(long shipmentID)
        {
            ShipmentGridRow row;
            if (shipmentRowMap.TryGetValue(shipmentID, out row))
            {
                return row;
            }

            return null;
        }

        /// <summary>
        /// Add the given shipments to the grid.  Their order will be preservied as a secondary sort to any grid sort column.
        /// </summary>
        public void AddShipments(IEnumerable<ShipmentEntity> shipments)
        {
            if (shipments == null)
            {
                throw new ArgumentNullException("shipments");
            }

            long lastOrderID = -1;
            int sortIndex = (entityGrid.Rows.Count == 0) ? -1 : AllRows.Max(r => r.SortIndex);

            foreach (ShipmentEntity shipment in shipments)
            {
                if (shipment.OrderID != lastOrderID)
                {
                    lastOrderID = shipment.OrderID;
                    sortIndex++;
                }

                AddShipment(shipment, sortIndex);
            }
        }

        /// <summary>
        /// Add a new row to the grid for the given shipment
        /// </summary>
        private ShipmentGridRow AddShipment(ShipmentEntity shipment, int sortIndex)
        {
            ShipmentGridRow row = new ShipmentGridRow(shipment, sortIndex);

            shipmentRowMap.Add(shipment.ShipmentID, row);
            entityGrid.Rows.Add(row);

            UpdateSecurityUI();

            return row;
        }

        /// <summary>
        /// Remove the given shipment from the grid
        /// </summary>
        private void RemoveShipmentRow(long shipmentID)
        {
            ShipmentGridRow row = shipmentRowMap[shipmentID];

            entityGrid.Rows.Remove(row);
            shipmentRowMap.Remove(shipmentID);

            UpdateSecurityUI();
        }

        /// <summary>
        /// Add a shipment to the selected order
        /// </summary>
        private void OnAddShipmentToOrder(object sender, EventArgs e)
        {
            ShipmentGridRow selectedRow = (ShipmentGridRow) entityGrid.SelectedElements[0];
            ShipmentEntity selectedShipment = selectedRow.Shipment;
            OrderEntity order = selectedShipment.Order;

            try
            {
                ShipmentEntity shipment = ShippingManager.CreateShipment(order.OrderID);
                ShipmentGridRow row = AddShipment(shipment, selectedRow.SortIndex);

                SelectShipments(new List<ShipmentEntity>() { shipment });

                Resort();

                ShipmentGridShipmentsChangedEventArgs eventArgs = new ShipmentGridShipmentsChangedEventArgs(new List<ShipmentEntity>() {shipment}, null);
                RaiseShipmentsAdded(eventArgs);
            }
            catch (SqlForeignKeyException)
            {
                MessageHelper.ShowError(this, "The order has been deleted.");

                List<ShipmentGridRow> toRemove = new List<ShipmentGridRow>();

                // Delete all the rows that are for this order
                foreach (ShipmentGridRow row in entityGrid.Rows)
                {
                    if (row.Shipment.OrderID == order.OrderID)
                    {
                        toRemove.Add(row);
                    }
                }

                foreach (ShipmentGridRow row in toRemove)
                {
                    RemoveShipmentRow(row.Shipment.ShipmentID);
                }

                Resort();

                if (toRemove.Count > 0)
                {
                    ShipmentGridShipmentsChangedEventArgs eventArgs = new ShipmentGridShipmentsChangedEventArgs(null, toRemove.Select(row => row.Shipment).ToList());
                    RaiseShipmentsRemoved(eventArgs);
                }
            }
        }

        /// <summary>
        /// Choose more orders to add to the shipping window
        /// </summary>
        private async void OnChooseMore(object sender, EventArgs e)
        {
            using (EntityPickerDlg dlg = new EntityPickerDlg(FilterTarget.Orders))
            {
                dlg.FormClosing += new FormClosingEventHandler(OnOrderPickerClosing);

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    ShipmentsLoader loader = new ShipmentsLoader(this);
                    loader.LoadCompleted += OnLoadMoreShipmentsCompleted;
                    await loader.LoadAsync(dlg.Selection.OrderedKeys);
                }
            }
        }

        /// <summary>
        /// The order picker window is closing
        /// </summary>
        void OnOrderPickerClosing(object sender, FormClosingEventArgs e)
        {
            EntityPickerDlg dlg = (EntityPickerDlg) sender;

            if (dlg.DialogResult == DialogResult.OK)
            {
                if (entityGrid.Rows.Count + dlg.Selection.Count > ShipmentsLoader.MaxAllowedOrders)
                {
                    MessageHelper.ShowInformation(dlg, string.Format("You can only ship up to {0} orders at a time.", ShipmentsLoader.MaxAllowedOrders));
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Select the given shipments
        /// </summary>
        public void SelectShipments(List<ShipmentEntity> shipments)
        {
            if (shipments == null)
            {
                throw new ArgumentNullException("shipments");
            }

            // first make sure they all exist
            SuspendSelectionProcessing();

            entityGrid.SelectedElements.Clear();

            foreach (ShipmentEntity shipment in shipments)
            {
                shipmentRowMap[shipment.ShipmentID].Selected = true;
            }

            Resort();
            ResumeSelectionProcessing();

            UpdateStatusBar();

            ShipmentGridShipmentsChangedEventArgs eventArgs = new ShipmentGridShipmentsChangedEventArgs(shipments, null);
            RaiseShipmentsAdded(eventArgs);
        }

        /// <summary>
        /// The async loading of shipments for shipping has completed
        /// </summary>
        void OnLoadMoreShipmentsCompleted(object sender, ShipmentsLoadedEventArgs e)
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (e.Cancelled)
            {
                return;
            }

            SuspendSelectionProcessing();

            entityGrid.SelectedElements.Clear();

            // Add all the shipments that we don't already have
            AddShipments(e.Shipments.Where(shipment => !shipmentRowMap.ContainsKey(shipment.ShipmentID)));

            // Select them all
            foreach (ShipmentEntity shipment in e.Shipments)
            {
                shipmentRowMap[shipment.ShipmentID].Selected = true;
            }

            Resort();
            ResumeSelectionProcessing();

            UpdateStatusBar();

            ShipmentGridShipmentsChangedEventArgs eventArgs = new ShipmentGridShipmentsChangedEventArgs(e.Shipments, null);
            RaiseShipmentsAdded(eventArgs);
        }

        /// <summary>
        /// A key has been pressed, check for shortcuts
        /// </summary>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (entityGrid.SelectedElements.Count > 0)
                {
                    OnDeleteShipments(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Delete the selected shipments
        /// </summary>
        private void OnDeleteShipments(object sender, EventArgs e)
        {
            List<ShipmentGridRow> shipmentRows = entityGrid.SelectedElements.Cast<ShipmentGridRow>().ToList();
            bool processed = shipmentRows.Count(r => r.Shipment.Processed) > 0;

            DialogResult result;

            if (processed)
            {
                result = MessageBox.Show(this,
                    "One or more of the selected shipments has been processed.\n\n" +
                    "Deleting a shipment does not void it.  If you delete the shipments\n" +
                    "you will no longer be able to void them or print their labels\n" +
                    "from ShipWorks.\n\n" +
                    "Delete all selected shipments?",
                    "ShipWorks",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);
            }
            else
            {
                result = MessageHelper.ShowQuestion(this, "Delete all selected shipments?");
            }

            if (result == DialogResult.OK)
            {
                SuspendSelectionProcessing();

                BackgroundExecutor<ShipmentGridRow> executor = new BackgroundExecutor<ShipmentGridRow>(this,
                    "Delete Shipments",
                    "ShipWorks is deleting the selected shipments.",
                    "Deleting {0} of {1}");

                executor.ExecuteCompleted += OnDeleteCompleted;
                executor.ExecuteAsync((gridRow, state, issueAdder) =>
                {
                    ShipmentEntity shipment = gridRow.Shipment;
                    long shipmentID = shipment.ShipmentID;

                    if (UserSession.Security.HasPermission(PermissionType.ShipmentsVoidDelete, shipment.OrderID))
                    {
                        // Delete the shipment.
                        ShippingManager.DeleteShipment(EntityUtility.CloneEntity(shipment));

                        // Mark it as deleted so the UI knows
                        shipment.DeletedFromDatabase = true;

                        // Remove the row from the grid now that it has been deleted
                        Invoke((MethodInvoker) delegate { RemoveShipmentRow(shipmentID); });
                    }
                    else
                    {
                        issueAdder.Add(gridRow);
                    }
                },
                shipmentRows,
                shipmentRows.Select(row => row.Shipment).ToList());
            }
        }

        /// <summary>
        /// The async delete of shipments has completed
        /// </summary>
        void OnDeleteCompleted(object sender, BackgroundExecutorCompletedEventArgs<ShipmentGridRow> e)
        {
            if (e.Issues.Count > 0)
            {
                MessageHelper.ShowWarning(this, "Some shipments could not be deleted since you didn't have permission.");
            }

            Resort();
            ResumeSelectionProcessing();

            List<ShipmentEntity> shipments = (List<ShipmentEntity>) e.UserState;

            ShipmentGridShipmentsChangedEventArgs eventArgs = new ShipmentGridShipmentsChangedEventArgs(null, shipments);
            RaiseShipmentsRemoved(eventArgs);
        }

        /// <summary>
        /// Removes selected shipments from the list.  They are not deleted.
        /// </summary>
        private void OnRemoveShipments(object sender, EventArgs e)
        {
            SuspendSelectionProcessing();

            List<ShipmentEntity> shipmentsDeleted = new List<ShipmentEntity>();
            foreach (ShipmentGridRow selected in entityGrid.SelectedElements.ToArray())
            {
                RemoveShipmentRow(selected.Shipment.ShipmentID);
                shipmentsDeleted.Add(selected.Shipment);
            }

            ResumeSelectionProcessing();

            ShipmentGridShipmentsChangedEventArgs eventArgs = new ShipmentGridShipmentsChangedEventArgs(null, shipmentsDeleted);
            RaiseShipmentsRemoved(eventArgs);
        }

        /// <summary>
        /// The shipments context menu is opening
        /// </summary>
        private void OnShipmentMenuOpening(object sender, CancelEventArgs e)
        {
            menuCopy.Enabled = entityGrid.Selection.Count > 0;
            menuRemoveFromList.Enabled = entityGrid.Selection.Count > 0;
            menuDeleteShipment.Enabled = entityGrid.Selection.Count > 0;

            OrderEntity order = GetSingleSelectedOrder();

            if (order != null)
            {
                menuCreateNewShipment.Text = string.Format("New Shipment (Order {0})", order.OrderNumberComplete);
            }
            else
            {
                menuCreateNewShipment.Text = "New Shipment";
            }

            menuCreateNewShipment.Enabled = newShipmentButton.Enabled;
            menuDeleteShipment.Enabled = deleteShipmentButton.Enabled;
        }

        /// <summary>
        /// Get the single order that is selected.  If shipments that have different orders are selected, this will return null.
        /// </summary>
        private OrderEntity GetSingleSelectedOrder()
        {
            OrderEntity order = null;

            // This determines the order that is selected - if and only if all selected shipments are for the same order
            foreach (ShipmentEntity shipment in entityGrid.SelectedElements.OfType<ShipmentGridRow>().Select(r => r.Entity))
            {
                if (order == null)
                {
                    order = shipment.Order;
                }
                else
                {
                    if (order.OrderID != shipment.OrderID)
                    {
                        order = null;
                        break;
                    }
                }
            }
            return order;
        }

        #endregion

        #region Sorting

        /// <summary>
        /// The current sort applied to the grid is changing
        /// </summary>
        private void OnGridSortChanged(object sender, GridEventArgs e)
        {
            ApplySecondarySort();
        }

        /// <summary>
        /// Apply the secondary sort on the shipment number column, if necesaary
        /// </summary>
        private void ApplySecondarySort()
        {
            if (entityGrid.SortColumn == null)
            {
                return;
            }

            List<GridColumn> sortColumns = entityGrid.Columns.Cast<GridColumn>().Where(c => c.SortOrder != SortOrder.None).ToList();

            if (sortColumns.Contains(shipmentNumberSorter))
            {
                return;
            }

            // After the primary sort, then sort by order number, and within an order, the shipment number
            entityGrid.PrimaryGrid.SetSort(
               new GridColumn[] { sortColumns[0], orderGridPositionSorter, shipmentNumberSorter },
               new ListSortDirection[] { entityGrid.SortDirection, ListSortDirection.Ascending, ListSortDirection.Ascending });
        }

        /// <summary>
        /// Sort the grid using the current sort, for after we add\remove shipment rows.
        /// </summary>
        private void Resort()
        {
            // Ensure a resort
            entityGrid.PrimaryGrid.SetSort(new GridColumn[] { entityGrid.SortColumn }, new ListSortDirection[] { entityGrid.SortDirection });

            // Make the selection visible
            if (SelectedRowCount > 0)
            {
                entityGrid.SelectedElements[0].EnsureVisible();
            }
        }

        /// <summary>
        /// Open the grid settings window
        /// </summary>
        private void OnGridSettings(object sender, EventArgs e)
        {
            entityGrid.ShowColumnEditorDialog();
        }

        #endregion

        /// <summary>
        /// Creates a copy of the shipment, ready for processing
        /// </summary>
        private void CopyShipments(IEnumerable<ShipmentEntity> shipments, bool forReturn)
        {
            List<ShipmentEntity> createdShipments = new List<ShipmentEntity>();
            int failureCount = 0;

            int totalCount = 0;
            foreach (ShipmentEntity shipment in shipments)
            {
                totalCount++;
                ShipmentEntity copy = ShippingManager.CreateShipmentCopy(shipment);

                // mark it as a return
                if (forReturn)
                {
                    copy.ReturnShipment = true;
                }

                try
                {
                    // save 
                    ShippingManager.SaveShipment(copy);

                    using (SqlAdapter sqlAdapter = new SqlAdapter(true))
                    {
                        ValidatedAddressManager.CopyValidatedAddresses(sqlAdapter, shipment.ShipmentID, "Ship", copy.ShipmentID, "Ship");

                        sqlAdapter.Commit();
                    }

                    // remember for loading later
                    createdShipments.Add(copy);
                }
                catch (SqlForeignKeyException)
                {
                    failureCount++;
                }
            }

            // add and select the new shipments in the UI
            if (createdShipments.Count > 0)
            {
                AddShipments(createdShipments);
                SelectShipments(createdShipments);

                Resort();
            }

            // show any error messages
            if (failureCount > 0)
            {
                string message;
                if (totalCount == 1)
                {
                    message = "This order has been deleted, ShipWorks was unable to create a copy of the shipment.";
                }
                else
                {
                    message = "One or more of the orders have been deleted, ShipWorks was unable to copy every shipment.";
                }

                MessageHelper.ShowError(this, message);
            }
        }

        /// <summary>
        /// Copy shipment as a Return
        /// </summary>
        private void OnCopyAsReturn(object sender, EventArgs e)
        {
            ShipmentEntity selectedShipment = (SelectedShipments.Count() == 1) ? SelectedShipments.First() : null;
            if (selectedShipment != null)
            {
                // create a return shipment
                CopyShipments(new List<ShipmentEntity>() { selectedShipment }, true);
            }
        }

        /// <summary>
        /// Copy the shipment
        /// </summary>
        private void OnCopy(object sender, EventArgs e)
        {
            CopyShipments(SelectedShipments, false);
        }
    }
}
