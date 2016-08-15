using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Upload;
using ShipWorks.UI.Wizard;
using System;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Upload
{
    public partial class OdbcUploadMappingPage : AddStoreWizardPage, IOdbcWizardPage
    {
        private readonly IOdbcDataSourceService dataSourceService;
        private readonly Func<string, IOdbcColumnSource> columnSourceFactory;
        private IOdbcUploadMappingControlViewModel viewModel;
        private OdbcStoreEntity store;
        private readonly Func<IOdbcUploadMappingControlViewModel> viewModelFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportMappingPage"/> class.
        /// </summary>
        public OdbcUploadMappingPage(IOdbcDataSourceService dataSourceService,
            Func<IOdbcUploadMappingControlViewModel> viewModelFactory,
            Func<string, IOdbcColumnSource> columnSourceFactory)
        {
            this.dataSourceService = dataSourceService;
            this.viewModelFactory = viewModelFactory;
            this.columnSourceFactory = columnSourceFactory;
            InitializeComponent();
            SteppingInto += OnSteppingInto;
            StepNext += OnNext;
            StepBack += OnBack;
        }

        /// <summary>
        /// Gets the position.
        /// </summary>
        public int Position => 6;

        /// <summary>
        /// Called when [stepping into].
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            store = GetStore<OdbcStoreEntity>();

            if (store.UploadStrategy == (int)OdbcShipmentUploadStrategy.DoNotUpload ||
                store.UploadColumnSourceType == (int)OdbcColumnSourceType.CustomQuery)
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = false;
                return;
            }

            viewModel = viewModelFactory();
            viewModel.Load(store);

            mappingControl.DataContext = viewModel;
        }

        /// <summary>
        /// Save the map to the ODBC Store
        /// </summary>
        private void OnNext(object sender, WizardStepEventArgs e)
        {
            if (!viewModel.ValidateRequiredMappingFields())
            {
                e.NextPage = this;
                return;
            }

            viewModel.Save(store);
        }

        /// <summary>
        /// Save the map without validation. Validation will happen when they come back to this page and click next.
        /// </summary>
        private void OnBack(object sender, WizardStepEventArgs e)
        {
            viewModel.Save(store);
        }
    }
}
