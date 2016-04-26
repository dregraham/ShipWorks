using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using System.Data;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// Control for selecting and connecting to a data source
    /// </summary>
    public partial class OdbcDataSourceControl : UserControl
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDataSourceControl"/> class.
        /// </summary>
        public OdbcDataSourceControl()
        {
            InitializeComponent();
            RefreshDataSources();
        }

        /// <summary>
        /// Gets or sets the selected data source.
        /// </summary>
        private OdbcDataSource SelectedDataSource { get; set; }

        /// <summary>
        /// Tests the connection.
        /// </summary>
        public GenericResult<OdbcDataSource> TestConnection()
        {
            CheckForCredentials();

            return SelectedDataSource.TestConnection();
        }

        /// <summary>
        /// Saves the connection string to the OdbcStoreEntity
        /// </summary>
        public bool SaveToEntity(OdbcStoreEntity store)
        {
            if (SelectedDataSource == null)
            {
                MessageHelper.ShowError(this, "Please select a data source.");
                return false;
            }

            CheckForCredentials();

            store.ConnectionString = SelectedDataSource.ConnectionString;
            return true;
        }

        /// <summary>
        /// Checks if the username and password textboxes have values and saves them to the data source.
        /// </summary>
        private void CheckForCredentials()
        {
            if (!string.IsNullOrWhiteSpace(usernameTextBox.Text))
            {
                SelectedDataSource.Username = usernameTextBox.Text;
            }

            if (!string.IsNullOrWhiteSpace(passwordTextBox.Text))
            {
                SelectedDataSource.Password = passwordTextBox.Text;
            }
        }

        /// <summary>
        /// Sets the selected data source
        /// </summary>
        private void SelectedDataSourceChanged(object sender, System.EventArgs e)
        {
            if (dataSourceComboBox.SelectedItem != null)
            {
                SelectedDataSource = (OdbcDataSource) dataSourceComboBox.SelectedItem;
            }
        }

        /// <summary>
        /// Load the existing data sources
        /// </summary>
        private void RefreshDataSources()
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                IOdbcDataSourceRepository repo = scope.Resolve<IOdbcDataSourceRepository>();

                try
                {
                    dataSourceComboBox.DataSource = repo.GetDataSources();
                    dataSourceComboBox.DisplayMember = "Name";
                }
                catch (DataException)
                {
                    // getting data sources failed.
                }
            }
        }
    }
}
