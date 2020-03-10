﻿using System;
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
using Autofac.Features.Indexed;

namespace ShipWorks.Shipping.Carriers.DhlExpress
{
    /// <summary>
    /// DHLExpress Account Editor Dialog
    /// </summary>
    [KeyedComponent(typeof(ICarrierAccountEditorDlg),ShipmentTypeCode.DhlExpress)]
    public partial class DhlExpressAccountEditorDlg : Form, ICarrierAccountEditorDlg
    {
        private readonly DhlExpressAccountEntity account;
        private readonly ICarrierAccountRetrieverFactory accountRetrieverFactory;
        private readonly IMessageHelper messageHelper;
        private readonly ICarrierAccountDescription accountDescription;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressAccountEditorDlg(ICarrierAccount account, 
            ICarrierAccountRetrieverFactory accountRetrieverFactory,
            IMessageHelper messageHelper, 
            IIndex<ShipmentTypeCode, ICarrierAccountDescription> accountDescriptionFactory)
        {
            InitializeComponent();

            DhlExpressAccountEntity dhlAccount = account as DhlExpressAccountEntity;
            MethodConditions.EnsureArgumentIsNotNull(dhlAccount, "Dhl Account");

            this.account = dhlAccount;
            this.accountRetrieverFactory = accountRetrieverFactory;
            this.messageHelper = messageHelper;
            this.accountDescription = accountDescriptionFactory[ShipmentTypeCode.DhlExpress];
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (account.UspsAccountId == null)
            {
                accountNumber.Text = account.AccountNumber.ToString();

                if (account.Description != GetDescription(account))
                {
                    description.Text = account.Description;
                }

                description.PromptText = GetDescription(account);
            }
            else
            {
                accountNumber.Text = "DHL Express from ShipWorks";
                description.ReadOnly = true;
                labelOptional.Visible = false;
            }
            
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
        /// Get the account description
        /// </summary>
        private string GetDescription(DhlExpressAccountEntity dhlAccount)
        {
            if (dhlAccount.UspsAccountId != null)
            {
                return "DHL Express from ShipWorks";
            }
            else
            {
                return accountDescription.GetDefaultAccountDescription(account);
            }
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
                accountRetrieverFactory.Create(account.ShipmentType).Save(account);

                DialogResult = DialogResult.OK;
            }
            catch (ORMConcurrencyException)
            {
                messageHelper.ShowError(
                    this, "Your changes cannot be saved because another use has deleted the account.");

                DialogResult = DialogResult.Abort;
            }
        }
    }
}