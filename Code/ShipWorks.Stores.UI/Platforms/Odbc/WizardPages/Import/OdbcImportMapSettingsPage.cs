using Autofac.Features.Indexed;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels;
using ShipWorks.UI.Wizard;
using System;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Import
{
    public partial class OdbcImportMapSettingsPage : AddStoreWizardPage, IOdbcWizardPage
    {
        private readonly IOdbcDataSourceService dataSourceService;
        private readonly IIndex<string, IOdbcMapSettingsControlViewModel> viewModelFactory;
        private readonly IOdbcSchema schema;
        private readonly Func<string, IDialog> dialogFactory;
        private IOdbcMapSettingsControlViewModel viewModel;
        private OdbcStoreEntity store;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportMappingPage"/> class.
        /// </summary>
        public OdbcImportMapSettingsPage(IOdbcDataSourceService dataSourceService,
            IIndex<string, IOdbcMapSettingsControlViewModel> viewModelFactory,
            IOdbcSchema schema,
            Func<string, IDialog> dialogFactory)
        {
            this.dataSourceService = dataSourceService;
            this.viewModelFactory = viewModelFactory;
            this.schema = schema;
            this.dialogFactory = dialogFactory;
            
            InitializeComponent();
            SteppingInto += OnSteppingInto;
            StepNext += OnNext;
            StepBack += OnBack;
        }

        /// <summary>
        /// The position in which to show this wizard page
        /// </summary>
        public int Position => 1;

        /// <summary>
        /// Save the map to the ODBC Store
        /// </summary>
        private void OnNext(object sender, WizardStepEventArgs e)
        {
            if (store == null)
            {
                store = GetStore<OdbcStoreEntity>();
            }

            if (!viewModel.ValidateRequiredMapSettings())
            {
                e.NextPage = this;
                return;
            }

            viewModel.SaveMapSettings(store);
            
            // Display the query warning dlg if proceeding to a query page
            if (store.ImportColumnSourceType != (int) OdbcColumnSourceType.Table)
            {
                IDialog warningDlg = dialogFactory("OdbcCustomQueryWarningDlg");
                warningDlg.LoadOwner(Wizard);
                warningDlg.ShowDialog();   
            }
        }

        /// <summary>
        /// Save the map settings to the ODBC Store
        /// </summary>
        private void OnBack(object sender, WizardStepEventArgs e)
        {
            if (store == null)
            {
                store = GetStore<OdbcStoreEntity>();
            }

            viewModel.SaveMapSettings(store);
        }

        /// <summary>
        /// Stepping into the map settings page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            store = GetStore<OdbcStoreEntity>();

            if (store.WarehouseStoreID.HasValue && !store.SetupComplete)
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = false;
                return;
            }

            IOdbcDataSource selectedDataSource = dataSourceService.GetImportDataSource(store);

            // Create new ViewModel when one does not exist, or a new data source is selected. This means clicking
            // back on the mapping page and not changing the data source will keep any mappings made, but selecting
            // a new data source and clicking next, will reset all mappings.
            if (viewModel == null ||
                !viewModel.DataSource.ConnectionString.Equals(selectedDataSource.ConnectionString,
                    StringComparison.Ordinal))
            {
                viewModel = viewModelFactory["Import"];
                viewModel.Load(selectedDataSource, schema, store.ImportColumnSource, store);
                mapSettingsControl.DataContext = viewModel;
            }
            else
            {
                viewModel.LoadMapSettings(store);
            }
        }
    }
}
