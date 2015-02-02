using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Api;
using ShipWorks.UI;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Window for updating the password of a stamps.com account
    /// </summary>
    public partial class StampsAccountUpdatePasswordDlg : Form
    {
        StampsAccountEntity account;
        /// <summary>
        /// Constructor
        /// </summary>
        public StampsAccountUpdatePasswordDlg(StampsAccountEntity account)
        {
            InitializeComponent();

            this.account = account;
            this.username.Text = account.Username;
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

                new StampsWebClient((StampsResellerType)account.StampsReseller).AuthenticateUser(account.Username, password.Text);

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    account.Password = SecureText.Encrypt(password.Text, account.Username);
                    adapter.SaveAndRefetch(account);

                    adapter.Commit();
                }

                StampsAccountManager.CheckForChangesNeeded();

                DialogResult = DialogResult.OK;

            }
            catch (StampsException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }
    }
}
