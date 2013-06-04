using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Window for allowing the user to agree to the UPS tracking agreement, per certification requirements.
    /// </summary>
    public partial class UpsTrackingAgreementDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsTrackingAgreementDlg()
        {
            InitializeComponent();
        }

        private void OnAgree(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
