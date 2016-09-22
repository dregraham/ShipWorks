using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Users.Logon
{
    /// <summary>
    /// Window for sending a user their username
    /// </summary>
    public partial class ForgotUsernameDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ForgotUsernameDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Send the username to the email address
        /// </summary>
        private void OnSendUsername(object sender, EventArgs e)
        {
            string email = textBox.Text.Trim();

            if (email.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Please enter your email address.");
                return;
            }

            using (SqlAdapter adapter = new SqlAdapter())
            {
                UserCollection users = UserCollection.Fetch(adapter, UserFields.Email == email);
                if (users.Count == 0)
                {
                    MessageHelper.ShowMessage(this, "No ShipWorks users were found with that email address.");
                    return;
                }

                UserEntity user = users[0];

                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    TangoWebClient.SendAccountUsername(user.Email, user.Username);

                    MessageHelper.ShowInformation(this, "Your username has been emailed to " + email + ".");

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
