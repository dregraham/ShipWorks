using ComponentFactory.Krypton.Toolkit;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Data.Caching;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.Definitions;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.UI.Utility;
using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace ShipWorks.ApplicationCore.WindowsServices
{
    public partial class ServiceStatusDialog : Form
    {
        static readonly Guid gridSettingsKey = new Guid("{53EE16A4-9315-4D22-B768-58613546476B}");

        EntityCacheEntityProvider entityProvider;


        public ServiceStatusDialog()
        {
            InitializeComponent();

            WindowStateSaver.Manage(this, WindowStateSaverOptions.SizeOnly);

            ThemedBorderProvider themeBorder = new ThemedBorderProvider(panelGridArea);
            panelTools.StateNormal.Draw = ThemeInformation.VisualStylesEnabled ? InheritBool.True : InheritBool.False;
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //entityGrid.PrimaryGrid.NewRowType = typeof(ShipWorks.Actions.WindowsServiceGridRow);

            //Service status includes computer info too
            PrefetchPath2 prefetch = new PrefetchPath2(EntityType.WindowsServiceEntity);
            prefetch.Add(WindowsServiceEntity.PrefetchPathComputer);

            entityProvider = new EntityCacheEntityProvider(EntityType.WindowsServiceEntity, prefetch);
            entityProvider.EntityChangesDetected += new EntityCacheChangeMonitoredChangedEventHandler(OnEntityProviderChangeDetected);

            //Prepare for paging
            entityGrid.InitializeGrid();

            //Prepare configurable columns
            entityGrid.InitializeColumns(new StandardGridColumnStrategy(gridSettingsKey, GridColumnDefinitionSet.ServiceStatus, InitializeDefaultGridLayout));
            entityGrid.SaveColumnsOnClose(this);

            entityGrid.Renderer = AppearanceHelper.CreateWindowsRenderer();

            //Open the data
            entityGrid.OpenGateway(new QueryableEntityGateway(entityProvider, new RelationPredicateBucket()));
        }

        void InitializeDefaultGridLayout(GridColumnLayout layout)
        {
            layout.DefaultSortColumnGuid = ServiceStatusColumnDefinitionFactory.CreateDefinitions()[ComputerFields.Name].ColumnGuid;
            layout.DefaultSortOrder = ListSortDirection.Ascending;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            entityProvider.Dispose();
        }


        void OnEntityProviderChangeDetected(object sender, EntityCacheChangeMonitoredChangedEventArgs e)
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

        void OnEditGridSettings(object sender, EventArgs e)
        {
            entityGrid.ShowColumnEditorDialog();
        }
    }
}
