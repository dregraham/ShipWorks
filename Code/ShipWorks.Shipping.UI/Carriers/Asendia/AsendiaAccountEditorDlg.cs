﻿using System;
using System.Windows.Forms;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping.UI.Carriers.Asendia
{
    /// <summary>
    /// Asendia Account Editor Dialog
    /// </summary>
    [KeyedComponent(typeof(ICarrierAccountEditorDlg),ShipmentTypeCode.Asendia)]
    public partial class AsendiaAccountEditorDlg : Form, ICarrierAccountEditorDlg
    {
        private readonly AsendiaAccountEntity account;
        private readonly ICarrierAccountRetrieverFactory accountRetrieverFactory;
        private readonly IMessageHelper messageHelper;
        private readonly ICarrierAccountDescription accountDescription;

        /// <summary>
        /// Constructor
        /// </summary>
        public AsendiaAccountEditorDlg(ICarrierAccount account, 
            ICarrierAccountRetrieverFactory accountRetrieverFactory,
            IMessageHelper messageHelper, 
            IIndex<ShipmentTypeCode, ICarrierAccountDescription> accountDescriptionFactory)
        {
            InitializeComponent();

            AsendiaAccountEntity asendiaAccount = account as AsendiaAccountEntity;
            MethodConditions.EnsureArgumentIsNotNull(asendiaAccount, "asendiaAccount");

            this.account = asendiaAccount;
            this.accountRetrieverFactory = accountRetrieverFactory;
            this.messageHelper = messageHelper;
            this.accountDescription = accountDescriptionFactory[ShipmentTypeCode.Asendia];
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
        /// Get the account description
        /// </summary>
        private string GetDescription(AsendiaAccountEntity asendiaAccount)
        {
            return accountDescription.GetDefaultAccountDescription(account);
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