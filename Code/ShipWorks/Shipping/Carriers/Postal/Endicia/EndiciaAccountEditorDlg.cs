using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Window for editing the endicia account
    /// </summary>
    public partial class EndiciaAccountEditorDlg : Form
    {
        private ITangoWebClient tangoWebClient;
        EndiciaAccountEntity account;
        private EndiciaApiClient endiciaApiClient;

        // Indicates if postage was purchased while the window was open
        bool postagePurchased = false;

        // The password when we opened, so we know if it changed
        string entryPassword;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaAccountEditorDlg(ITangoWebClient tangoWebClient)
        {
            this.tangoWebClient = tangoWebClient;
        }

        /// <summary>
        /// Load the account into this control
        /// </summary>
        public void LoadAccount(EndiciaAccountEntity account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            InitializeComponent();

            endiciaApiClient = new EndiciaApiClient();
            this.account = account;

            EndiciaReseller reseller = (EndiciaReseller) account.EndiciaReseller;

            string resellerName = EndiciaAccountManager.GetResellerName(reseller);
            labelEndicia.Text = resellerName;
            Text = String.Format("{0} Account", resellerName);

            if (reseller == EndiciaReseller.Express1)
            {
                infotipPassword.Caption = "Please contact Express1 at 1-800-399-3971 to retrieve your account password.";
                labelNote.Text = String.Format("Note: {0}", infotipPassword.Caption);
            }

            SetUIValues();
        }

        /// <summary>
        /// Indicates if postage was purchased while the window was open
        /// </summary>
        public bool PostagePurchased
        {
            get { return postagePurchased; }
        }

        /// <summary>
        /// Update UI values based on data
        /// </summary>
        private void SetUIValues()
        {
            EnumHelper.BindComboBox<EndiciaScanFormAddressSource>(comboScanAddress);

            accountNumber.Text = account.AccountNumber;

            entryPassword = SecureText.Decrypt(account.ApiUserPassword, "Endicia");
            password.Text = entryPassword;

            personControl.LoadEntity(new PersonAdapter(account, ""));

            if (account.Description != EndiciaAccountManager.GetDefaultDescription(account))
            {
                description.Text = account.Description;
            }

            description.PromptText = EndiciaAccountManager.GetDefaultDescription(account);

            mailingPostalCode.Text = account.MailingPostalCode;

            comboScanAddress.SelectedValue = (EndiciaScanFormAddressSource) account.ScanFormAddressSource;
        }

        /// <summary>
        /// Window has been shown
        /// </summary>
        private void OnShown(object sender, EventArgs e)
        {
            Refresh();

            LoadAccountBalance();
        }

        /// <summary>
        /// Load the current account balance for the account
        /// </summary>
        private void LoadAccountBalance()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                balance.Text = (new PostageBalance(new EndiciaPostageWebClient(account), tangoWebClient)).Value.ToString("c");
            }
            catch (EndiciaException ex)
            {
                MessageHelper.ShowError(this, "ShipWorks could not load the balance of the account.\n\nError: " + ex.Message);

                balance.Text = "Error";
                buyPostage.Enabled = false;
            }

            buyPostage.Left = balance.Right;
        }

        /// <summary>
        /// The address content of the shipper has been edited
        /// </summary>
        private void OnPersonContentChanged(object sender, EventArgs e)
        {
            personControl.SaveToEntity();
            description.PromptText = EndiciaAccountManager.GetDefaultDescription(account);
        }

        /// <summary>
        /// Purchase more postage for the account
        /// </summary>
        private void OnBuyPostage(object sender, EventArgs e)
        {
            using (EndiciaBuyPostageDlg dlg = new EndiciaBuyPostageDlg(account))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    postagePurchased = true;

                    LoadAccountBalance();
                }
            }
        }

        /// <summary>
        /// Reset the passphrase
        /// </summary>
        private void OnChangePassphrase(object sender, EventArgs e)
        {
            using (EndiciaPassphraseChangeDlg dlg = new EndiciaPassphraseChangeDlg(account, password.Text))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    password.Text = SecureText.Decrypt(account.ApiUserPassword, "Endicia");
                }
            }
        }

        /// <summary>
        /// User is ready to save the changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            personControl.SaveToEntity();

            if (account.FirstName.Length == 0 || account.LastName.Length == 0)
            {
                MessageHelper.ShowError(this, "Enter a first and last name for the account.");
                return;
            }

            if (account.Street1.Length == 0)
            {
                MessageHelper.ShowError(this, "Enter a street address for the account.");
                return;
            }

            if (description.Text.Trim().Length > 0)
            {
                account.Description = description.Text.Trim();
            }
            else
            {
                account.Description = EndiciaAccountManager.GetDefaultDescription(account);
            }

            account.MailingPostalCode = mailingPostalCode.Text.Trim();
            account.ScanFormAddressSource = (int) comboScanAddress.SelectedValue;

            Cursor.Current = Cursors.WaitCursor;

            if (password.Text != entryPassword)
            {
                account.ApiUserPassword = SecureText.Encrypt(password.Text, "Endicia");

                try
                {
                    endiciaApiClient.GetAccountStatus(account);
                }
                catch (EndiciaException ex)
                {
                    MessageHelper.ShowError(this, "ShipWorks could not connect to your account.  The passphrase may be incorrect.\n\nDetail: " + ex.Message);

                    return;
                }
            }

            try
            {
                EndiciaAccountManager.SaveAccount(account);

                DialogResult = DialogResult.OK;
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(this, "Your changes cannot be saved because another user has deleted the account.");

                DialogResult = DialogResult.Abort;
            }
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // Rollback changes if not saved
            if (DialogResult != DialogResult.OK)
            {
                account.RollbackChanges();

                EndiciaAccountManager.CheckForChangesNeeded();
            }
        }
    }
}
