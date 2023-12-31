﻿using System.Windows.Forms;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Import
{
    /// <summary>
    /// Odbc Data Source wizard page
    /// </summary>
    public partial class OdbcImportDataSourcePage : AddStoreWizardPage, IOdbcWizardPage
    {
        private readonly IOdbcDataSourceService dataSourceService;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcImportDataSourcePage(IExternalProcess odbcControlPanel, IOdbcDataSourceService dataSourceService)
        {
            this.dataSourceService = dataSourceService;
            InitializeComponent();
            odbcDataSourceControl.LoadDependencies(odbcControlPanel);
            odbcDataSourceControl.RefreshDataSources();
        }

        /// <summary>
        /// The position in which to show this wizard page
        /// </summary>
        public int Position => 0;

        /// <summary>
        /// Load data source from store and pass into control
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            OdbcStoreEntity store = GetStore<OdbcStoreEntity>();

            if (!store.IsMappingRequired && store.ImportStrategy != (int) OdbcImportStrategy.OnDemand)
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = false;
                return;
            }

            if (!string.IsNullOrWhiteSpace(store.ImportConnectionString))
            {
                odbcDataSourceControl.LoadDataSource(dataSourceService.GetImportDataSource(store));
            }
        }

        /// <summary>
        /// User is moving to the next wizard page, perform any autoconfiguration or credentials saving
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            OdbcStoreEntity store = GetStore<OdbcStoreEntity>();

            // Test the connection using the OdbcDataSourceControl
            bool testSuccessful = odbcDataSourceControl.TestConnection();

            if (testSuccessful)
            {
                store.ImportConnectionString = odbcDataSourceControl.SelectedDataSource.Serialize();
            }
            else
            {
                e.NextPage = this;
            }
        }
    }
}
