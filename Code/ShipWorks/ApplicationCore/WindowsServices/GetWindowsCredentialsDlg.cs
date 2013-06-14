using System;
using System.DirectoryServices.AccountManagement;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.ApplicationCore.WindowsServices
{
    /// <summary>
    /// Prompts user for username and password.
    /// </summary>
    public partial class GetWindowsCredentialsDlg : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetWindowsCredentialsDlg"/> class.
        /// </summary>
        public GetWindowsCredentialsDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string UserName
        {
            get
            {
                return username.Text.Trim();
            }
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get
            {
                return password.Text.Trim();
            }
        }

        /// <summary>
        /// Gets the domain. 
        /// </summary>
        /// <value>
        /// The domain. If user enterred nothing, returns null
        /// </value>
        public string Domain
        {
            get
            {
                string trimmedDomain = domain.Text.Trim();

                return string.IsNullOrEmpty(trimmedDomain) ? null : trimmedDomain;
            }
        }

        /// <summary>
        /// Called when [OK] button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnOK(object sender, EventArgs e)
        {
            if (ValidateForm() && ValidateCredentials())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        /// <summary>
        /// Validates the credentials.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private bool ValidateCredentials()
        {
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
                    MessageHelper.ShowMessage(this, "Username or password is not valid.");
                }
            }
            catch (PrincipalServerDownException)
            {
                MessageHelper.ShowMessage(this, "There was a problem reaching the entered domain.");
            }

            return isValid;
        }

        /// <summary>
        /// Validates the form.
        /// </summary>
        private bool ValidateForm()
        {
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

        /// <summary>
        /// Called when [cancel] clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}