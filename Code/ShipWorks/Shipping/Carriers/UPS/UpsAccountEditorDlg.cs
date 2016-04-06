using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Window for editing an existing UPS account
    /// </summary>
    public partial class UpsAccountEditorDlg : Form
    {
        UpsAccountEntity account;

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

            bool invoiceAuthenticationRequired = false;

            try
            {
                UpsClerk clerk = new UpsClerk(account);

                UpsRegistrationStatus status = clerk.RegisterAccount(account);

                if (status == UpsRegistrationStatus.InvoiceAuthenticationRequired)
                {
                    invoiceAuthenticationRequired = true;
                }
            }
            catch (UpsWebServiceException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }

            upsRateTypeControl.Initialize(account, false, invoiceAuthenticationRequired);
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
            personControl.SaveToEntity();

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
                account.Description = UpsAccountManager.GetDefaultDescription(account);
            }
            try
            {
                //upsRateTypeControl.RegisterAndSaveToEntity();

                UpsAccountManager.SaveAccount(account);

                DialogResult = DialogResult.OK;
            }
            catch (CarrierException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                return;
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(this, "Your changes cannot be saved because another use has deleted the shipper.");

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
