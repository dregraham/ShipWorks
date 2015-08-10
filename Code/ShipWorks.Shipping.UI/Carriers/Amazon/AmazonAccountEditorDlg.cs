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
            description.DataBindings.Add(ObjectUtility.Nameof(() => description.PromptText), viewModel, ObjectUtility.Nameof(() => viewModel.DescriptionPrompt), false, DataSourceUpdateMode.Never);
            description.DataBindings.Add(ObjectUtility.Nameof(() => description.Text), viewModel, ObjectUtility.Nameof(() => viewModel.Description));
        }

        /// <summary>
        /// User is ready to save the changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            contactInformation.SaveToEntity(viewModel.Person);
            viewModel.Save(account);

            if (viewModel.Success)
            {
                DialogResult = DialogResult.OK;
                return;
            }

            MessageHelper.ShowError(this, viewModel.Message);
            DialogResult = DialogResult.Abort;
        }
    }
}