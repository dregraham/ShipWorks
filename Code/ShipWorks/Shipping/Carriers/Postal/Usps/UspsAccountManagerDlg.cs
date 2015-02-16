using System;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Stamps;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Window for adding and removing stamps.com accounts
    /// </summary>
    public partial class UspsAccountManagerDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UspsAccountManagerDlg(StampsResellerType stampsResellerType)
        {
            InitializeComponent();

            accountControl.StampsResellerType = stampsResellerType;
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
