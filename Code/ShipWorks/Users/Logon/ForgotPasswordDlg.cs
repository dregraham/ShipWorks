using System;
using System.Windows.Forms;
using Interapptive.Shared.Security;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.Licensing;
using Interapptive.Shared.UI;

namespace ShipWorks.Users.Logon
{
    /// <summary>
    /// Window for sending a user their password
    /// </summary>
    public partial class ForgotPasswordDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ForgotPasswordDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Send the user the password
        /// </summary>
        private void OnSendPassword(object sender, EventArgs e)
        {
            string usernameOrEmail = textBox.Text.Trim();

            if (usernameOrEmail.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Please enter your username or email address.");
                return;
            }

            using (SqlAdapter adapter = new SqlAdapter())
            {
                UserCollection users = UserCollection.Fetch(adapter, UserFields.Email == usernameOrEmail | UserFields.Username == usernameOrEmail);
                if (users.Count == 0)
                {
                    MessageHelper.ShowMessage(this, "No ShipWorks users were found with that username or email address.");
                    return;
                }

                UserEntity user = users[0];

                // Make sure there is an email - there wouldn't be if coming from 2x and hadn't been updated yet
                if (string.IsNullOrWhiteSpace(user.Email))
                {
                    MessageHelper.ShowError(this, string.Format("The user '{0}' does not have an email address configured in ShipWorks, and a new password could not be emailed.", user.Username));
                    return;
                }

                // Create a new password
                string password = SecureText.Encrypt(Guid.NewGuid().ToString(), "Whatever");
                password = password.Substring(0, Math.Min(password.Length, 8));

                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    // Make sure we can send it to them...
                    TangoWebClient.SendAccountPassword(user.Email, password);

                    // And then update the database
                    user.Password = UserUtility.HashPassword(password);

                    // Save the password
                    adapter.SaveAndRefetch(user);

                    MessageHelper.ShowInformation(this, "A new password has been emailed to " + user.Email + ".");

                    DialogResult = DialogResult.OK;
                }
                catch (TangoException ex)
                {
                    MessageHelper.ShowError(this, ex.Message);
                    return;
                }
            }
        }
    }
}
