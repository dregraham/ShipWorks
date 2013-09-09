using System.Linq;
using Divelements.SandGrid;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.Definitions;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;


namespace ShipWorks.ApplicationCore.Services.UI
{
    /// <summary>
    /// A dialog box showing the list of computers that require the ShipWorks background process/service to 
    /// be running based on the configuration of any scheduled actions in the system.
    /// </summary>
    public partial class ServiceStatusDialog : Form
    {
        static readonly Guid gridSettingsKey = new Guid("{53EE16A4-9315-4D22-B768-58613546476B}");
        GridHiddenSortColumn<ServiceStatusEntityGridRow> orderGridPositionSorter = new GridHiddenSortColumn<ServiceStatusEntityGridRow>(r => r.SortIndex);

        private int startingServiceChecksRemaining;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceStatusDialog"/> class.
        /// </summary>
        public ServiceStatusDialog()
        {
            InitializeComponent();

            WindowStateSaver.Manage(this, WindowStateSaverOptions.SizeOnly);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            startingServicePanel.Visible = false;

            //Prepare configurable columns
            entityGrid.InitializeColumns(new StandardGridColumnStrategy(gridSettingsKey, 
                GridColumnDefinitionSet.ServiceStatus,
                InitializeDefaultGridLayout,
                new List<GridColumn> { orderGridPositionSorter }));
            entityGrid.SaveColumnsOnClose(this);
            entityGrid.SortChanged += OnEntityGridSortChanged;

            entityGrid.Renderer = AppearanceHelper.CreateWindowsRenderer();
            entityGrid.RowHighlightType = RowHighlightType.None;

            // Load the data and start the timer to get data refreshed
            LoadData();
            dataRefreshTimer.Start();

            entityGrid.GridCellLinkClicked += OnGridCellLinkClicked;
        }

        /// <summary>
        /// Sorting of the entity grid has changed
        /// </summary>
        private void OnEntityGridSortChanged(object sender, GridEventArgs gridEventArgs)
        {
            ApplySecondarySort();
        }

        /// <summary>
        /// Called when [data refresh timer tick] to reload/refresh the data in the entity grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnDataRefreshTimerTick(object sender, EventArgs eventArgs)
        {
            // Disable the timer until after the data has been refreshed to avoid events queuing
            // up if the database is slow to respond
            dataRefreshTimer.Stop();

            // Force a reload of the services from the database because the heartbeat is paused
            // while a dialog is open, and if the services are not refreshed, the UI will think
            // the service failed to start
            ServiceStatusManager.CheckForChangesNeeded();

            CheckForStartingService();
            LoadData();

            dataRefreshTimer.Start();
        }

        /// <summary>
        /// Loads the (static) list of computers/services that need to be running into the gateway and use it 
        /// to populate the contents of the grid
        /// </summary>
        private void LoadData()
        {
            List<ServiceStatusEntity> entities = ServiceStatusManager.GetComputersRequiringShipWorksService();

            entityGrid.Rows.Clear();
            int secondarySortIndex = 0;

            foreach (ServiceStatusEntity serviceStatus in entities)
            {
                entityGrid.Rows.Add(new ServiceStatusEntityGridRow(serviceStatus, secondarySortIndex++));
            }

            ApplySort();
        }

        /// <summary>
        /// Checks whether the application is currently trying to start a service
        /// </summary>
        private void CheckForStartingService()
        {
            // If a service is starting, see if it has started successfully
            if (startingServiceChecksRemaining <= 0)
            {
                return;
            }

            --startingServiceChecksRemaining;
            ServiceStatusEntity service = ServiceStatusManager.GetServiceStatus(UserSession.Computer.ComputerID, ShipWorksServiceType.Scheduler);

            if (service.GetStatus() == ServiceStatus.Running)
            {
                ShowStartingServiceUI(false);
                startingServiceChecksRemaining = 0;
            }
            else if (startingServiceChecksRemaining == 0)
            {
                ShowStartingServiceUI(false);
                MessageBox.Show("The service was not able to start.", "ShipWorks", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Apply the sort, adding the computer name column to stop grid rows from moving when the data is reloaded
        /// </summary>
        private void ApplySort()
        {
            entityGrid.PrimaryGrid.SetSort(
                new[] { entityGrid.SortColumn }, 
                new[] { entityGrid.SortDirection });
        }

        /// <summary>
        /// Apply the secondary sort on the hidden order column, if necesaary
        /// </summary>
        private void ApplySecondarySort()
        {
            if (entityGrid.SortColumn == null)
            {
                return;
            }

            List<GridColumn> sortColumns = entityGrid.Columns.Cast<GridColumn>().Where(c => c.SortOrder != SortOrder.None).ToList();

            if (sortColumns.Contains(orderGridPositionSorter))
            {
                return;
            }

            // After the primary sort, then sort by order number, and within an order, the shipment number
            entityGrid.PrimaryGrid.SetSort(
               new[] { sortColumns[0], orderGridPositionSorter },
               new[] { entityGrid.SortDirection, ListSortDirection.Ascending });
        }

        /// <summary>
        /// The start link was clicked on the grid
        /// </summary>
        private void OnGridCellLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            GridLinkAction action = (GridLinkAction)((GridActionDisplayType)e.Column.DisplayType).ActionData;

            if (action == GridLinkAction.Start)
            {
                startingServiceChecksRemaining = 30000 / dataRefreshTimer.Interval;
                ShowStartingServiceUI(true);
                ShipWorksServiceBase.RunAllInBackground();
            }
        }

        /// <summary>
        /// Show or hide the UI that lets the user know that the service is starting.
        /// </summary>
        /// <param name="isStarting">Is the process just starting or is it finished?</param>
        private void ShowStartingServiceUI(bool isStarting)
        {
            entityGrid.Enabled = !isStarting;
            startingServicePanel.Visible = isStarting;
        }

        /// <summary>
        /// Set up the default grid layout
        /// </summary>
        /// <param name="layout"></param>
        private void InitializeDefaultGridLayout(GridColumnLayout layout)
        {
            layout.DefaultSortColumnGuid = ServiceStatusColumnDefinitionFactory.CreateDefinitions()[ServiceStatusFields.ComputerID].ColumnGuid;
            layout.DefaultSortOrder = ListSortDirection.Ascending;
        }
    }
}
