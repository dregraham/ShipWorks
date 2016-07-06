using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.UI.Wizard;
using System;
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
        private readonly IMessageHelper messageHelper;
        private readonly Func<IOdbcDataSource> dataSourceFactory;
        private readonly Func<Type, ILog> logFactory;
        private readonly Func<string, IOdbcColumnSource> columnSourceFactory;
        private readonly Func<IOdbcImportFieldMappingControlViewModel> viewModelFactory;
        private IOdbcImportFieldMappingControlViewModel viewModel;
        private OdbcStoreEntity store;
        private const string CustomQueryColumnSourceName = "Custom Import";


        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingPage"/> class.
        /// </summary>
        public OdbcImportFieldMappingPage(IMessageHelper messageHelper,
            Func<IOdbcDataSource> dataSourceFactory,
            Func<IOdbcImportFieldMappingControlViewModel> viewModelFactory,
            Func<Type, ILog> logFactory,
            Func<string, IOdbcColumnSource> columnSourceFactory)
        {
            this.messageHelper = messageHelper;
            this.dataSourceFactory = dataSourceFactory;
            this.logFactory = logFactory;
            this.columnSourceFactory = columnSourceFactory;
            viewModel = viewModelFactory();
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

            IOdbcDataSource selectedDataSource = dataSourceFactory();

            selectedDataSource.Restore(store.ConnectionString);

            string columnSourceName = store.OdbcColumnSourceType == (int) OdbcColumnSourceType.Table
                ? store.OdbcColumnSource
                : "Custom";

            IOdbcColumnSource columnSource = columnSourceFactory(columnSourceName);
            columnSource.Load(selectedDataSource, store.OdbcColumnSource,(OdbcColumnSourceType) store.OdbcColumnSourceType);

            mappingControl.DataContext = viewModel;
            viewModel.LoadColumnSource(columnSource, (OdbcDownloadStrategy) store.OdbcDownloadStrategy);
        }
    }
}
