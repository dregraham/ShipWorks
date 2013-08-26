using System.Linq;
using System.Threading.Tasks;
using ComponentFactory.Krypton.Toolkit;
using Divelements.SandGrid;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.ApplicationCore.Services.Hosting;
using ShipWorks.Data.Caching;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.Definitions;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.UI.Utility;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using ShipWorks.Users;


namespace ShipWorks.ApplicationCore.Services.UI
{
    public partial class ServiceStatusDialog : Form
    {
        static readonly Guid gridSettingsKey = new Guid("{53EE16A4-9315-4D22-B768-58613546476B}");

        private bool startingService;
        EntityCacheEntityProvider entityProvider;
        private Timer startingServiceTimer;


        public ServiceStatusDialog()
        {
            InitializeComponent();

            WindowStateSaver.Manage(this, WindowStateSaverOptions.SizeOnly);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            startingServicePanel.Visible = false;
            
            //Service status includes computer info too
            PrefetchPath2 prefetch = new PrefetchPath2(EntityType.ServiceStatusEntity);
            prefetch.Add(ServiceStatusEntity.PrefetchPathComputer);

            entityProvider = new EntityCacheEntityProvider(EntityType.ServiceStatusEntity, prefetch);
            entityProvider.EntityChangesDetected += OnEntityProviderChangeDetected;

            //Prepare for paging
            entityGrid.InitializeGrid();

            //Prepare configurable columns
            entityGrid.InitializeColumns(new StandardGridColumnStrategy(gridSettingsKey, GridColumnDefinitionSet.ServiceStatus, InitializeDefaultGridLayout));
            entityGrid.SaveColumnsOnClose(this);

            entityGrid.Renderer = AppearanceHelper.CreateWindowsRenderer();
            entityGrid.RowHighlightType = RowHighlightType.None;

            //Open the data
            entityGrid.OpenGateway(new QueryableEntityGateway(entityProvider, new RelationPredicateBucket()));

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
            UpdateStartingServiceUI(true);
            MessageBox.Show("The service was not able to start.", "ShipWorks", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            
            entityProvider.Dispose();
        }

        /// <summary>
        /// A service has changed
        /// </summary>
        void OnEntityProviderChangeDetected(object sender, EntityCacheChangeMonitoredChangedEventArgs e)
        {
            ServiceStatusManager.CheckForChangesNeeded();

            if (e.Inserted.Count + e.Deleted.Count > 0)
            {
                entityGrid.ReloadGridRows();
            }
            else
            {
                entityGrid.UpdateGridRows();
            }

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
        }
    }
}
