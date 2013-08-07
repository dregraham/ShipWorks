using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Connection;
using Interapptive.Shared;
using System.Reflection;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Administration;
using System.Data.SqlClient;
using log4net;

namespace ShipWorks.ApplicationCore.Help
{
    /// <summary>
    /// The ShipWorks about window
    /// </summary>
    public partial class ShipWorksAboutDlg : Form
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksAboutDlg));

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksAboutDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // ShipWorks Version
            Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            version.Text = assemblyVersion.ToString(4);

            // Built
            built.Text = AssemblyDateAttribute.Read().ToLocalTime().ToLongDateString();

            if (SqlSession.IsConfigured)
            {
                labelDatabase.Text = SqlSession.Current.IsLocalDb() ? "Local only" : string.Format("'{0}' on {1}", SqlSession.Current.DatabaseName, SqlSession.Current.ServerInstance);
            }
            else
            {
                labelDatabase.Text = "(Not connected)";
            }

            Refresh();
            LoadUsage();
        }

        /// <summary>
        /// Open the browser to the support forum
        /// </summary>
        private void OnGetSupport(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("http://www.shipworks.com/shipworks/getsupport.php", this);
        }

        /// <summary>
        /// Open an email to support
        /// </summary>
        private void OnEmailSupport(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenMailTo("support@shipworks.com", this);
        }

        /// <summary>
        /// Format the credentials display
        /// </summary>
        private static string FormatCredentials(SqlSessionConfiguration sqlConfig)
        {
            if (sqlConfig.WindowsAuth)
            {
                return "Windows Authentication";
            }
            else
            {
                return sqlConfig.Username;
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
    }
}
