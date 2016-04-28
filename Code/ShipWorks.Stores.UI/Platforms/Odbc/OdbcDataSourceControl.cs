using Autofac;
using Interapptive.Shared.Collections;
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
        private readonly Lazy<IExternalProcess> odbcControlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcDataSourceControl"/> class.
        /// </summary>
        public OdbcDataSourceControl()
        {
            InitializeComponent();
            log = LogManager.GetLogger(typeof(OdbcDataSourceControl));

            odbcControlPanel = new Lazy<IExternalProcess>(IoC.UnsafeGlobalLifetimeScope.Resolve<IExternalProcess>);
        }

        /// <summary>
        /// Gets the selected data source.
        /// </summary>
        private OdbcDataSource SelectedDataSource =>
            dataSource.SelectedItem as OdbcDataSource;

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

            GenericResult<OdbcDataSource> connectionResult = SelectedDataSource.TestConnection();

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
        public void SaveToEntity(OdbcStoreEntity store) =>
            store.ConnectionString = SelectedDataSource.ConnectionString;

        /// <summary>
        /// Sets the selected data source
        /// </summary>
        private void SelectedDataSourceChanged(object sender, EventArgs e)
        {
            username.Text = SelectedDataSource?.Username ?? string.Empty;
            password.Text = SelectedDataSource?.Password ?? string.Empty;
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

            GenericResult<List<OdbcDataSource>> dataSourceResult = GetDataSources();

            if (!dataSourceResult.Success)
            {
                dataSource.DataSource = null;
                MessageHelper.ShowError(ParentForm, dataSourceResult.Message);
            }
            else if (dataSourceResult.Context.None())
            {
                dataSource.DataSource = null;

                log.Warn("No datasources retrieved from IOdbcDataSourceRepository");
                MessageHelper.ShowWarning(ParentForm, "ShipWorks was unable to retrieve any DSN sources.");
            }
            else
            {
                BindDataSources(dataSourceResult.Context);
            }

        }

        /// <summary>
        /// Binds dataSources to the dataSource combobox.
        /// </summary>
        /// <remarks>
        /// If we can find the previously selected data source in the list of new data sources, 
        /// we select it and populate any previous username and password.
        /// </remarks>
        private void BindDataSources(List<OdbcDataSource> dataSources)
        {
            OdbcDataSource currentDataSource = SelectedDataSource;

            dataSource.DataSource = dataSources;
            dataSource.DisplayMember = "Name";

            if (currentDataSource != null)
            {
                OdbcDataSource matchedDataSource =
                    dataSources.FirstOrDefault(d => d.Name == currentDataSource.Name);
                if (matchedDataSource != null)
                {
                    // Previously selected datasource found. Select it and set the datasources username and password
                    dataSource.SelectedItem = matchedDataSource;
                    matchedDataSource.Username = username.Text;
                    matchedDataSource.Password = password.Text;
                }
                else
                {
                    // Previous datasource not found. Clear out username and password.
                    username.Text = string.Empty;
                    password.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets a list of data sources from an IOdbcDataSourceRepository.
        /// </summary>
        /// <returns>
        /// The context will be the list of datasources and success is true if no exceptions were thrown.
        /// </returns>
        private GenericResult<List<OdbcDataSource>> GetDataSources()
        {
            GenericResult<List<OdbcDataSource>> genericResult;

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                IOdbcDataSourceRepository repo = scope.Resolve<IOdbcDataSourceRepository>();

                try
                {
                    genericResult = new GenericResult<List<OdbcDataSource>>(repo.GetDataSources().ToList())
                    {
                        Success = true
                    };
                }
                catch (DataException ex)
                {
                    log.Error("Error thrown by repo.GetDataSources", ex);
                    genericResult = new GenericResult<List<OdbcDataSource>>(null)
                    {
                        Success = false,
                        Message = "ShipWorks encountered an error retrieving data sources. " +
                                  $"{Environment.NewLine}{Environment.NewLine}{ex.Message}"
                    };
                }
            }
            return genericResult;
        }


        /// <summary>
        /// Called when leaving the username
        /// </summary>
        private void OnLeaveUsername(object sender, EventArgs e) =>
            SelectedDataSource.Username = username.Text;

        /// <summary>
        /// Called when leaving password
        /// </summary>
        private void OnLeavePassword(object sender, EventArgs e) =>
            SelectedDataSource.Password = password.Text;

        /// <summary>
        /// Called when [click add data source].
        /// </summary>
        private void OnClickAddDataSource(object sender, EventArgs e) => 
            odbcControlPanel.Value.Launch(RefreshDataSources);
    }
}
