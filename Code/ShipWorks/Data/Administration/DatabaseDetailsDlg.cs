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

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Window for showing the detailed of the current database connection
    /// </summary>
    public partial class DatabaseDetailsDlg : Form
    {
        static readonly ILog log = LogManager.GetLogger(typeof(DatabaseDetailsDlg));

        // Indicates if remote connections were enabled while the window was open
        bool remoteConnectionsEnabled = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseDetailsDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (SqlSession.IsConfigured)
            {
                SqlSessionConfiguration configuration = SqlSession.Current.Configuration;
                if (configuration.IsLocalDb())
                {
                    labelSqlInstance.Text = "Local Only";
                    labelDatabase.Text = "Default";
                    labelLoggedInAs.Text = "Default";
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
                labelDatabase.Text = "(Not connected)";
                labelSqlInstance.Text = "";
                labelLoggedInAs.Text = "";
                labelRemoteConnections.Text = "";
                linkEnableRemoteConnections.Visible = false;
            }

            Refresh();
            LoadUsage();
        }

        /// <summary>
        /// Indicates if remote connections were enabled while the window was open
        /// </summary>
        public bool RemoteConnectionsEnabled
        {
            get { return remoteConnectionsEnabled; }
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
        private void LoadUsage()
        {
            usageOrders.Text = "";
            usageResources.Text = "";
            usageAudit.Text = "";
            usageTotal.Text = "";
            usageRemaining.Text = "";
            infotipSizeRemaining.Visible = false;

            if (SqlSession.IsConfigured && SqlSession.Current.CanConnect())
            {
                try
                {
                    usageOrders.Text = StringUtility.FormatByteCount(SqlDiskUsage.OrdersUsage);
                    usageResources.Text = StringUtility.FormatByteCount(SqlDiskUsage.ResourceUsage);
                    usageAudit.Text = StringUtility.FormatByteCount(SqlDiskUsage.AuditUsage);
                    usageTotal.Text = StringUtility.FormatByteCount(SqlDiskUsage.TotalUsage);

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
            using (DatabaseSetupWizard dlg = new DatabaseSetupWizard(DatabaseSetupWizard.SetupMode.EnableRemoteConnections))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    remoteConnectionsEnabled = true;

                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
