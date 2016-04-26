using System.Windows.Forms;
using log4net;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using System;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Odbc;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    /// <summary>
    /// Odbc Data Source wizard page
    /// </summary>
    public partial class OdbcDataSourcePage : AddStoreWizardPage
    {
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcDataSourcePage() :this(LogManager.GetLogger(typeof(OdbcDataSourcePage)))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private OdbcDataSourcePage(ILog log)
        {
            this.log = log;
            InitializeComponent();
            odbcDataSourceControl.RefreshDataSources();
        }

        /// <summary>
        ///     User is moving to the next wizard page, perform any autoconfiguration or credentials saving
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            OdbcStoreEntity store = GetStore<OdbcStoreEntity>();

            log.Debug("Testing Odbc data source.");

            // Test the connection using the OdbcDataSourceControl
            GenericResult<OdbcDataSource> result = odbcDataSourceControl.TestConnection();

            if (result.Success)
            {
                log.Error($"Odbc data source connected successfully.");

                GenericResult<StoreEntity> saveResult = odbcDataSourceControl.SaveToEntity(store);
                // Save the entity, if it fails stay on this page
                if (!saveResult.Success)
                {
                    MessageHelper.ShowError(this, saveResult.Message);
                    e.NextPage = this;
                }
            }
            else
            {
                // display error if the connection fails
                MessageHelper.ShowError(this, $"ShipWorks was unable to connect to the ODBC data source. {Environment.NewLine}{result.Message}");
                log.Error($"Odbc data source connection failed: {result.Message}");

                e.NextPage = this;
            }
        }
    }
}