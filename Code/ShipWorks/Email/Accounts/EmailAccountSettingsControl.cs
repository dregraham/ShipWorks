using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using Rebex.Net;
using System.Threading;
using Interapptive.Shared.Security;
using ShipWorks.Common.Threading;
using log4net;
using Interapptive.Shared.UI;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// User control for editing detailed email account settings
    /// </summary>
    public partial class EmailAccountSettingsControl : UserControl
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(EmailAccountSettingsControl));

        EmailAccountEntity emailAccount;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailAccountSettingsControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<EmailIncomingServerType>(incomingServerType);
        }

        /// <summary>
        /// Load the settings from the given account into the UI
        /// </summary>
        public void LoadSettings(EmailAccountEntity emailAccount, bool allowChangeServerType)
        {
            if (emailAccount == null)
            {
                throw new ArgumentNullException("emailAccount");
            }

            this.emailAccount = emailAccount;

            alias.Text = emailAccount.AccountName;
            displayName.Text = emailAccount.DisplayName;
            emailAddress.Text = emailAccount.EmailAddress;

            incomingServer.Text = emailAccount.IncomingServer;
            incomingServerType.SelectedValue = (EmailIncomingServerType) emailAccount.IncomingServerType;
            incomingServerType.Enabled = allowChangeServerType;
            incomingUserName.Text = emailAccount.IncomingUsername;
            incomingPassword.Text = SecureText.Decrypt(emailAccount.IncomingPassword, emailAccount.IncomingUsername);

            smtpServer.Text = emailAccount.OutgoingServer;
        }

        /// <summary>
        /// Save the contents of the control to the entity passed in LoadSettings. If an error occurs, the error is displayed and false is returned.
        /// </summary>
        public bool SaveToEntity()
        {
            if (displayName.Text.Trim().Length == 0 ||
                emailAddress.Text.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Enter your name and email address.");
                return false;
            }

            if (alias.Text.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Enter an alias for the account.");
                return false;
            }

            if (incomingServer.Text.Trim().Length == 0 || smtpServer.Text.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Enter the incoming and outgoing mail servers.");
                return false;
            }

            if (incomingUserName.Text.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Enter a user name for your incoming mail server.");
                return false;
            }

            emailAccount.AccountName = alias.Text;
            emailAccount.DisplayName = displayName.Text;
            emailAccount.EmailAddress = emailAddress.Text.Trim();

            emailAccount.IncomingServer = incomingServer.Text.Trim();
            emailAccount.IncomingServerType = (int) incomingServerType.SelectedValue;
            emailAccount.IncomingUsername = incomingUserName.Text;
            emailAccount.IncomingPassword = SecureText.Encrypt(incomingPassword.Text, emailAccount.IncomingUsername);

            emailAccount.OutgoingServer = smtpServer.Text.Trim();

            return true;
        }

        /// <summary>
        /// Open the incoming settings window
        /// </summary>
        private void OnIncomingSettings(object sender, EventArgs e)
        {
            using (EmailAccountPopSettingsDlg dlg = new EmailAccountPopSettingsDlg(emailAccount))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Open the SMTP settings window
        /// </summary>
        private void OnSmtpSettings(object sender, EventArgs e)
        {
            using (EmailAccountSmtpSettingsDlg dlg = new EmailAccountSmtpSettingsDlg(emailAccount))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Send a test email message
        /// </summary>
        private void OnSendTest(object sender, EventArgs e)
        {
            if (!EmailUtility.IsValidEmailAddress(testEmailAddress.Text))
            {
                MessageHelper.ShowMessage(this, "Please specify a valid email address to send the test.");
                return;
            }

            if (!SaveToEntity())
            {
                return;
            }

            ProgressItem progress = new ProgressItem("Send Email");
            ProgressProvider progressProvider = new ProgressProvider();
            progressProvider.ProgressItems.Add(progress);

            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "Send Test Email";
            progressDlg.Description = "ShipWorks is sending a test email message.";
            progressDlg.AutoCloseWhenComplete = false;

            WaitCallback worker = new WaitCallback(OnSendTestWorker);
            worker.BeginInvoke(progress, new AsyncCallback(OnSendTestComplete), new object[] { worker, progressDlg } );

            progressDlg.Show(this);
        }

        /// <summary>
        /// Worker thread function for sending the test email
        /// </summary>
        private void OnSendTestWorker(object state)
        {
            ProgressItem progress = (ProgressItem) state;

            progress.Detail = "Log on to mail server...";
            progress.Starting();

            using (Smtp smtp = EmailUtility.LogonToSmtp(emailAccount))
            {
                if (!progress.IsCancelRequested)
                {
                    progress.CanCancel = false;
                    progress.Detail = "Sending test message...";

                    smtp.Send(
                        string.Format("ShipWorks <{0}>", emailAccount.EmailAddress),
                        testEmailAddress.Text.Trim(),
                        "ShipWorks Test Message",
                        "This is an email message sent automatically by ShipWorks while testing the settings for your account.");
                }

                smtp.Disconnect();
            }

            progress.Detail = "Email message sent.";
            progress.Completed();
        }

        /// <summary>
        /// Callback from completion of sending the test email
        /// </summary>
        private void OnSendTestComplete(IAsyncResult asyncResult)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AsyncCallback(OnSendTestComplete), asyncResult);
                return;
            }

            object[] state = (object[]) asyncResult.AsyncState;
            WaitCallback worker = (WaitCallback) state[0];
            ProgressDlg progressDlg = (ProgressDlg) state[1];

            try
            {
                worker.EndInvoke(asyncResult);

                if (progressDlg.ProgressProvider.CancelRequested)
                {
                    progressDlg.CloseForced();
                }
            }
            catch (EmailException ex)
            {
                log.Error("Error sending test email", ex);
                progressDlg.ProgressProvider.Terminate(ex);
            }
            catch (SmtpException ex)
            {
                log.Error("Error sending test email", ex);
                progressDlg.ProgressProvider.Terminate(ex);
            }
        }
    }
}
