using System;
using System.Windows.Forms;
using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Edit info for a FedEx shipper
    /// </summary>
    public partial class FedExAccountEditorDlg : Form
    {
        FedExAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExAccountEditorDlg(FedExAccountEntity account)
        {
            InitializeComponent();

            this.account = account;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            accountNumber.Text = account.AccountNumber;

            personControl.LoadEntity(new PersonAdapter(account, ""));

            if (account.Description != FedExAccountManager.GetDefaultDescription(account))
            {
                description.Text = account.Description;
            }

            description.PromptText = FedExAccountManager.GetDefaultDescription(account);

            settingsControl.LoadAccount(account);
        }

        /// <summary>
        /// The address content of the shipper has been edited
        /// </summary>
        private void OnPersonContentChanged(object sender, EventArgs e)
        {
            personControl.SaveToEntity();
            description.PromptText = FedExAccountManager.GetDefaultDescription(account);
        }

        /// <summary>
        /// User is ready to save the changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            personControl.SaveToEntity();

            try
            {
                settingsControl.SaveToAccount(account);
            }
            catch (CarrierException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                return;
            }

            if (account.FirstName.Length == 0 || account.LastName.Length == 0)
            {
                MessageHelper.ShowError(this, "Enter a first and last name for the shipper.");
                return;
            }

            if (account.Street1.Length == 0)
            {
                MessageHelper.ShowError(this, "Enter a street address for the shipper.");
                return;
            }

            if (description.Text.Trim().Length > 0)
            {
                account.Description = description.Text.Trim();
            }
            else
            {
                account.Description = FedExAccountManager.GetDefaultDescription(account);
            }

            try
            {
                if (account.Fields[(int) FedExAccountFieldIndex.Letterhead].IsChanged || 
                    account.Fields[(int) FedExAccountFieldIndex.Signature].IsChanged)
                {
                    using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                    {
                        var shippingClerk = scope.Resolve<IFedExShippingClerk>();
                        shippingClerk.PerformUploadImages(account);
                    }
                }

                FedExAccountManager.SaveAccount(account);

                DialogResult = DialogResult.OK;
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(this, "Your changes cannot be saved because another user has deleted the shipper.");

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
            }
        }
    }
}
