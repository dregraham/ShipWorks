using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Window for updating the password of a stamps.com account
    /// </summary>
    public partial class UspsAccountUpdatePasswordDlg : Form
    {
        readonly UspsAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsAccountUpdatePasswordDlg(UspsAccountEntity account)
        {
            InitializeComponent();

            this.account = account;
            username.Text = account.Username;
        }

        /// <summary>
        /// OK to try to update the password
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(password.Text))
            {
                MessageHelper.ShowError(this, "Please enter a password.");
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                new UspsWebClient((UspsResellerType)account.UspsReseller).AuthenticateUser(account.Username, password.Text);

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    account.Password = SecureText.Encrypt(password.Text, account.Username);
                    adapter.SaveAndRefetch(account);

                    adapter.Commit();
                }

                StampsAccountManager.CheckForChangesNeeded();

                DialogResult = DialogResult.OK;

            }
            catch (UspsException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }
    }
}
