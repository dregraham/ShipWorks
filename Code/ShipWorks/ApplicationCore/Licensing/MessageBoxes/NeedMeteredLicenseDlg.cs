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
    /// Window that explains to the user that the old legacy non-metered licenses are not ok
    /// </summary>
    public partial class NeedMeteredLicenseDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NeedMeteredLicenseDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clicked the store link
        /// </summary>
        private void OnClickLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("https://www.interapptive.com/store", this);
        }
    }
}