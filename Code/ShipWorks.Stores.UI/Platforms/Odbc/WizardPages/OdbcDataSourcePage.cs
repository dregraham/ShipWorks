using System.Windows.Forms;
using log4net;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
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
        }

        /// <summary>
        ///     User is moving to the next wizard page, perform any autoconfiguration or credentials saving
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            OdbcStoreEntity store = GetStore<OdbcStoreEntity>();

            // Test the connection using the OdbcDataSourceControl
            // display error if the connection fails
            // set the connection string on the store entity if it passes
        }
    }
}