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
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Window for editing detailed SMTP settings for an email account
    /// </summary>
    public partial class EmailAccountSmtpSettingsDlg : Form
    {
        EmailAccountEntity emailAccount;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailAccountSmtpSettingsDlg(EmailAccountEntity emailAccount)
        {
            InitializeComponent();

            this.emailAccount = emailAccount;

            SmtpSecurity security = (SmtpSecurity) emailAccount.OutgoingSecurityType;

            port.Text = emailAccount.OutgoingPort.ToString();
            secureRequired.Checked = (security != SmtpSecurity.Unsecure);
            securityMethod.SelectedIndex = (security != SmtpSecurity.Implicit) ? 0 : 1;

            EmailSmtpCredentialSource credentialSource = (EmailSmtpCredentialSource) emailAccount.OutgoingCredentialSource;

            // Cant be "POP before SMTP" if using IMAP
            if (emailAccount.IncomingSecurityType != (int) EmailIncomingServerType.Pop3)
            {
                if (credentialSource == EmailSmtpCredentialSource.PopBeforeSmtp)
                {
                    credentialSource = EmailSmtpCredentialSource.SameAsIncoming;
                }

                authenticatePopBeforeSmtp.Visible = false;
            }

            // The top checkbox is on if any credential source is chosen
            smtpAuthentication.Checked = credentialSource != EmailSmtpCredentialSource.None;

            // Select the appropriate radio option
            switch (credentialSource)
            {
                case EmailSmtpCredentialSource.None:
                case EmailSmtpCredentialSource.SameAsIncoming:
                    authenticateAsIncoming.Checked = true;
                    break;

                case EmailSmtpCredentialSource.Specify:
                    authenticateSpecifyLogon.Checked = true;
                    username.Text = emailAccount.OutgoingUsername;
                    password.Text = SecureText.Decrypt(emailAccount.OutgoingPassword, emailAccount.OutgoingUsername);
                    break;

                case EmailSmtpCredentialSource.PopBeforeSmtp:
                    authenticatePopBeforeSmtp.Checked = true;
                    break;

                default:
                    throw new InvalidOperationException("Unhandled credential source type. " + credentialSource);
            }

            UpdateAuthenticationUI();
        }

        /// <summary>
        /// Update the UI state for the authentication controls
        /// </summary>
        private void UpdateAuthenticationUI()
        {
            panelAuthentication.Enabled = smtpAuthentication.Checked;
            username.Enabled = authenticateSpecifyLogon.Checked;
            password.Enabled = authenticateSpecifyLogon.Checked;
        }

        /// <summary>
        /// A option has been changed that affects the authentication ui
        /// </summary>
        private void OnChangeAuthenticationType(object sender, EventArgs e)
        {
            UpdateAuthenticationUI();
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
        /// Save the settings to the email object passed to the constructor
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            int thePort;
            if (!int.TryParse(port.Text, out thePort) || thePort <= 0)
            {
                MessageHelper.ShowError(this, "Please enter a valid port number.");
                return;
            }

            EmailSmtpCredentialSource credentialSource;

            // Determine what the credential source for authentication is
            if (smtpAuthentication.Checked)
            {
                if (authenticateAsIncoming.Checked)
                {
                    credentialSource = EmailSmtpCredentialSource.SameAsIncoming;
                }
                else if (authenticateSpecifyLogon.Checked)
                {
                    credentialSource = EmailSmtpCredentialSource.Specify;
                }
                else
                {
                    credentialSource = EmailSmtpCredentialSource.PopBeforeSmtp;
                }
            }
            else
            {
                credentialSource = EmailSmtpCredentialSource.None;
            }

            // If its Specify, then save the user\pass
            if (credentialSource == EmailSmtpCredentialSource.Specify)
            {
                if (username.Text.Trim().Length == 0)
                {
                    MessageHelper.ShowError(this, "Please enter a user name.");
                    return;
                }

                emailAccount.OutgoingUsername = username.Text;
                emailAccount.OutgoingPassword = SecureText.Encrypt(password.Text, username.Text);
            }
            else
            {
                emailAccount.OutgoingUsername = "";
                emailAccount.OutgoingPassword = "";
            }

            emailAccount.OutgoingPort = thePort;

            if (secureRequired.Checked)
            {
                emailAccount.OutgoingSecurityType = (int) ((securityMethod.SelectedIndex == 0) ? SmtpSecurity.Explicit : SmtpSecurity.Implicit);
            }
            else
            {
                emailAccount.OutgoingSecurityType = (int) SmtpSecurity.Unsecure;
            }

            // Save the credential source
            emailAccount.OutgoingCredentialSource = (int) credentialSource;

            DialogResult = DialogResult.OK;
        }

    }
}
