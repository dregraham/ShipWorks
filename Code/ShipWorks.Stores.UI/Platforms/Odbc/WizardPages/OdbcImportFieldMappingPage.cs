using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.UI.Wizard;
using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
using log4net;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    /// <summary>
    /// Wizard page for choosing an import map to load or to start creating a new one
    /// </summary>
    public partial class OdbcImportFieldMappingPage : AddStoreWizardPage, IOdbcWizardPage, IWin32Window
    {
        private readonly Func<IOdbcDataSource> dataSourceFactory;
        private readonly Func<string, IOdbcColumnSource> columnSourceFactory;
        private IOdbcImportFieldMappingControlViewModel viewModel;
        private OdbcStoreEntity store;
        private const string CustomQueryColumnSourceName = "Custom Import";
        private string previousColumnSource;
        private OdbcDownloadStrategy? previousDownloadStrategy = null;
        private readonly Func<IOdbcImportFieldMappingControlViewModel> viewModelFactory;


        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingPage"/> class.
        /// </summary>
        public OdbcImportFieldMappingPage(Func<IOdbcDataSource> dataSourceFactory,
            Func<IOdbcImportFieldMappingControlViewModel> viewModelFactory,
            Func<string, IOdbcColumnSource> columnSourceFactory)
        {
            this.dataSourceFactory = dataSourceFactory;
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
        /// <param name="eventArgs">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void OnSteppingInto(object sender, EventArgs eventArgs)
        {
            store = GetStore<OdbcStoreEntity>();

            OdbcDownloadStrategy currentDownloadStrategy = (OdbcDownloadStrategy) store.OdbcDownloadStrategy;
            string currentColumnSource = store.OdbcColumnSource;

            // Only load column source when the page is first loaded or the column source changes.
            if (string.IsNullOrWhiteSpace(previousColumnSource) ||
                !previousColumnSource.Equals(currentColumnSource, StringComparison.Ordinal))
            {
                IOdbcDataSource selectedDataSource = dataSourceFactory();

                selectedDataSource.Restore(store.ConnectionString);

                string columnSourceName = store.OdbcColumnSourceType == (int) OdbcColumnSourceType.Table ?
                    currentColumnSource :
                    CustomQueryColumnSourceName;

                IOdbcColumnSource columnSource = columnSourceFactory(columnSourceName);

                columnSource.Load(selectedDataSource, currentColumnSource,
                    (OdbcColumnSourceType) store.OdbcColumnSourceType);

                viewModel = viewModelFactory();
                mappingControl.DataContext = viewModel;
                viewModel.LoadColumnSource(columnSource);
                viewModel.LoadDownloadStrategy(currentDownloadStrategy);
                previousColumnSource = currentColumnSource;
            }

            // Only load download strategy when the page is first loaded or the download strategy changes.
            if (previousDownloadStrategy == null ||
                previousDownloadStrategy != currentDownloadStrategy)
            {
                viewModel.LoadDownloadStrategy(currentDownloadStrategy);

                previousDownloadStrategy = currentDownloadStrategy;
            }
        }
    }
}
