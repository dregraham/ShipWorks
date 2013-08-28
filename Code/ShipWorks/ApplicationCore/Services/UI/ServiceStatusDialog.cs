using System.Linq;
using Divelements.SandGrid;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Data.Caching;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.Definitions;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using ShipWorks.Users;
using System.Collections.Generic;
using ShipWorks.Actions.Triggers;


namespace ShipWorks.ApplicationCore.Services.UI
{
    /// <summary>
    /// A dialog box showing the list of computers that require the ShipWorks background process/service to 
    /// be running based on the configuration of any scheduled actions in the system.
    /// </summary>
    public partial class ServiceStatusDialog : Form
    {
        static readonly Guid gridSettingsKey = new Guid("{53EE16A4-9315-4D22-B768-58613546476B}");

        private bool startingService;
        private Timer startingServiceTimer;

        private readonly Timer dataRefreshTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceStatusDialog"/> class.
        /// </summary>
        public ServiceStatusDialog()
        {
            InitializeComponent();

            WindowStateSaver.Manage(this, WindowStateSaverOptions.SizeOnly);

            // Keep the timer disabled until after the initial population of the grid
            dataRefreshTimer = new Timer { Interval = 10000, Enabled = false };
            dataRefreshTimer.Tick += OnDataRefreshTimerTick;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            startingServicePanel.Visible = false;
            
            //Prepare for paging
            entityGrid.InitializeGrid();

            //Prepare configurable columns
            entityGrid.InitializeColumns(new StandardGridColumnStrategy(gridSettingsKey, GridColumnDefinitionSet.ServiceStatus, InitializeDefaultGridLayout));
            entityGrid.SaveColumnsOnClose(this);

            entityGrid.Renderer = AppearanceHelper.CreateWindowsRenderer();
            entityGrid.RowHighlightType = RowHighlightType.None;

            // Load the data and start the timer to get data refreshed
            LoadData();
            dataRefreshTimer.Enabled = true;

            entityGrid.GridCellLinkClicked += OnGridCellLinkClicked;
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
            dataRefreshTimer.Enabled = false;
            LoadData();

            dataRefreshTimer.Enabled = true;
        }


        /// <summary>
        /// Loads the (static) list of computers/services that need to be running into the gateway and use it 
        /// to populate the contents of the grid
        /// </summary>
        private void LoadData()
        {
            List<ServiceStatusEntity> entities = ServiceStatusManager.GetComputersRequiringShipWorksService();
            LocalCollectionEntityGateway<ServiceStatusEntity> requiredServicesGateway = new LocalCollectionEntityGateway<ServiceStatusEntity>(entities);

            entityGrid.OpenGateway(requiredServicesGateway);
        }

        /// <summary>
        /// The start link was clicked on the grid
        /// </summary>
        private void OnGridCellLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            GridLinkAction action = (GridLinkAction)((GridActionDisplayType)e.Column.DisplayType).ActionData;

            if (action == GridLinkAction.Start)
            {
                startingService = true;
                UpdateStartingServiceUI(false);
                ShipWorksServiceBase.RunAllInBackground();
                startingServiceTimer = new Timer { Interval = 30000, Enabled = true };
                startingServiceTimer.Tick += StartingServiceTimerOnTick;
            }
        }

        /// <summary>
        /// The start service timer elapsed, which means the service failed to start
        /// </summary>
        private void StartingServiceTimerOnTick(object sender, EventArgs e)
        {
            // If a service is starting, see if it has started successfully
            if (startingService)
            {
                ServiceStatusEntity service = ServiceStatusManager.GetServiceStatus(UserSession.Computer.ComputerID, ShipWorksServiceType.Scheduler);

                if (service.GetStatus() == ServiceStatus.Running)
                {
                    UpdateStartingServiceUI(true);
                    startingService = false;
                }
            }
            else
            {
                UpdateStartingServiceUI(true);
                MessageBox.Show("The service was not able to start.", "ShipWorks", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Show or hide the UI that lets the user know that the service is starting.
        /// </summary>
        /// <param name="isComplete">Is the process just starting or is it finished?</param>
        private void UpdateStartingServiceUI(bool isComplete)
        {
            // Enable/disable the timer based on whether the process is complete; we don't want the 
            // data being refreshed while the service is still starting due to the 30 second wait
            // to avoid confusion (i.e. the service is started, the data is reloaded causing the 
            // row to be removed, but the "starting service..." UI is still visible)
            dataRefreshTimer.Enabled = isComplete;

            if (isComplete)
            {
                // We need to get rid of the timer if the startup is complete
                startingServiceTimer.Tick -= StartingServiceTimerOnTick;
                startingServiceTimer.Dispose();
                startingServiceTimer = null;
            }

            // Refresh the data source of the grid
            LoadData();

            entityGrid.Enabled = isComplete;
            startingServicePanel.Visible = !isComplete;
        }

        /// <summary>
        /// Set up the default grid layout
        /// </summary>
        /// <param name="layout"></param>
        void InitializeDefaultGridLayout(GridColumnLayout layout)
        {
            layout.DefaultSortColumnGuid = ServiceStatusColumnDefinitionFactory.CreateDefinitions()[ServiceStatusFields.ComputerID].ColumnGuid;
            layout.DefaultSortOrder = ListSortDirection.Ascending;
        }

        /// <summary>
        /// The form has been closed
        /// </summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (startingServiceTimer != null)
            {
                startingServiceTimer.Tick -= StartingServiceTimerOnTick;
                startingServiceTimer.Dispose();
            }

            if (dataRefreshTimer != null)
            {
                dataRefreshTimer.Tick -= OnDataRefreshTimerTick;
                dataRefreshTimer.Dispose();
            }
        }
    }
}
