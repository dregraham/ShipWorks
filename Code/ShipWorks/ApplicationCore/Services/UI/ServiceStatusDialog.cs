using ComponentFactory.Krypton.Toolkit;
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
using ShipWorks.UI.Utility;
using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace ShipWorks.ApplicationCore.Services.UI
{
    public partial class ServiceStatusDialog : Form
    {
        static readonly Guid gridSettingsKey = new Guid("{53EE16A4-9315-4D22-B768-58613546476B}");

        EntityCacheEntityProvider entityProvider;


        public ServiceStatusDialog()
        {
            InitializeComponent();

            WindowStateSaver.Manage(this, WindowStateSaverOptions.SizeOnly);

            new ThemedBorderProvider(panelGridArea);
            panelTools.StateNormal.Draw = ThemeInformation.VisualStylesEnabled ? InheritBool.True : InheritBool.False;
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            entityGrid.PrimaryGrid.NewRowType = typeof(ServiceGridRow);

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

        private void OnGridCellLinkClicked(object sender, GridHyperlinkClickEventArgs e)
        {
            EntityGridRow row = e.Row;

            if (row.EntityID == null)
            {
                MessageHelper.ShowMessage(this, "The data is not yet loaded.");
                return;
            }

            GridLinkAction action = (GridLinkAction)((GridActionDisplayType)e.Column.DisplayType).ActionData;

            if (action == GridLinkAction.Start)
            {
                // Start the service
            }
        }

        void InitializeDefaultGridLayout(GridColumnLayout layout)
        {
            layout.DefaultSortColumnGuid = ServiceStatusColumnDefinitionFactory.CreateDefinitions()[ServiceStatusFields.ComputerID].ColumnGuid;
            layout.DefaultSortOrder = ListSortDirection.Ascending;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            entityProvider.Dispose();
        }


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
        }
    }
}
