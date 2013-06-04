using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Rebex.Net;
using ShipWorks.UI;
using Interapptive.Shared.UI;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Window for editing detailed incoming server settings
    /// </summary>
    public partial class EmailAccountPopSettingsDlg : Form
    {
        EmailAccountEntity emailAccount;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailAccountPopSettingsDlg(EmailAccountEntity emailAccount)
        {
            InitializeComponent();

            this.emailAccount = emailAccount;

            EmailIncomingSecurityType security = (EmailIncomingSecurityType) emailAccount.IncomingSecurityType;

            port.Text = emailAccount.IncomingPort.ToString();
            secureRequired.Checked = (security != EmailIncomingSecurityType.Unsecure);
            securityMethod.SelectedIndex = (security != EmailIncomingSecurityType.Implicit) ? 0 : 1;
        }

        /// <summary>
        /// Changing the secure connection requirement
        /// </summary>
        private void OnChangeSecureConnection(object sender, EventArgs e)
        {
            labelMethod.Enabled = secureRequired.Checked;
            securityMethod.Enabled = secureRequired.Checked;
        }

        /// <summary>
        /// Save the changes to the account object that was loaded in the constructor
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            int thePort;
            if (!int.TryParse(port.Text, out thePort) || thePort <= 0)
            {
                MessageHelper.ShowError(this, "Please enter a valid port number.");
                return;
            }

            emailAccount.IncomingPort = thePort;

            if (secureRequired.Checked)
            {
                emailAccount.IncomingSecurityType = (int) (securityMethod.SelectedIndex == 0 ? EmailIncomingSecurityType.Explicit : EmailIncomingSecurityType.Implicit);
            }
            else
            {
                emailAccount.IncomingSecurityType = (int) EmailIncomingSecurityType.Unsecure;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
