using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.UI.Wizard;
using System;
using System.Windows.Interop;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    /// <summary>
    /// Wizard page for choosing an import map to load or to start creating a new one
    /// </summary>
    public partial class OdbcImportFieldMappingPage : AddStoreWizardPage, IOdbcWizardPage, IWin32Window
    {
        private readonly IOdbcDataSourceService dataSourceService;
        private readonly Func<string, IOdbcColumnSource> columnSourceFactory;
        private IOdbcImportFieldMappingControlViewModel viewModel;
        private OdbcStoreEntity store;
        private const string CustomQueryColumnSourceName = "Custom Import";
        private string previousColumnSource;
        private OdbcImportStrategy? previousImportStrategy = null;
        private readonly Func<IOdbcImportFieldMappingControlViewModel> viewModelFactory;


        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingPage"/> class.
        /// </summary>
        public OdbcImportFieldMappingPage(IOdbcDataSourceService dataSourceService,
            Func<IOdbcImportFieldMappingControlViewModel> viewModelFactory,
            Func<string, IOdbcColumnSource> columnSourceFactory)
        {
            this.dataSourceService = dataSourceService;
            this.viewModelFactory = viewModelFactory;
            this.columnSourceFactory = columnSourceFactory;
            InitializeComponent();
            SteppingInto += OnSteppingInto;
            StepNext += OnNext;
        }

        /// <summary>
        /// The position in which to show this wizard page
        /// </summary>
        public int Position => 2;

        /// <summary>
        /// Save the map to the ODBC Store
        /// </summary>
        private void OnNext(object sender, WizardStepEventArgs e)
        {
            if (store == null)
            {
                store = GetStore<OdbcStoreEntity>();
            }

            if (!viewModel.ValidateRequiredMappingFields())
            {
                e.NextPage = this;
                return;
            }

            viewModel.Save(store);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            store = GetStore<OdbcStoreEntity>();

            OdbcImportStrategy currentImportStrategy = (OdbcImportStrategy) store.ImportStrategy;
            string currentColumnSource = store.ImportColumnSource;

            // Only load column source when the page is first loaded or the column source changes.
            if (string.IsNullOrWhiteSpace(previousColumnSource) ||
                !previousColumnSource.Equals(currentColumnSource, StringComparison.Ordinal))
            {
                IOdbcDataSource selectedDataSource = dataSourceService.GetImportDataSource(store);

                string columnSourceName = store.ImportColumnSourceType == (int) OdbcColumnSourceType.Table ?
                    currentColumnSource :
                    CustomQueryColumnSourceName;

                IOdbcColumnSource columnSource = columnSourceFactory(columnSourceName);

                columnSource.Load(selectedDataSource, currentColumnSource,
                    (OdbcColumnSourceType) store.ImportColumnSourceType);

                viewModel = viewModelFactory();
                mappingControl.DataContext = viewModel;
                viewModel.LoadColumnSource(columnSource);
                viewModel.LoadDownloadStrategy(currentImportStrategy);
                previousColumnSource = currentColumnSource;
            }

            // Only load download strategy when the page is first loaded or the download strategy changes.
            if (previousImportStrategy == null ||
                previousImportStrategy != currentImportStrategy)
            {
                viewModel.LoadDownloadStrategy(currentImportStrategy);

                previousImportStrategy = currentImportStrategy;
            }
        }
    }
}
