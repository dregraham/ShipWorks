using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Window for editing an existing UPS account
    /// </summary>
    public partial class UpsAccountEditorDlg : Form
    {
        private UpsAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsAccountEditorDlg(UpsAccountEntity account)
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

            if (account.Description != UpsAccountManager.GetDefaultDescription(account))
            {
                description.Text = account.Description;
            }

            description.PromptText = UpsAccountManager.GetDefaultDescription(account);

            upsRateTypeControl.Initialize(account, false);
        }

        /// <summary>
        /// The address content of the shipper has been edited
        /// </summary>
        private void OnPersonContentChanged(object sender, EventArgs e)
        {
            personControl.SaveToEntity();
            description.PromptText = UpsAccountManager.GetDefaultDescription(account);
        }

        /// <summary>
        /// User is ready to save the changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            UpdateAccountDescription();
            personControl.SaveToEntity();

            try
            {
                if (!upsRateTypeControl.RegisterAndSaveToEntity())
                {
                    return;
                }

                UpsAccountManager.SaveAccount(account);
                DialogResult = DialogResult.OK;
            }
            catch (CarrierException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(this, "Your changes cannot be saved because another use has deleted the shipper.");
                DialogResult = DialogResult.Abort;
            }
        }

        /// <summary>
        /// Updates the account description
        /// </summary>
        private void UpdateAccountDescription()
        {
            account.Description = string.IsNullOrEmpty(description.Text.Trim()) ?
                                        UpsAccountManager.GetDefaultDescription(account) :
                                        description.Text.Trim();
        }

        /// <summary>
        /// Validates the required fields
        /// </summary>
        private bool ValidateFields()
        {
            if (account.FirstName.Length == 0 || account.LastName.Length == 0)
            {
                MessageHelper.ShowError(this, "Enter a first and last name for the shipper.");
                return false;
            }

            if (account.Street1.Length == 0)
            {
                MessageHelper.ShowError(this, "Enter a street address for the shipper.");
                return false;
            }

            return true;
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
