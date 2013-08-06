using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using log4net;

namespace ShipWorks.ApplicationCore.MessageBoxes
{
    /// <summary>
    /// Welcome window for first time install
    /// </summary>
    public partial class WelcomeDlg : Form
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(WelcomeDlg));

        /// <summary>
        /// Constructor
        /// </summary>
        public WelcomeDlg()
        {
            log.InfoFormat("Creating welcome.");

            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            log.InfoFormat("Loading welcome.");
        }

        /// <summary>
        /// Link to open the support forum
        /// </summary>
        private void OnLinkSupportForum(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.shipworks.com/support", this);
        }
    }
}