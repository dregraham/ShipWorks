using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels;
using ShipWorks.UI.Wizard;
using System;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    public partial class OdbcUploadMappingPage : AddStoreWizardPage, IOdbcWizardPage
    {
        private readonly Func<IOdbcDataSource> dataSourceFactory;
        private readonly Func<string, IOdbcColumnSource> columnSourceFactory;
        private IOdbcUploadMappingControlViewModel viewModel;
        private OdbcStoreEntity store;
        private string previousColumnSource;
        private readonly Func<IOdbcUploadMappingControlViewModel> viewModelFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingPage"/> class.
        /// </summary>
        public OdbcUploadMappingPage(Func<IOdbcDataSource> dataSourceFactory,
            Func<IOdbcUploadMappingControlViewModel> viewModelFactory,
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
        /// Gets the position.
        /// </summary>
        public int Position => 5;

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
        /// Called when [stepping into].
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            store = GetStore<OdbcStoreEntity>();

            if (store.UploadStrategy == (int) OdbcShipmentUploadStrategy.DoNotUpload)
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = false;
                return;
            }

            if (store.UploadColumnSourceType == (int) OdbcColumnSourceType.CustomQuery)
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = false;
                return;
            }

            string currentColumnSource = store.UploadColumnSource;

            // Only load column source when the page is first loaded or the column source changes.
            if (string.IsNullOrWhiteSpace(previousColumnSource) ||
                !previousColumnSource.Equals(currentColumnSource, StringComparison.Ordinal))
            {
                IOdbcDataSource selectedDataSource = dataSourceFactory();

                selectedDataSource.Restore(store.ConnectionString);

                IOdbcColumnSource columnSource = columnSourceFactory(currentColumnSource);

                columnSource.Load(selectedDataSource, currentColumnSource,
                    (OdbcColumnSourceType)store.ImportSourceType);

                viewModel = viewModelFactory();
                mappingControl.DataContext = viewModel;
                viewModel.LoadColumnSource(columnSource);
                previousColumnSource = currentColumnSource;
            }
        }
    }
}
