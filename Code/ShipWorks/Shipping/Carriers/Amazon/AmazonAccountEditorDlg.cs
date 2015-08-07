using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// OnTrac Account Editor Dialog
    /// </summary>
    public partial class AmazonAccountEditorDlg : Form
    {
        private readonly AmazonAccountEditorViewModel viewModel;
        private AmazonAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonAccountEditorDlg(AmazonAccountEditorViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
        }

        /// <summary>
        /// Load the given account
        /// </summary>
        public void LoadAccount(AmazonAccountEntity amazonAccount)
        {
            account = amazonAccount;

            viewModel.Load(account);
            contactInformation.LoadEntity(viewModel.Person);
        } 

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            merchantId.DataBindings.Add(ObjectUtility.Nameof(() => merchantId.Text), viewModel.Credentials, ObjectUtility.Nameof(() => viewModel.Credentials.MerchantId));
            authToken.DataBindings.Add(ObjectUtility.Nameof(() => authToken.Text), viewModel.Credentials, ObjectUtility.Nameof(() => viewModel.Credentials.AuthToken));

            description.PromptText = viewModel.DescriptionPrompt;
            description.Text = viewModel.Description;

            //description.DataBindings.Add(ObjectUtility.Nameof(() => description.PromptText), viewModel, ObjectUtility.Nameof(() => viewModel.DescriptionPrompt));
            //description.DataBindings.Add(ObjectUtility.Nameof(() => description.Text), viewModel, ObjectUtility.Nameof(() => viewModel.Description));

            //accountNumber.Text = account.AccountNumber.ToString();
            //password.Text = SecureText.Decrypt(account.Password, account.AccountNumber.ToString());

            //if (account.Description != OnTracAccountManager.GetDefaultDescription(account))
            //{
            //    description.Text = account.Description;
            //}

            //description.PromptText = OnTracAccountManager.GetDefaultDescription(account);

            //PersonAdapter personAdapter = new PersonAdapter(account, "");
            //contactInformation.LoadEntity(personAdapter);
        }

        ///// <summary>
        ///// The address content of the shipper has been edited
        ///// </summary>
        //private void OnPersonContentChanged(object sender, EventArgs e)
        //{
        //    contactInformation.SaveToEntity();
        //    description.PromptText = OnTracAccountManager.GetDefaultDescription(account);
        //}

        /// <summary>
        /// User is ready to save the changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            viewModel.Description = description.Text;

            contactInformation.SaveToEntity(viewModel.Person);
            viewModel.Save(account);

            if (viewModel.Success)
            {
                DialogResult = DialogResult.OK;
                return;
            }

            MessageHelper.ShowError(this, viewModel.Message);
            DialogResult = DialogResult.Abort;
            //try
            //{
            //    // If the password changed, check with OnTrac to confirm it correct
            //    if (SecureText.Decrypt(account.Password, account.AccountNumber.ToString()) != password.Text.Trim())
            //    {
            //        Cursor.Current = Cursors.WaitCursor;

            //        var authenticationRequest = new OnTracAuthentication(account.AccountNumber, password.Text.Trim());
            //        authenticationRequest.IsValidUser();
            //    }

            //    account.Password = SecureText.Encrypt(password.Text.Trim(), account.AccountNumber.ToString());

            //    if (description.Text.Trim().Length > 0)
            //    {
            //        account.Description = description.Text.Trim();
            //    }
            //    else
            //    {
            //        account.Description = OnTracAccountManager.GetDefaultDescription(account);
            //    }

            //    contactInformation.SaveToEntity();

            //    OnTracAccountManager.SaveAccount(account);

            //    DialogResult = DialogResult.OK;
            //}
            //catch (OnTracException ex)
            //{
            //    MessageHelper.ShowError(this, ex.Message);
            //}
            //catch (ORMConcurrencyException)
            //{
            //    MessageHelper.ShowError(
            //        this, "Your changes cannot be saved because another use has deleted the account.");

            //    DialogResult = DialogResult.Abort;
            //}
        }
    }
}