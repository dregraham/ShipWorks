using System;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Authentication;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// OnTrac Account Editor Dialog
    /// </summary>
    public partial class OnTracAccountEditorDlg : Form
    {
        private readonly OnTracAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracAccountEditorDlg(OnTracAccountEntity account)
        {
            InitializeComponent();

            this.account = account;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            accountNumber.Text = account.AccountNumber.ToString();
            password.Text = SecureText.Decrypt(account.Password, account.AccountNumber.ToString());

            if (account.Description != OnTracAccountManager.GetDefaultDescription(account))
            {
                description.Text = account.Description;
            }

            description.PromptText = OnTracAccountManager.GetDefaultDescription(account);

            PersonAdapter personAdapter = new PersonAdapter(account, "");
            contactInformation.LoadEntity(personAdapter);
        }

        /// <summary>
        /// The address content of the shipper has been edited
        /// </summary>
        private void OnPersonContentChanged(object sender, EventArgs e)
        {
            contactInformation.SaveToEntity();
            description.PromptText = OnTracAccountManager.GetDefaultDescription(account);
        }

        /// <summary>
        /// User is ready to save the changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            try
            {
                // If the password changed, check with OnTrac to confirm it correct
                if (SecureText.Decrypt(account.Password, account.AccountNumber.ToString()) != password.Text.Trim())
                {
                    Cursor.Current = Cursors.WaitCursor;

                    var authenticationRequest = new OnTracAuthentication(account.AccountNumber, password.Text.Trim());
                    authenticationRequest.IsValidUser();
                }

                account.Password = SecureText.Encrypt(password.Text.Trim(), account.AccountNumber.ToString());

                if (description.Text.Trim().Length > 0)
                {
                    account.Description = description.Text.Trim();
                }
                else
                {
                    account.Description = OnTracAccountManager.GetDefaultDescription(account);
                }

                contactInformation.SaveToEntity();

                OnTracAccountManager.SaveAccount(account);

                DialogResult = DialogResult.OK;
            }
            catch (OnTracException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(
                    this, "Your changes cannot be saved because another use has deleted the account.");

                DialogResult = DialogResult.Abort;
            }
        }
    }
}