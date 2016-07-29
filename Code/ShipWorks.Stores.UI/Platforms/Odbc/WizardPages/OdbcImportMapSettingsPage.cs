using System;
using Autofac.Features.Indexed;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels;
using ShipWorks.UI.Wizard;
using IWin32Window = System.Windows.Interop.IWin32Window;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    public partial class OdbcImportMapSettingsPage : AddStoreWizardPage, IOdbcWizardPage, IWin32Window
    {
        private readonly IMessageHelper messageHelper;
        private readonly IOdbcDataSourceService dataSourceService;
        private readonly IIndex<string, IOdbcMapSettingsControlViewModel> viewModelFactory;
        private readonly IOdbcSchema schema;
        private IOdbcMapSettingsControlViewModel viewModel;
        private OdbcStoreEntity store;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingPage"/> class.
        /// </summary>
        public OdbcImportMapSettingsPage(IMessageHelper messageHelper,
            IOdbcDataSourceService dataSourceService,
            IIndex<string, IOdbcMapSettingsControlViewModel> viewModelFactory,
            IOdbcSchema schema)
        {
            this.messageHelper = messageHelper;
            this.dataSourceService = dataSourceService;
            this.viewModelFactory = viewModelFactory;
            this.schema = schema;
            InitializeComponent();
            SteppingInto += OnSteppingInto;
            StepNext += OnNext;
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
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void OnSteppingInto(object sender, EventArgs eventArgs)
        {
            store = GetStore<OdbcStoreEntity>();

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
        }
    }
}