using System;
using System.DirectoryServices.AccountManagement;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.ApplicationCore.Services.Hosting.Windows
{
    /// <summary>
    /// Prompts user for username and password.
    /// </summary>
    public partial class WindowsServiceCredentialsDlg : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsServiceCredentialsDlg"/> class.
        /// </summary>
        public WindowsServiceCredentialsDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Indicates if a windows user account should be used (vs. Local System)
        /// </summary>
        public bool UseWindowsUserAccount
        {
            get
            {
                return radioWindowsUser.Checked;
            }
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        public string UserName
        {
            get
            {
                return radioLocalSystem.Checked ? "" : username.Text.Trim();
            }
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password
        {
            get
            {
                return radioLocalSystem.Checked ? "" : password.Text.Trim();
            }
        }

        /// <summary>
        /// Gets the domain, or null if not entered
        /// </summary>
        public string Domain
        {
            get
            {
                if (radioLocalSystem.Checked)
                {
                    return "";
                }

                string trimmedDomain = domain.Text.Trim();

                return string.IsNullOrEmpty(trimmedDomain) ? null : trimmedDomain;
            }
        }

        /// <summary>
        /// Changing the radio selection on what account to use
        /// </summary>
        private void OnAccountChanged(object sender, EventArgs e)
        {
            username.Enabled = radioWindowsUser.Checked;
            password.Enabled = radioWindowsUser.Checked;
            domain.Enabled = radioWindowsUser.Checked;
        }

        /// <summary>
        /// Called when [OK] button is clicked.
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (ValidateForm() && ValidateCredentials())
            {
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Validates the credentials.
        /// </summary>
        private bool ValidateCredentials()
        {
            if (radioLocalSystem.Checked)
            {
                return true;
            }

            Cursor.Current = Cursors.WaitCursor;

            bool isValid = false;

            // Settings for Active Directory
            ContextType contextType = ContextType.Domain;

            // Settings for local PC
            if (string.IsNullOrEmpty(Domain))
            {
                contextType = ContextType.Machine;
            }

            try
            {
                // create a "principal context" - e.g. your domain (could be machine, too)
                using (PrincipalContext pc = new PrincipalContext(contextType, Domain))
                {
                    // validate the credentials
                    isValid = pc.ValidateCredentials(UserName, Password);
                }

                if (!isValid)
                {
                    MessageHelper.ShowError(this, "Username or password is not valid.");
                }
            }
            catch (PrincipalServerDownException)
            {
                MessageHelper.ShowError(this, "There was a problem reaching the entered domain.");
            }

            return isValid;
        }

        /// <summary>
        /// Validates the form.
        /// </summary>
        private bool ValidateForm()
        {
            if (radioLocalSystem.Checked)
            {
                return true;
            }

            bool isValid = false;

            var errorMessage = new StringBuilder();

            if (string.IsNullOrEmpty(UserName))
            {
                errorMessage.AppendLine("Enter a Username.");
            }

            if (string.IsNullOrEmpty(Password))
            {
                errorMessage.AppendLine("Enter a password.");
            }

            if (errorMessage.Length > 0)
            {
                MessageHelper.ShowMessage(this, errorMessage.ToString());
            }
            else
            {
                isValid = true;
            }

            return isValid;
        }
    }
}