using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Divelements.SandGrid;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;
using ShipWorks.UI;
using ShipWorks.Data.Connection;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Window for adding and removing stamps.com accounts
    /// </summary>
    public partial class StampsAccountManagerDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StampsAccountManagerDlg(bool isExpress1)
        {
            accountControl.IsExpress1 = isExpress1;

            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            accountControl.Initialize();
        }
    }
}
