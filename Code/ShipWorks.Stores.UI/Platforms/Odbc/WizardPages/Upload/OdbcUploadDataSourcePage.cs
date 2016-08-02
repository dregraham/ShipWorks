using Interapptive.Shared.Threading;
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
        public int Position => 4;

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
                store.UploadConnectionString = odbcDataSourceControl.SelectedDataSource.Serialize();
            }
            else
            {
                e.NextPage = this;
            }
        }

        /// <summary>
        /// Called when [stepping into].
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            OdbcStoreEntity store = GetStore<OdbcStoreEntity>();

            if (store.UploadStrategy != (int)OdbcShipmentUploadStrategy.UseShipmentDataSource)
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = false;
            }
            else if (!string.IsNullOrWhiteSpace(store.ImportConnectionString))
            {
                odbcDataSourceControl.LoadDataSource(dataSourceService.GetImportDataSource(store));
            }
        }
    }
}
