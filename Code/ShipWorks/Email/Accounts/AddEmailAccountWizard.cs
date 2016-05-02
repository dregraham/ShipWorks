using System;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.Net;
using ShipWorks.Common.Threading;
using Interapptive.Shared.Security;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using Interapptive.Shared.UI;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Wizard for adding a new email account to ShipWorks
    /// </summary>
    public partial class AddEmailAccountWizard : WizardForm
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(AddEmailAccountWizard));

        // Result from the last settings search
        EmailSettingsSearchResult settingsSearchResult;

        // The email account settings pending creation
        EmailAccountEntity emailAccount;

        // Force the incoming server type
        EmailIncomingServerType? forceIncomingServerType;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddEmailAccountWizard(EmailIncomingServerType? forceIncomingServerType)
        {
            InitializeComponent();

            this.forceIncomingServerType = forceIncomingServerType;
            this.emailAccount = EmailUtility.CreateNewAccount(forceIncomingServerType ?? EmailIncomingServerType.Pop3);
        }

        /// <summary>
        /// The EmailAccount that was creatd.  Only valid if the DialogResult is OK.
        /// </summary>
        public EmailAccountEntity EmailAccount
        {
            get
            {
                if (DialogResult != DialogResult.OK)
                {
                    return null;
                }

                return emailAccount;
            }
        }

        /// <summary>
        /// Changing if using automatic or manual configuration
        /// </summary>
        private void OnChangeManualConfiguration(object sender, EventArgs e)
        {
            panelAutoConfig.Enabled = radioAutomaticConfig.Checked;
        }

        /// <summary>
        /// Stepping next from the basic info page
        /// </summary>
        private void OnBasicInfoStepNext(object sender, WizardStepEventArgs e)
        {
            // Automatic configuration
            if (radioAutomaticConfig.Checked)
            {
                if (displayName.Text.Trim().Length == 0)
                {
                    MessageHelper.ShowError(this, "Enter your name.");
                    e.NextPage = CurrentPage;
                    return;
                }

                if (emailAddress.Text.Trim().Length == 0)
                {
                    MessageHelper.ShowError(this, "Enter your email address.");
                    e.NextPage = CurrentPage;
                    return;
                }

                if (!EmailUtility.IsValidEmailAddress(emailAddress.Text))
                {
                    MessageHelper.ShowMessage(this, "Please enter a valid email address.");
                    e.NextPage = CurrentPage;
                    return;
                }

                if (password.Text.Trim().Length == 0)
                {
                    MessageHelper.ShowError(this, "Enter the password for your email account.\n\nIf you don't have one, chose Manual Configuration.");
                    e.NextPage = CurrentPage;
                    return;
                }

                emailAccount.DisplayName = displayName.Text.Trim();
                emailAccount.EmailAddress = emailAddress.Text.Trim();
                emailAccount.AccountName = emailAccount.EmailAddress;

                // If username is not yet set, use before the @
                if (string.IsNullOrEmpty(emailAccount.IncomingUsername))
                {
                    emailAccount.IncomingUsername = emailAccount.EmailAddress.Substring(0, emailAccount.EmailAddress.IndexOf('@'));
                }

                emailAccount.IncomingPassword = SecureText.Encrypt(password.Text, emailAccount.IncomingUsername);
            }
            // Manual configuration
            else
            {
                settingsSearchResult = EmailSettingsSearchResult.FailedResult;

                e.NextPage = wizardPageManualConfigure;
            }
        }

        /// <summary>
        /// Stepping into the initial basic info page
        /// </summary>
        private void OnBasicInfoSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            // When stepping back in, clear the last result, so stepping next triggers another search
            if (e.StepReason == WizardStepReason.StepBack)
            {
                settingsSearchResult = null;
            }
        }

        /// <summary>
        /// Stepping into the page for enabling accoung access (GMail)
        /// </summary>
        private void OnEnableAccessSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            bool gmailPop = emailAddress.Text.Contains("gmail.com") && forceIncomingServerType == EmailIncomingServerType.Pop3;

            e.Skip = !gmailPop;
            e.RaiseStepEventWhenSkipping = true;

            // When stepping back in, clear the last result, so stepping next triggers another search
            if (e.StepReason == WizardStepReason.StepBack)
            {
                settingsSearchResult = null;
            }
        }

        /// <summary>
        /// Open the help link for enabling POP for GMail
        /// </summary>
        private void OnClickGMailPopHelp(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://mail.google.com/support/bin/answer.py?answer=13273", this);
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnEnableAccessStepNext(object sender, WizardStepEventArgs e)
        {
            // Will be non-null after a succesull search.  And gets reset to null on re-entering the page later from a Back
            if (settingsSearchResult != null)
            {
                return;
            }

            // Stay on this wizard page until the search completes
            e.NextPage = CurrentPage;

            EmailSettingsSearcher searcher = new EmailSettingsSearcher(forceIncomingServerType);
            searcher.SearchCompleted += new EmailSettingsSearchCompletedEventHandler(OnEmailSettingsSearchCompleted);

            ProgressDlg progressDlg = new ProgressDlg(searcher.ProgressProvider);
            progressDlg.Title = "Configuring Your Email";
            progressDlg.Description = "Searching for the settings to use for your account.";
            progressDlg.AutoCloseWhenComplete = false;
            progressDlg.Show(this);

            // Begin searching
            searcher.SearchAsync(emailAddress.Text.Trim(), password.Text, progressDlg);
        }

        void OnEmailSettingsSearchCompleted(object sender, EmailSettingsSearchCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EmailSettingsSearchCompletedEventHandler(OnEmailSettingsSearchCompleted), sender, e);
                return;
            }

            // Ensure the progress window has closed
            ProgressDlg dlg = (ProgressDlg) e.UserState;
            dlg.CloseForced();

            EmailSettingsSearcher searcher = (EmailSettingsSearcher) sender;
            searcher.SearchCompleted -= this.OnEmailSettingsSearchCompleted;

            // Check for errors
            if (e.Error != null)
            {
                // Rethrow the error as is
                log.Error("Error while saving", e.Error);
                throw new ApplicationException(e.Error.Message, e.Error);
            }
            else if (!e.Cancelled)
            {
                settingsSearchResult = e.SearchResult;

                // If successful, use the found settings
                if (settingsSearchResult.Success)
                {
                    // Copy POP server settings
                    emailAccount.IncomingServer = e.SearchResult.IncomingHost;
                    emailAccount.IncomingServerType = (int) e.SearchResult.IncomingHostType;
                    emailAccount.IncomingPort = e.SearchResult.IncomingPort;
                    emailAccount.IncomingSecurityType = (int) e.SearchResult.IncomingSecurity;
                    emailAccount.IncomingUsername = e.SearchResult.IncomingUsername;
                    emailAccount.IncomingPassword = SecureText.Encrypt(e.SearchResult.IncomingPassword, e.SearchResult.IncomingUsername);

                    // Copy SMTP server settings
                    emailAccount.OutgoingServer = e.SearchResult.SmtpHost;
                    emailAccount.OutgoingPort = e.SearchResult.SmtpPort;
                    emailAccount.OutgoingSecurityType = (int) e.SearchResult.SmtpSecurity;

                    // Determine SMTP credentials
                    if (e.SearchResult.IncomingUsername == e.SearchResult.SmtpUsername &&
                        e.SearchResult.IncomingPassword == e.SearchResult.SmtpPassword)
                    {
                        emailAccount.OutgoingCredentialSource = (int) EmailSmtpCredentialSource.SameAsIncoming;
                    }
                    else
                    {
                        emailAccount.OutgoingCredentialSource = (int) EmailSmtpCredentialSource.Specify;
                        emailAccount.OutgoingUsername = e.SearchResult.SmtpUsername;
                        emailAccount.OutgoingPassword = SecureText.Encrypt(e.SearchResult.SmtpPassword, e.SearchResult.SmtpUsername);
                    }
                }

                MoveNext();
            }
        }

        /// <summary>
        /// Stepping into the search failed page
        /// </summary>
        private void OnSearchFailedSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (settingsSearchResult == null)
            {
                throw new InvalidOperationException("Should not be able to get to this page with a null search result.");
            }

            e.Skip = settingsSearchResult.Success || e.StepReason == WizardStepReason.StepBack;
        }

        /// <summary>
        /// Stepping into the page used to configure manual settings
        /// </summary>
        private void OnManualSettingsSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (settingsSearchResult == null)
            {
                throw new InvalidOperationException("Should not be able to get to this page with a null search result.");
            }

            if (settingsSearchResult.Success)
            {
                e.Skip = true;
                return;
            }

            // Load the settings
            emailAccountSettings.LoadSettings(emailAccount, (forceIncomingServerType == null) );
        }

        /// <summary>
        /// Stepping next from the manual settings page
        /// </summary>
        private void OnManualSettingsStepNext(object sender, WizardStepEventArgs e)
        {
            // Save the results to the entity.
            if (!emailAccountSettings.SaveToEntity())
            {
                e.NextPage = CurrentPage;
                return;
            }
            else
            {
                e.NextPage = wizardPageSuccess;
            }

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(emailAccount);
            }
        }

        /// <summary>
        /// Stepping into the account alias page
        /// </summary>
        private void OnAccountAliasSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            alias.Text = emailAccount.AccountName;
        }

        /// <summary>
        /// Stepping next from the alias page
        /// </summary>
        private void OnAccountAliasStepNext(object sender, WizardStepEventArgs e)
        {
            if (alias.Text.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "You must enter an alias for the account.");
                e.NextPage = CurrentPage;

                return;
            }

            emailAccount.AccountName = alias.Text.Trim();

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(emailAccount);
            }
        }
    }
}
