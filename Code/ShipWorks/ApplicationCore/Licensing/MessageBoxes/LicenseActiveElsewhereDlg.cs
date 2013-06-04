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
    /// Window displayed when a license is activated by another store
    /// </summary>
    public partial class LicenseActiveElsewhereDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LicenseActiveElsewhereDlg()
        {
            InitializeComponent();
        }

        private void OnClickAccountLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("https://www.interapptive.com/account", this);
        }
    }
}