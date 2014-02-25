using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Window for resetting an endicia passphrase
    /// </summary>
    public partial class EndiciaPassphraseChangeDlg : Form
    {
        EndiciaAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaPassphraseChangeDlg(EndiciaAccountEntity account, string currentPassphrase)
        {
            InitializeComponent();

            this.account = account;

            current.Text = currentPassphrase;
        }

        /// <summary>
        /// Saving 
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(current.Text))
            {
                MessageHelper.ShowInformation(this, "Please enter your current passphrase.");
                return;
            }

            if (string.IsNullOrWhiteSpace(newPassphrase.Text))
            {
                MessageHelper.ShowInformation(this, "Please enter your new phassphrase.");
                return;
            }

            if (newPassphrase.Text != confirmation.Text)
            {
                MessageHelper.ShowError(this, "The new passphrase and the confirmation do not match.");
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                string passphrase = (new EndiciaApiClient()).ChangeApiPassphrase(account.AccountNumber, (EndiciaReseller)account.EndiciaReseller, current.Text, newPassphrase.Text);

                // We need to update the account entity with the updated passphrase
                account.ApiUserPassword = passphrase;
                account.Fields[(int) EndiciaAccountFieldIndex.ApiUserPassword].ForcedCurrentValueWrite(passphrase, passphrase);
                account.Fields[(int) EndiciaAccountFieldIndex.ApiUserPassword].IsChanged = false;

                // We also need to save it immediately
                EndiciaAccountEntity prototype = new EndiciaAccountEntity(account.EndiciaAccountID) { IsNew = false };
                prototype.ApiUserPassword = passphrase;

                using (SqlAdapter adapater = new SqlAdapter())
                {
                    adapater.SaveEntity(prototype, false);
                }

                MessageHelper.ShowInformation(this, "Your Endicia account passphrase has been changed.\n\nYou will need to update all applications sharing this account with the new passphrase.");

                DialogResult = DialogResult.OK;
            }
            catch (EndiciaException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }
    }
}
