using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using Divelements.SandGrid;
using Interapptive.Shared.Utility;
using ShipWorks.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Properties;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms;
using ShipWorks.Stores.Communication;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Common.Threading;
using System.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Users.Audit;
using ShipWorks.Data.Model.FactoryClasses;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Window for managing all of the user's stores in ShipWorks
    /// </summary>
    public partial class StoreManagerDlg : Form
    {
        Font fontStrikeout;

        class BackgroundDeleteState
        {
            public ProgressProvider ProgressProvider { get; set; }
            public ProgressDisplayDelayer Delayer { get; set; }
            public StoreEntity Store { get; set; }
            public int EstimatedObjectCount { get; set; }
        }

        Dictionary<long, DateTime?> lastDownloadTimes;

        /// <summary>
        /// Constructor
        /// </summary>
        public StoreManagerDlg()
        {
            InitializeComponent();

            WindowStateSaver.Manage(this);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            UserSession.Security.DemandPermission(PermissionType.ManageStores);

            fontStrikeout = new Font(sandGrid.Font, FontStyle.Strikeout);

            // Grab the times that each store last downloaded.  Only need to do this once  upfront... if they download while the
            // window is open, not a big deal.
            lastDownloadTimes = StoreManager.GetLastDownloadTimes();

            editionGuiHelper.RegisterElement(addStore, Editions.EditionFeature.SingleStore);

            // Load the stores and update the UI
            LoadStores();
        }

        /// <summary>
        /// Load all the stores from the database
        /// </summary>
        private void LoadStores()
        {
            sandGrid.Rows.Clear();

            // Force a refresh
            StoreManager.CheckForChanges();

            foreach (StoreEntity store in StoreManager.GetAllStores())
            {
                string storeType = StoreTypeManager.GetType(store).StoreTypeName;

                GridRow row = new GridRow(new string[] { store.StoreName, storeType, GetLastDownloadDescription(store) });
                row.Tag = store;

                sandGrid.Rows.Add(row);

                UpdateStoreRowDisplay(store);
            }

            if (sandGrid.Rows.Count > 0)
            {
                sandGrid.Rows[0].Selected = true;
            }

            UpdateEditButtonState();
        }

        /// <summary>
        /// Update the display text and strikeout state of the row for the given store
        /// </summary>
        private void UpdateStoreRowDisplay(StoreEntity store)
        {
            bool enabled = store.Enabled;

            Color color = enabled ? sandGrid.ForeColor : Color.DimGray;
            Font font = enabled ? sandGrid.Font : fontStrikeout;

            var row = GetStoreRow(store);

            row.Cells[0].Text = store.StoreName;
            row.Cells[0].Image = Resources.store_16;

            // Apply the font
            row.Font = font;

            // Apply the color
            foreach (GridCell cell in row.Cells)
            {
                cell.ForeColor = color;
            }
        }

        /// <summary>
        /// Get the last download time as a descriptive string
        /// </summary>
        private string GetLastDownloadDescription(StoreEntity store)
        {
            DateTime? lastDownload;
            if (!lastDownloadTimes.TryGetValue(store.StoreID, out lastDownload))
            {
                lastDownload = null;
            }

            if (lastDownload == null)
            {
                return "Never";
            }

            return StringUtility.FormatFriendlyDateTime(lastDownload.Value);
        }

        /// <summary>
        /// Get \ set the store that is selected in the grid
        /// </summary>
        private StoreEntity SelectedStore
        {
            get
            {
                if (sandGrid.SelectedElements.Count != 1)
                {
                    return null;
                }

                return (StoreEntity) sandGrid.SelectedElements[0].Tag;
            }
            set
            {
                foreach (GridRow row in sandGrid.Rows)
                {
                    if (((StoreEntity)row.Tag).StoreID == value.StoreID)
                    {
                        row.Selected = true;
                        return;
                    }
                }

                throw new NotFoundException(string.Format("Store {0} not found in grid rows.", value.StoreName));
            }
        }

        /// <summary>
        /// Edit the settings of the selected store
        /// </summary>
        private void OnEditStore(object sender, EventArgs e)
        {
            EditStore(SelectedStore);
        }

        /// <summary>
        /// Row activated (initiate edit)
        /// </summary>
        private void OnRowActivated(object sender, GridRowEventArgs e)
        {
            EditStore(e.Row.Tag as StoreEntity);
        }

        /// <summary>
        /// Open the store settings editor the specified store
        /// </summary>
        private void EditStore(StoreEntity store)
        {
            Cursor.Current = Cursors.WaitCursor;

            using (StoreSettingsDlg dlg = new StoreSettingsDlg(store))
            {
                dlg.ShowDialog(this);

                UpdateStoreRowDisplay(store);
            }
        }

        /// <summary>
        /// Begin the rename operation on the selected store
        /// </summary>
        private void OnRename(object sender, EventArgs e)
        {
            ((GridRow) sandGrid.SelectedElements[0]).BeginEdit();
        }

        /// <summary>
        /// Rename operation is finalizing
        /// </summary>
        private void OnAfterRename(object sender, GridAfterEditEventArgs e)
        {
            StoreEntity store = e.Row.Tag as StoreEntity;
            string proposed = e.Value.ToString().Trim();

            if (proposed == store.StoreName || proposed.Length == 0)
            {
                e.Cancel = true;
                return;
            }

            IEntityFields2 fields = store.Fields.Clone();
            store.StoreName = proposed;

            try
            {
                StoreManager.SaveStore(store);
            }
            catch (DuplicateNameException ex)
            {
                MessageHelper.ShowInformation(this, ex.Message);

                // Reload it to get its name back the way it was
                store.Fields = fields;

                e.Cancel = true;
            }
        }

        /// <summary>
        /// Delete the selected store
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            using (StoreConfirmDeleteDlg dlg = new StoreConfirmDeleteDlg(SelectedStore.StoreName))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // Progress Provider
                    ProgressProvider progressProvider = new ProgressProvider();

                    // Progress Item
                    ProgressItem workProgress = new ProgressItem("Delete Store");
                    workProgress.Detail = string.Format("Deleting store '{0}'...", SelectedStore.StoreName);
                    workProgress.CanCancel = false;
                    progressProvider.ProgressItems.Add(workProgress);
                    workProgress.Starting();

                    // Proress Dialog
                    ProgressDlg progressDlg = new ProgressDlg(progressProvider);
                    progressDlg.Title = "Delete";
                    progressDlg.Description = "ShipWorks is deleting the store.";

                    // Create the progress delayer - don't show the progress window if it happens too quickly
                    ProgressDisplayDelayer delayer = new ProgressDisplayDelayer(progressDlg);

                    // Background info
                    BackgroundDeleteState backgroundState = new BackgroundDeleteState
                        {
                            ProgressProvider = progressProvider,
                            Delayer = delayer,
                            Store = SelectedStore,
                            EstimatedObjectCount = GetEstimatedObjectCount(SelectedStore.StoreID)
                        };

                    // Queue the thread to actually delete the store
                    ThreadPool.QueueUserWorkItem(
                        ExceptionMonitor.WrapWorkItem(AsyncDeleteStore, "deleting"),
                        backgroundState);

                    // Queue the monitor that will show progress
                    ThreadPool.QueueUserWorkItem(
                        ExceptionMonitor.WrapWorkItem(AsyncMonitorDeletionProgress, "deleting"),
                        backgroundState);

                    // Show the progrss window only after a certain amount of time goes by
                    delayer.ShowAfter(this, TimeSpan.FromSeconds(.25));
                }
            }
        }

        /// <summary>
        /// Get the estimated number of objects that are in the given store.  Used to provide deletion progress.
        /// </summary>
        private int GetEstimatedObjectCount(long storeID)
        {
            int count = 0;

            using (SqlAdapter adapter = new SqlAdapter(true, System.Transactions.IsolationLevel.ReadUncommitted))
            {
                count += OrderCollection.GetCount(adapter, OrderFields.StoreID == storeID);

                RelationPredicateBucket itemBucket = new RelationPredicateBucket(OrderFields.StoreID == storeID);
                itemBucket.Relations.Add(OrderEntity.Relations.OrderItemEntityUsingOrderID);
                count += adapter.GetDbCount(new OrderItemEntityFactory().CreateFields(), itemBucket);

                RelationPredicateBucket attBucket = new RelationPredicateBucket(OrderFields.StoreID == storeID);
                attBucket.Relations.Add(OrderEntity.Relations.OrderItemEntityUsingOrderID);
                attBucket.Relations.Add(OrderItemEntity.Relations.OrderItemAttributeEntityUsingOrderItemID);
                count += adapter.GetDbCount(new OrderItemAttributeEntityFactory().CreateFields(), attBucket);

                RelationPredicateBucket chargeBucket = new RelationPredicateBucket(OrderFields.StoreID == storeID);
                chargeBucket.Relations.Add(OrderEntity.Relations.OrderChargeEntityUsingOrderID);
                count += adapter.GetDbCount(new OrderChargeEntityFactory().CreateFields(), chargeBucket);

                RelationPredicateBucket paymentBucket = new RelationPredicateBucket(OrderFields.StoreID == storeID);
                paymentBucket.Relations.Add(OrderEntity.Relations.OrderPaymentDetailEntityUsingOrderID);
                count += adapter.GetDbCount(new OrderPaymentDetailEntityFactory().CreateFields(), paymentBucket);

                RelationPredicateBucket shipmentBucket = new RelationPredicateBucket(OrderFields.StoreID == storeID);
                shipmentBucket.Relations.Add(OrderEntity.Relations.ShipmentEntityUsingOrderID);
                count += adapter.GetDbCount(new ShipmentEntityFactory().CreateFields(), shipmentBucket);
            }

            return count;
        }

        /// <summary>
        /// Delete the store on the background thread
        /// </summary>
        private void AsyncDeleteStore(object userData)
        {
            BackgroundDeleteState state = (BackgroundDeleteState) userData;

            // We don't audit anything for deleting a store
            using (AuditBehaviorScope auditScope = new AuditBehaviorScope(AuditBehaviorDisabledState.Disabled))
            {
                DeletionService.DeleteStore(state.Store);
            }

            state.ProgressProvider.ProgressItems[0].Completed();

            BeginInvoke((MethodInvoker) delegate
                {
                    state.Delayer.NotifyComplete();

                    LoadStores();
                });
        }

        /// <summary>
        /// Monitor the deletion progress of a store to provide progress, and know when it's done
        /// </summary>
        private void AsyncMonitorDeletionProgress(object userData)
        {
            BackgroundDeleteState state = (BackgroundDeleteState) userData;

            ProgressItem progress = state.ProgressProvider.ProgressItems[0];

            // Keep checking for updates while the store is still not deleted
            while (!state.ProgressProvider.IsComplete)
            {
                int currentCountLeft = GetEstimatedObjectCount(state.Store.StoreID);

                progress.PercentComplete = state.EstimatedObjectCount > 0 ? Math.Min(((100 * (state.EstimatedObjectCount - currentCountLeft)) / state.EstimatedObjectCount), 100) : 0;

                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Add a new store to ShipWorks
        /// </summary>
        private void OnAddStore(object sender, EventArgs e)
        {
            StoreEntity newStore = AddStoreWizard.RunWizard(this);

            if (newStore != null)
            {
                LoadStores();

                SelectedStore = newStore;
                ActiveControl = sandGrid;
            }
        }

        /// <summary>
        /// Selected store changed
        /// </summary>
        private void OnChangeGridSelection(object sender, SelectionChangedEventArgs e)
        {
            UpdateEditButtonState();
        }

        /// <summary>
        /// Update the state of the edit buttons based on selection
        /// </summary>
        private void UpdateEditButtonState()
        {
            edit.Enabled = sandGrid.SelectedElements.Count == 1;
            delete.Enabled = sandGrid.SelectedElements.Count == 1;
            rename.Enabled = sandGrid.SelectedElements.Count == 1;

            editionGuiHelper.UpdateUI();
        }

        /// <summary>
        /// Get the grid row for the given store
        /// </summary>
        private GridRow GetStoreRow(StoreEntity store)
        {
            foreach (GridRow row in sandGrid.Rows)
            {
                if (row.Tag == store)
                {
                    return row;
                }
            }

            throw new InvalidOperationException("Could not find grid row for store " + store.StoreName);
        }
    }
}