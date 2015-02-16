using System;
using System.Windows.Forms;

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
        public UspsAccountManagerDlg(UspsResellerType resellerType)
        {
            InitializeComponent();

            accountControl.StampsResellerType = resellerType;
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
