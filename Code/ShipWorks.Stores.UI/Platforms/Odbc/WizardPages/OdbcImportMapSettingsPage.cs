using System;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.UI.Wizard;
using IWin32Window = System.Windows.Interop.IWin32Window;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    public partial class OdbcImportMapSettingsPage : AddStoreWizardPage, IOdbcWizardPage, IWin32Window
    {
        private readonly IMessageHelper messageHelper;
        private readonly Func<IOdbcDataSource> dataSourceFactory;
        private readonly Func<IOdbcImportFieldMappingControlViewModel> viewModelFactory;
        private IOdbcImportFieldMappingControlViewModel viewModel;
        private OdbcStoreEntity store;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingPage"/> class.
        /// </summary>
        public OdbcImportMapSettingsPage(IMessageHelper messageHelper,
            Func<IOdbcDataSource> dataSourceFactory,
            Func<IOdbcImportFieldMappingControlViewModel> viewModelFactory)
        {
            this.messageHelper = messageHelper;
            this.dataSourceFactory = dataSourceFactory;
            this.viewModelFactory = viewModelFactory;
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

            if (viewModel.SelectedTable == null)
            {
                messageHelper.ShowError("Please setup your import map before continuing to the next page.");
                e.NextPage = this;
                return;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void OnSteppingInto(object sender, EventArgs eventArgs)
        {
            store = GetStore<OdbcStoreEntity>();

            IOdbcDataSource selectedDataSource = dataSourceFactory();

            selectedDataSource.Restore(store.ConnectionString);

            // Create new ViewModel when one does not exist, or a new data source is selected. This means clicking
            // back on the mapping page and not changing the data source will keep any mappings made, but selecting
            // a new data source and clicking next, will reset all mappings.
            if (viewModel == null ||
                !viewModel.DataSource.ConnectionString.Equals(selectedDataSource.ConnectionString,
                    StringComparison.Ordinal))
            {
                viewModel = viewModelFactory();

                viewModel.Load(selectedDataSource);
                mapSettingsControl.DataContext = viewModel;
            }
        }
    }
}