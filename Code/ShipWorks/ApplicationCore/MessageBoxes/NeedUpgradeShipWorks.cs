using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;

namespace ShipWorks.ApplicationCore.MessageBoxes
{
    /// <summary>
    /// Window shown to a user when the ShipWorks database is newer than what this version supports
    /// </summary>
    public partial class NeedUpgradeShipWorks : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NeedUpgradeShipWorks()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Click the link to download the latest version of ShipWorks
        /// </summary>
        private void OnClickDownloadLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("http://www.shipworks.com/shipworks/downloadcustomer.php", this);
        }
    }
}
