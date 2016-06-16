using Autofac;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// Control for selecting and connecting to a data source
    /// </summary>
    public partial class OdbcDataSourceControl : UserControl
    {
        private readonly ILog log;
        private IExternalProcess odbcControlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDataSourceControl"/> class.
        /// </summary>
        public OdbcDataSourceControl()
        {
            InitializeComponent();
            log = LogManager.GetLogger(typeof(OdbcDataSourceControl));
        }

        /// <summary>
        /// Gets the selected data source.
        /// </summary>
        private EncryptedOdbcDataSource SelectedDataSource => dataSource.SelectedItem as EncryptedOdbcDataSource;

        /// <summary>
        /// Loads the dependencies.
        /// </summary>
        public void LoadDependencies(IExternalProcess odbcControlPanelProcess)
        {
            odbcControlPanel = odbcControlPanelProcess;
        }

        /// <summary>
        /// Tests the connection.
        /// </summary>
        public bool TestConnection()
        {
            log.Debug("Testing data source");

            if (SelectedDataSource == null)
            {
                MessageHelper.ShowWarning(ParentForm, $"ShipWorks could not find any ODBC data sources. {Environment.NewLine} " +
                                                "Please add one to continue.");
                return false;
            }

            GenericResult<IOdbcDataSource> connectionResult = SelectedDataSource.TestConnection();

            if (connectionResult.Success)
            {
                log.Error("Odbc data source connected successfully.");
            }
            else
            {
                log.Error($"Odbc data source connection failed: {connectionResult.Message}");
                MessageHelper.ShowError(ParentForm, "ShipWorks was unable to connect to the ODBC data source. " +
                                              $"{Environment.NewLine}{Environment.NewLine}{connectionResult.Message}");
            }

            return connectionResult.Success;
        }

        /// <summary>
        /// Saves the connection string to the OdbcStoreEntity
        /// </summary>
        public void SaveToEntity(OdbcStoreEntity store)
        {
            if (SelectedDataSource == null)
            {
                MessageHelper.ShowError(this, "You must add a datasource to continue.");
                return;
            }

            try
            {
                store.ConnectionString = SelectedDataSource.Serialize();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageHelper.ShowError(this, "An error occurred while trying to save the data source.");
            }
        }


        /// <summary>
        /// Sets the selected data source
        /// </summary>
        private void SelectedDataSourceChanged(object sender, EventArgs e)
        {
            UpdatePanelVisibility();

            username.Text = SelectedDataSource?.Username ?? string.Empty;
            password.Text = SelectedDataSource?.Password ?? string.Empty;
            customConnectionString.Text = SelectedDataSource?.ConnectionString ?? string.Empty;
        }

        /// <summary>
        /// Updates which panels are visible based on the selected data source
        /// </summary>
        private void UpdatePanelVisibility()
        {
            if (SelectedDataSource.IsCustom)
            {
                customPanel.Show();
                credentialsPanel.Hide();
            }
            else
            {
                customPanel.Hide();
                credentialsPanel.Show();
            }
        }

        /// <summary>
        /// Load the existing data sources
        /// </summary>
        public void RefreshDataSources()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(RefreshDataSources));
                return;
            }

            GenericResult<List<IOdbcDataSource>> dataSourceResult = GetDataSources();

            if (!dataSourceResult.Success)
            {
                dataSource.DataSource = null;
                MessageHelper.ShowError(ParentForm, dataSourceResult.Message);
            }
            else if (dataSourceResult.Value.None())
            {
                dataSource.DataSource = null;

                log.Warn("No data sources found in IOdbcDataSourceRepository");
                MessageHelper.ShowWarning(ParentForm, $"ShipWorks could not find any ODBC data sources. {Environment.NewLine} " +
                                                "Please add one to continue.");
            }
            else
            {
                BindDataSources(dataSourceResult.Value);
            }
        }

        /// <summary>
        /// Binds dataSources to the dataSource combobox.
        /// </summary>
        /// <remarks>
        /// If we can find the previously selected data source in the list of new data sources,
        /// we select it and populate any previous username and password.
        /// </remarks>
        private void BindDataSources(List<IOdbcDataSource> dataSources)
        {
            EncryptedOdbcDataSource currentDataSource = SelectedDataSource;

            dataSource.DataSource = dataSources;
            dataSource.DisplayMember = "Name";

            if (currentDataSource != null)
            {
                IOdbcDataSource matchedDataSource = dataSources.FirstOrDefault(d => d.Name == currentDataSource.Name);
                if (matchedDataSource != null)
                {
                    // Previously selected datasource found.
                    dataSource.SelectedItem = matchedDataSource;
                }
                else
                {
                    // Previous datasource not found. Clear out username and password.
                    username.Text = string.Empty;
                    password.Text = string.Empty;
                    customConnectionString.Text = string.Empty;
                }
            }

            UpdatePanelVisibility();
        }

        /// <summary>
        /// Gets a list of data sources from an IOdbcDataSourceRepository.
        /// </summary>
        /// <returns>
        /// The context will be the list of datasources and success is true if no exceptions were thrown.
        /// </returns>
        private GenericResult<List<IOdbcDataSource>> GetDataSources()
        {
            GenericResult<List<IOdbcDataSource>> genericResult;

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                try
                {
                    IOdbcDataSourceRepository repo = scope.Resolve<IOdbcDataSourceRepository>();
                    genericResult = GenericResult.FromSuccess(repo.GetDataSources().ToList());
                }
                catch (DataException ex)
                {
                    log.Error("Error thrown by repo.GetDataSources", ex);
                    genericResult =
                        GenericResult.FromError<List<IOdbcDataSource>>(
                            "ShipWorks encountered an error finding data sources. " +
                            $"{Environment.NewLine}{Environment.NewLine}{ex.Message}");
                }
            }
            return genericResult;
        }

        /// <summary>
        /// Called when leaving the username
        /// </summary>
        private void OnChangedUsername(object sender, EventArgs e) =>
            SelectedDataSource.ChangeConnection(SelectedDataSource.Name, username.Text, SelectedDataSource.Password);

        /// <summary>
        /// Called when leaving password
        /// </summary>
        private void OnChangedPassword(object sender, EventArgs e) =>
            SelectedDataSource.ChangeConnection(SelectedDataSource.Name, SelectedDataSource.Username, password.Text);

        /// <summary>
        /// Called when leaving customConnectionString
        /// </summary>
        private void OnChangedCustomConnectionString(object sender, EventArgs e) =>
            SelectedDataSource.ChangeConnection(customConnectionString.Text);

        /// <summary>
        /// Called when [click add data source].
        /// </summary>
        private void OnClickAddDataSource(object sender, EventArgs e)
        {
            try
            {
                odbcControlPanel.Launch(RefreshDataSources);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageHelper.ShowError(this, "An error occurred while trying to open ODBC control panel.");
            }
        }

        /// <summary>
        /// Called when [click test data source]
        /// </summary>
        private void OnTestConnection(object sender, EventArgs e)
        {
            if (TestConnection())
            {
                MessageHelper.ShowMessage(this, "Connection to the ODBC data source was successful!");
            }
        }
    }
}
