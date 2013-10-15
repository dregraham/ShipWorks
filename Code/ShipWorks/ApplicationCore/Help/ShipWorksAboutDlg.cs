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
        }

        /// <summary>
        /// Open the browser to the support forum
        /// </summary>
        private void OnGetSupport(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("http://support.shipworks.com/", this);
        }

        /// <summary>
        /// Open an email to support
        /// </summary>
        private void OnEmailSupport(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenMailTo("support@shipworks.com", this);
        }
    }
}
