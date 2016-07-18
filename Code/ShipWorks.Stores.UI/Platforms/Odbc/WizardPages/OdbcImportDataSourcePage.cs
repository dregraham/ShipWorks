﻿using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.UI.Wizard;
using System.Windows.Forms;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    /// <summary>
    /// Odbc Data Source wizard page
    /// </summary>
    public partial class OdbcImportDataSourcePage : AddStoreWizardPage, IOdbcWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcImportDataSourcePage(IExternalProcess odbcControlPanel)
        {
            InitializeComponent();
            odbcDataSourceControl.LoadDependencies(odbcControlPanel);
            odbcDataSourceControl.RefreshDataSources();
        }

        /// <summary>
        /// The position in which to show this wizard page
        /// </summary>
        public int Position => 0;

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