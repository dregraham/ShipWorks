using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;

namespace ShipWorks.ApplicationCore.Licensing.MessageBoxes
{
    /// <summary>
    /// Displayed when a license has been canceled
    /// </summary>
    public partial class LicenseCanceledDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LicenseCanceledDlg()
        {
            InitializeComponent();
        }

        private void OnClickAccountLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("https://www.interapptive.com/account", this);
        }
    }
}