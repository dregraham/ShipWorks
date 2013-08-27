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
    public partial class ServiceStatusDialog : Form
    {
        static readonly Guid gridSettingsKey = new Guid("{53EE16A4-9315-4D22-B768-58613546476B}");

        private bool startingService;
        private Timer startingServiceTimer;

        private LocalCollectionEntityGateway<ServiceStatusEntity> requiredServicesGateway;


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
            
            //Prepare for paging
            entityGrid.InitializeGrid();

            //Prepare configurable columns
            entityGrid.InitializeColumns(new StandardGridColumnStrategy(gridSettingsKey, GridColumnDefinitionSet.ServiceStatus, InitializeDefaultGridLayout));
            entityGrid.SaveColumnsOnClose(this);

            entityGrid.Renderer = AppearanceHelper.CreateWindowsRenderer();
            entityGrid.RowHighlightType = RowHighlightType.None;

            //Open the data
            requiredServicesGateway = new LocalCollectionEntityGateway<ServiceStatusEntity>(ServiceStatusManager.GetComputersRequiringShipWorksService());
            entityGrid.OpenGateway(requiredServicesGateway);

            entityGrid.GridCellLinkClicked += OnGridCellLinkClicked;
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
        /// Show or hide the ui that lets the user know that the service is starting
        /// </summary>
        /// <param name="isComplete">Is the process just starting or is it finished?</param>
        private void UpdateStartingServiceUI(bool isComplete)
        {
            // We need to get rid of the timer if the startup is complete
            if (isComplete)
            {
                startingServiceTimer.Tick -= StartingServiceTimerOnTick;
                startingServiceTimer.Dispose();
                startingServiceTimer = null;
            }

            // Refresh the data source of the grid
            requiredServicesGateway = new LocalCollectionEntityGateway<ServiceStatusEntity>(ServiceStatusManager.GetComputersRequiringShipWorksService());
            entityGrid.OpenGateway(requiredServicesGateway);

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
        }
    }
}
