using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// Control for selecting and connecting to a data source
    /// </summary>
    public partial class OdbcDataSourceControl : UserControl
    {
        private IOdbcDataSourceRepository repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDataSourceControl"/> class.
        /// </summary>
        public OdbcDataSourceControl()
        {
            InitializeComponent();
            dataSourceComboBox.DataSource = repo.GetDataSources();
            dataSourceComboBox.DisplayMember = "Name";
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
            store.ConnectionString = SelectedDataSource.ConnectionString;
            return true;
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
    }
}
