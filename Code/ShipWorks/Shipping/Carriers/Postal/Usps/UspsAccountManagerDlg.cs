using System;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Window for adding and removing USPS accounts
    /// </summary>
    public partial class UspsAccountManagerDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UspsAccountManagerDlg(UspsResellerType resellerType)
        {
            InitializeComponent();

            accountControl.UspsResellerType = resellerType;
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
