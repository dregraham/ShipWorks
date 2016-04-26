using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;
using System.Windows.Forms;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    /// <summary>
    /// Odbc Data Source wizard page
    /// </summary>
    public partial class OdbcDataSourcePage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcDataSourcePage()
        {
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

            // Test the connection using the OdbcDataSourceControl
            bool testSuccessful = odbcDataSourceControl.TestConnection();

            if (testSuccessful)
            {
                odbcDataSourceControl.SaveToEntity(store);
            }
            else
            {
                e.NextPage = this;
            }
        }
    }
}