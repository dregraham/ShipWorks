﻿using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.UI.Wizard;
using System.Windows.Forms;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Upload
{
    /// <summary>
    /// Wizard page to setup the OdbcUploadDataSource
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Management.AddStoreWizardPage" />
    /// <seealso cref="ShipWorks.Stores.Platforms.Odbc.IOdbcWizardPage" />
    public partial class OdbcUploadDataSourcePage : AddStoreWizardPage, IOdbcWizardPage
    {
        private readonly IOdbcDataSourceService dataSourceService;
        private OdbcStoreEntity store;
        private bool dataSourceLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcUploadDataSourcePage"/> class.
        /// </summary>
        /// <param name="odbcControlPanel">The ODBC control panel.</param>
        /// <param name="dataSourceService"></param>
        public OdbcUploadDataSourcePage(IExternalProcess odbcControlPanel, IOdbcDataSourceService dataSourceService)
        {
            this.dataSourceService = dataSourceService;
            InitializeComponent();
            odbcDataSourceControl.LoadDependencies(odbcControlPanel);
            odbcDataSourceControl.RefreshDataSources();
        }

        /// <summary>
        /// The position of the wizard page in the collection of pages
        /// </summary>
        public int Position => 6;

        /// <summary>
        /// Called when [stepping into].
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            store = GetStore<OdbcStoreEntity>();

            if (store.UploadStrategy != (int)OdbcShipmentUploadStrategy.UseShipmentDataSource && !string.IsNullOrEmpty(store.ImportConnectionString))
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = false;
            }
            else if (!string.IsNullOrWhiteSpace(store.UploadConnectionString) && !dataSourceLoaded)
            {
                odbcDataSourceControl.LoadDataSource(dataSourceService.GetUploadDataSource(store));
                dataSourceLoaded = true;
            }
        }

        /// <summary>
        /// User is moving to the next wizard page, perform any autoconfiguration or credentials saving
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            // Test the connection using the OdbcDataSourceControl
            bool testSuccessful = odbcDataSourceControl.TestConnection();

            if (testSuccessful)
            {
                store.UploadConnectionString = odbcDataSourceControl.SelectedDataSource.Serialize();
            }
            else
            {
                e.NextPage = this;
            }
        }
    }
}
