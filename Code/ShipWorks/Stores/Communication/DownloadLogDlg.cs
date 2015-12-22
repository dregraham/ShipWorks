using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI;
using ShipWorks.UI.Utility;
using ComponentFactory.Krypton.Toolkit;
using Interapptive.Shared;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Grid.Columns;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Users;
using ShipWorks.Data.Grid;
using ShipWorks.UI.Controls.SandGrid;
using ShipWorks.Data.Grid.Columns.Definitions;
using Interapptive.Shared.UI;
using ShipWorks.Data.Caching;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Window for allowing the user to see the download log
    /// </summary>
    public partial class DownloadLogDlg : Form
    {
        ContextMenuStrip filterStoreMenu;
        ContextMenuStrip filterResultMenu;
        ContextMenuStrip filterInitiatedByMenu;

        int? dayRange = 0;
        long? storeID = null;
        DownloadResult? result = null;
        DownloadInitiatedBy? initiatedBy = null;

        // Used to be the entity cache, monitor for changes, and notify us when the grid needs refreshed
        EntityCacheEntityProvider entityProvider;

        static Guid gridSettingsKey = new Guid("{8E0179C8-2F20-4633-909B-60490369EBB4}");

        /// <summary>
        /// Constructor
        /// </summary>
        public DownloadLogDlg()
        {
            InitializeComponent();

            WindowStateSaver.Manage(this);

            ThemedBorderProvider themeBorder = new ThemedBorderProvider(panelGridArea);
            panelTools.StateNormal.Draw = ThemeInformation.VisualStylesEnabled ? InheritBool.True : InheritBool.False;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            CreateFilterMenus();

            entityGrid.ErrorProvider = new EntityGridRowErrorFieldProvider(DownloadFields.ErrorMessage);

            // Create the entity provider for caching and refreshing
            entityProvider = new EntityCacheEntityProvider(EntityType.DownloadEntity);
            entityProvider.EntityChangesDetected += new EntityCacheChangeMonitoredChangedEventHandler(OnEntityProviderChangeDetected);

            // Prepare for paging
            entityGrid.InitializeGrid();

            // Prepare configurable columns
            entityGrid.InitializeColumns(new StandardGridColumnStrategy(gridSettingsKey, GridColumnDefinitionSet.DownloadLog, InitializeDefaultGridLayout));
            entityGrid.SaveColumnsOnClose(this);

            UpdateFilterControlPositions();

            menuList.SelectedIndex = 0;
        }

        /// <summary>
        /// Create the default column layout to use for the grid
        /// </summary>
        private void InitializeDefaultGridLayout(GridColumnLayout layout)
        {
            layout.DefaultSortColumnGuid = DownloadLogColumnDefinitionFactory.CreateDefinitions()[DownloadFields.Started].ColumnGuid;
            layout.DefaultSortOrder = ListSortDirection.Descending;
        }

        /// <summary>
        /// Create the menu's to use for the filtering
        /// </summary>
        [NDependIgnoreLongMethod]
        private void CreateFilterMenus()
        {
            ///////////////
            // Store Menu
            filterStoreMenu = new ContextMenuStrip();

            ToolStripMenuItem allStoresItem = new ToolStripMenuItem("Any", null, OnChangeQueryStoreFilter);
            allStoresItem.Tag = null;
            allStoresItem.Checked = true;

            filterStoreMenu.Items.Add(allStoresItem);

            foreach (StoreEntity store in StoreManager.GetAllStores())
            {
                ToolStripMenuItem storeItem = new ToolStripMenuItem(store.StoreName, null, OnChangeQueryStoreFilter);
                storeItem.Tag = store.StoreID;

                filterStoreMenu.Items.Add(storeItem);
            }

            ////////////////
            // Result Menu
            filterResultMenu = new ContextMenuStrip();

            ToolStripMenuItem allResultsItem = new ToolStripMenuItem("Any", null, OnChangeQueryResultFilter);
            allResultsItem.Tag = null;
            allResultsItem.Checked = true;

            filterResultMenu.Items.Add(allResultsItem);

            // Get every type of download result
            foreach (EnumEntry<DownloadResult> result in EnumHelper.GetEnumList<DownloadResult>().Where(e => e.Value != DownloadResult.Unfinished))
            {
                ToolStripMenuItem resultItem = new ToolStripMenuItem(result.Description, result.Image, OnChangeQueryResultFilter);
                resultItem.Tag = result.Value;

                filterResultMenu.Items.Add(resultItem);
            }

            //////////////
            // Initiated By menu
            filterInitiatedByMenu = new ContextMenuStrip();

            ToolStripMenuItem allInitiationsItem = new ToolStripMenuItem("Any", null, OnChangeQueryReasonFilter);
            allInitiationsItem.Tag = null;
            allInitiationsItem.Checked = true;

            filterInitiatedByMenu.Items.Add(allInitiationsItem);

            // Add every reason
            foreach (EnumEntry<DownloadInitiatedBy> initiatedBy in EnumHelper.GetEnumList<DownloadInitiatedBy>())
            {
                ToolStripMenuItem initiatedByItem = new ToolStripMenuItem(initiatedBy.Description, null, OnChangeQueryReasonFilter);
                initiatedByItem.Tag = initiatedBy.Value;

                filterInitiatedByMenu.Items.Add(initiatedByItem);
            }
        }

        /// <summary>
        /// The entity provider detected changes to the underlying data.  The grid needs updated.
        /// </summary>
        private void OnEntityProviderChangeDetected(object sender, EntityCacheChangeMonitoredChangedEventArgs e)
        {
            if (e.Inserted.Count + e.Deleted.Count > 0)
            {
                entityGrid.ReloadGridRows();
            }
            else
            {
                entityGrid.UpdateGridRows();
            }
        }

        /// <summary>
        /// Something has changed that affects the current query filter
        /// </summary>
        private void OnChangeQueryDateFilter(object sender, EventArgs e)
        {
            switch (menuList.SelectedIndex)
            {
                case 0: dayRange = 0; break;
                case 1: dayRange = 6; break;
                case 2: dayRange = 29; break;
                case 3: dayRange = null; break;
            }

            UpdateQueryFilter();
        }

        /// <summary>
        /// Changing the store filtering option
        /// </summary>
        private void OnChangeQueryStoreFilter(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem) sender;
            SetRadioCheck(menuItem);

            filterStore.Text = menuItem.Text;
            storeID = (long?) menuItem.Tag;

            UpdateFilterControlPositions();
            UpdateQueryFilter();
        }

        /// <summary>
        /// The download result filter is changing
        /// </summary>
        private void OnChangeQueryResultFilter(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem) sender;
            SetRadioCheck(menuItem);

            filterResult.Text = menuItem.Text;
            result = (DownloadResult?) menuItem.Tag;

            UpdateFilterControlPositions();
            UpdateQueryFilter();
        }

        /// <summary>
        /// The download result filter is changing
        /// </summary>
        private void OnChangeQueryReasonFilter(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem) sender;
            SetRadioCheck(menuItem);

            filterInitiatedBy.Text = menuItem.Text;
            initiatedBy = (DownloadInitiatedBy?) menuItem.Tag;

            UpdateFilterControlPositions();
            UpdateQueryFilter();
        }

        /// <summary>
        /// Ensure only the given menu item is checked
        /// </summary>
        private void SetRadioCheck(ToolStripMenuItem menuItem)
        {
            foreach (ToolStripMenuItem otherItem in menuItem.Owner.Items.OfType<ToolStripMenuItem>())
            {
                otherItem.Checked = false;
            }

            menuItem.Checked = true;
        }

        /// <summary>
        /// Update the positions of the controls so that everything has room
        /// </summary>
        private void UpdateFilterControlPositions()
        {
            labelFilterStore.Left = filterResult.Right + 2;
            filterStore.Left = labelFilterStore.Right - 2;

            labelFilterInitiatedBy.Left = filterStore.Right + 2;
            filterInitiatedBy.Left = labelFilterInitiatedBy.Right - 2;
        }

        /// <summary>
        /// Update the filter that controls what the gateway shows
        /// </summary>
        private void UpdateQueryFilter()
        {
            RelationPredicateBucket bucket = new RelationPredicateBucket();
            
            // Date filtering
            if (dayRange != null)
            {               
                int utcOffset = ((TimeSpan) (DateTime.Now - DateTime.UtcNow)).Hours;

                EntityField2 startedField = DownloadFields.Started;
                startedField.ExpressionToApply = new DbFunctionCall("dbo.DatePartOnly(DATEADD(hour, " + utcOffset + ", {0}))", new object[] { DownloadFields.Started });

                bucket.PredicateExpression.Add(startedField >= DateTime.Now.AddDays(-dayRange.Value).Date);
            }

            // Store filtering
            if (storeID != null)
            {
                bucket.PredicateExpression.AddWithAnd(DownloadFields.StoreID == storeID.Value);
            }

            // Result filtering
            if (result != null)
            {
                bucket.PredicateExpression.AddWithAnd(DownloadFields.Result == (int) result.Value);
            }

            // Reason filtering
            if (initiatedBy != null)
            {
                bucket.PredicateExpression.AddWithAnd(DownloadFields.InitiatedBy == (int) initiatedBy.Value);
            }

            entityGrid.OpenGateway(new QueryableEntityGateway(entityProvider, bucket));
        }

        /// <summary>
        /// Clicking the link to change the current store filter
        /// </summary>
        private void OnClickStoreFilter(object sender, EventArgs e)
        {
            filterStoreMenu.Show(filterStore.Parent.PointToScreen(new Point(filterStore.Left, filterStore.Bottom)));
        }

        /// <summary>
        /// Clicking the link to change the current result filter
        /// </summary>
        private void OnClickResultFilter(object sender, EventArgs e)
        {
            filterResultMenu.Show(filterResult.Parent.PointToScreen(new Point(filterResult.Left, filterResult.Bottom)));
        }

        /// <summary>
        /// Clicking the link to change the current reason filter
        /// </summary>
        private void OnClickReasonFilter(object sender, EventArgs e)
        {
            filterInitiatedByMenu.Show(filterInitiatedBy.Parent.PointToScreen(new Point(filterInitiatedBy.Left, filterInitiatedBy.Bottom)));
        }

        /// <summary>
        /// Open the grid settings window
        /// </summary>
        private void OnGridSettings(object sender, EventArgs e)
        {
            entityGrid.ShowColumnEditorDialog();
        }

        /// <summary>
        /// Window has closed
        /// </summary>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            entityProvider.Dispose();
            entityProvider = null;
        }
    }
}
