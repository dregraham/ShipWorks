using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data;

namespace ShipWorks.Stores.Platforms.PayPal
{
    /// <summary>
    /// Form to host the PayPalCredential user control for configuring
    /// PayPal Api credentials.
    /// </summary>
    public partial class PayPalCredentialDlg : Form
    {
        // the account being configured
        PayPalAccountAdapter payPalAccount;

        /// <summary>
        /// Constructor
        /// </summary>
        public PayPalCredentialDlg(PayPalAccountAdapter payPalAccount)
        {
            InitializeComponent();

            this.payPalAccount = payPalAccount;

            // load the settings
            payPalCredentials.LoadCredentials(payPalAccount);
        }

        /// <summary>
        /// Cancel clicked
        /// </summary>
        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        
        /// <summary>
        /// Accept the entered values 
        /// </summary>
        private void OnOkClick(object sender, EventArgs e)
        {
            if (payPalCredentials.SaveCredentials(payPalAccount))
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                // a message was shown, don't do anything
            }
        }
    }
}
