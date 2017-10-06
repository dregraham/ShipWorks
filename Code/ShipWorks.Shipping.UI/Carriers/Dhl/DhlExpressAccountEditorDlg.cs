using System;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.Custom;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.DhlExpress
{
    /// <summary>
    /// DHLExpress Account Editor Dialog
    /// </summary>
    [KeyedComponent(typeof(ICarrierAccountEditorDlg),ShipmentTypeCode.DhlExpress)]
    public partial class DhlExpressAccountEditorDlg : Form, ICarrierAccountEditorDlg
    {
        private readonly DhlExpressAccountEntity account;
        private readonly IDhlExpressAccountRepository accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressAccountEditorDlg(ICarrierAccount account, IDhlExpressAccountRepository accountRepository)
        {
            InitializeComponent();

            DhlExpressAccountEntity dhlAccount = account as DhlExpressAccountEntity;
            MethodConditions.EnsureArgumentIsNotNull(dhlAccount, "Dhl Account");

            this.account = dhlAccount;
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            accountNumber.Text = account.AccountNumber.ToString();

            if (account.Description != GetDescription(account))
            {
                description.Text = account.Description;
            }

            description.PromptText = GetDescription(account);
            contactInformation.LoadEntity(account.Address);
        }

        /// <summary>
        /// The address content of the shipper has been edited
        /// </summary>
        private void OnPersonContentChanged(object sender, EventArgs e)
        {
            contactInformation.SaveToEntity();
            description.PromptText = GetDescription(account);
        }

        /// <summary>
        /// Get the accoun description
        /// </summary>
        private static string GetDescription(DhlExpressAccountEntity dhlAccount)
        {
            return DhlExpressAccountManager.GetDefaultDescription(dhlAccount);
        }

        /// <summary>
        /// User is ready to save the changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            try
            {
                if (description.Text.Trim().Length > 0)
                {
                    account.Description = description.Text.Trim();
                }
                else
                {
                    account.Description = GetDescription(account);
                }

                contactInformation.SaveToEntity();
                accountRepository.Save(account);

                DialogResult = DialogResult.OK;
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