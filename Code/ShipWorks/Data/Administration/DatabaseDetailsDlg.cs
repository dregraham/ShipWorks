﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Utility;
using System.Data.SqlClient;
using log4net;
using Autofac;
using Autofac.Core.Lifetime;
using Interapptive.Shared.Data;
using System.Threading.Tasks;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Window for showing the detailed of the current database connection
    /// </summary>
    public partial class DatabaseDetailsDlg : Form
    {
        static readonly ILog log = LogManager.GetLogger(typeof(DatabaseDetailsDlg));

        // Indicates if remote connections were enabled while the window was open
        bool databaseConfigurationChanged = false;
        private ILifetimeScope lifetimeScope;

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseDetailsDlg(ILifetimeScope lifetimeScope)
        {
            InitializeComponent();

            this.lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private async void OnLoad(object sender, EventArgs e)
        {
            if (SqlSession.IsConfigured)
            {
                SqlSessionConfiguration configuration = SqlSession.Current.Configuration;
                if (configuration.IsLocalDb())
                {
                    labelSqlInstance.Text = "Local Only";
                    labelDatabase.Text = "Default";
                    labelLoggedInAs.Text = "Default";

                    linkEnableRemoteConnections.Visible = !lifetimeScope.Resolve<IConfigurationData>().IsArchive();
                }
                else
                {
                    labelSqlInstance.Text = configuration.ServerInstance;
                    labelDatabase.Text = configuration.DatabaseName;
                    labelLoggedInAs.Text = FormatCredentials(configuration);
                    labelRemoteConnections.Text = "Supported";
                    linkEnableRemoteConnections.Visible = false;
                }
            }
            else
            {
                labelSqlInstance.Text = "(Not connected)";
                labelDatabase.Text = "";
                labelLoggedInAs.Text = "";
                labelRemoteConnections.Text = "";
                linkEnableRemoteConnections.Visible = false;
            }

            Refresh();
            await LoadUsage();
        }

        /// <summary>
        /// Indicates if the database configuration has changed while open
        /// </summary>
        public bool DatabaseConfigurationChanged
        {
            get { return databaseConfigurationChanged; }
        }

        /// <summary>
        /// Format the credentials display
        /// </summary>
        private static string FormatCredentials(SqlSessionConfiguration configuration)
        {
            if (configuration.WindowsAuth)
            {
                return "Windows Authentication";
            }
            else
            {
                return configuration.Username;
            }
        }

        /// <summary>
        /// Load database usage information
        /// </summary>
        private async Task LoadUsage()
        {
            usageOrders.Text = "...";
            usageEmail.Text = "...";
            usageAudit.Text = "...";
            usagePrintJob.Text = "...";
            usageLabel.Text = "...";
            usageTotal.Text = "...";
            usageRemaining.Text = "...";
            usageOther.Text = "...";
            usageShipSense.Text = "...";
            infotipSizeRemaining.Visible = false;

            if (SqlSession.IsConfigured && SqlSession.Current.CanConnect())
            {
                try
                {
                    usageAudit.Text = StringUtility.FormatByteCount(await SqlDiskUsage.GetAuditUsage());
                    usageDownloadDetails.Text = StringUtility.FormatByteCount(await SqlDiskUsage.GetDownloadUsage());
                    usageEmail.Text = StringUtility.FormatByteCount(await SqlDiskUsage.GetResourceEmailData());
                    usageOrders.Text = StringUtility.FormatByteCount(await SqlDiskUsage.GetOrdersUsage());
                    usagePrintJob.Text = StringUtility.FormatByteCount(await SqlDiskUsage.GetResourcePrintResultData());
                    usageLabel.Text = StringUtility.FormatByteCount(await SqlDiskUsage.GetResourceLabelData());
                    usageShipSense.Text = StringUtility.FormatByteCount(await SqlDiskUsage.GetShipSenseUsage());
                    usageOther.Text = StringUtility.FormatByteCount(await SqlDiskUsage.GetOtherUsage());
                    usageTotal.Text = StringUtility.FormatByteCount(SqlDiskUsage.GetDatabaseSpaceUsed());
                    
                    long remaining = SqlDiskUsage.SpaceRemaining;
                    if (remaining == -1)
                    {
                        usageRemaining.Text = "Unlimited";
                        infotipSizeRemaining.Caption = "Your version of SQL Server does not have a database size limit.";
                    }
                    else
                    {
                        usageRemaining.Text = StringUtility.FormatByteCount(remaining);
                        infotipSizeRemaining.Caption = string.Format(infotipSizeRemaining.Caption, SqlDiskUsage.SizeLimitGB);
                    }

                    infotipSizeRemaining.Visible = true;
                    infotipSizeRemaining.Left = usageRemaining.Right + 2;

                }
                catch (SqlException ex)
                {
                    log.Error("Failed to load usage information.", ex);
                }
            }
        }

        /// <summary>
        /// Enable remote connections
        /// </summary>
        private void OnEnableRemoteConnections(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (DetailedDatabaseSetupWizard dlg = new DetailedDatabaseSetupWizard(DetailedDatabaseSetupWizard.SetupMode.EnableRemoteConnections, lifetimeScope))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    databaseConfigurationChanged = true;

                    DialogResult = DialogResult.OK;
                }
            }
        }

        /// <summary>
        /// Configure the database
        /// </summary>
        private void OnConfigureDatabase(object sender, EventArgs e)
        {
            using (DetailedDatabaseSetupWizard dlg = new DetailedDatabaseSetupWizard(lifetimeScope))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    databaseConfigurationChanged = true;

                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
