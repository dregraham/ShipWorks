using Autofac.Features.Indexed;
using Interapptive.Shared.UI;
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
    /// <summary>
    /// Wizard page for inputting upload map settings
    /// </summary>
    public partial class OdbcUploadMapSettingsPage : AddStoreWizardPage, IOdbcWizardPage
    {
        private readonly IMessageHelper messageHelper;
        private readonly IOdbcDataSourceFactory dataSourceFactory;
        private readonly IIndex<string, IOdbcMapSettingsControlViewModel> viewModelFactory;
        private readonly IOdbcSchema schema;
        private IOdbcMapSettingsControlViewModel viewModel;
        private OdbcStoreEntity store;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcUploadMapSettingsPage"/> class.
        /// </summary>
        public OdbcUploadMapSettingsPage(IMessageHelper messageHelper,
            IOdbcDataSourceFactory dataSourceFactory,
            IIndex<string, IOdbcMapSettingsControlViewModel> viewModelFactory,
            IOdbcSchema schema)
        {
            this.messageHelper = messageHelper;
            this.dataSourceFactory = dataSourceFactory;
            this.viewModelFactory = viewModelFactory;
            this.schema = schema;
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

            if (!viewModel.ValidateRequiredMapSettings())
            {
                e.NextPage = this;
                return;
            }

            viewModel.SaveMapSettings(store);
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

            IOdbcDataSource selectedDataSource = dataSourceFactory.CreateUploadDataSource(store);

            // Create new ViewModel when one does not exist, or a new data source is selected. This means clicking
            // back on the mapping page and not changing the data source will keep any mappings made, but selecting
            // a new data source and clicking next, will reset all mappings.
            if (viewModel == null ||
                !viewModel.DataSource.ConnectionString.Equals(selectedDataSource.ConnectionString,
                    StringComparison.Ordinal))
            {
                viewModel = viewModelFactory["Upload"];

                try
                {
                    schema.Load(selectedDataSource);
                }
                catch (ShipWorksOdbcException ex)
                {
                    messageHelper.ShowError(ex.Message);
                }

                viewModel.Load(selectedDataSource, schema.Tables);
                mapSettingsControl.DataContext = viewModel;
            }
        }
    }
}
