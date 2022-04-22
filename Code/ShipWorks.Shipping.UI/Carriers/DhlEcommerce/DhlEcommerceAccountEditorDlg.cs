using System;
using System.Windows.Forms;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping.UI.Carriers.DhlEcommerce
{
    /// <summary>
    /// DHL eCommerce Account Editor Dialog
    /// </summary>
    [KeyedComponent(typeof(ICarrierAccountEditorDlg), ShipmentTypeCode.DhlEcommerce)]
    public partial class DhlEcommerceAccountEditorDlg : Form, ICarrierAccountEditorDlg
    {
        private readonly DhlEcommerceAccountEntity account;
        private readonly ICarrierAccountRetrieverFactory accountRetrieverFactory;
        private readonly IMessageHelper messageHelper;
        private readonly ICarrierAccountDescription accountDescription;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceAccountEditorDlg(ICarrierAccount account,
            ICarrierAccountRetrieverFactory accountRetrieverFactory,
            IMessageHelper messageHelper,
            IIndex<ShipmentTypeCode, ICarrierAccountDescription> accountDescriptionFactory)
        {
            InitializeComponent();

            var dhlEcommerceAccount = account as DhlEcommerceAccountEntity;
            MethodConditions.EnsureArgumentIsNotNull(dhlEcommerceAccount, "dhlEcommerceAccount");

            this.account = dhlEcommerceAccount;
            this.accountRetrieverFactory = accountRetrieverFactory;
            this.messageHelper = messageHelper;
            this.accountDescription = accountDescriptionFactory[ShipmentTypeCode.Asendia];
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            accountNumber.Text = $"{account.ClientId}-{account.PickupNumber}"; // TODO: DHLECommerce verify with product this is what we want to show here

            description.Text = account.Description;

            description.PromptText = account.Description;
            contactInformation.LoadEntity(account.Address);
        }

        /// <summary>
        /// The address content of the shipper has been edited
        /// </summary>
        private void OnPersonContentChanged(object sender, EventArgs e)
        {
            contactInformation.SaveToEntity();
            description.PromptText = account.Description;
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

                contactInformation.SaveToEntity();
                accountRetrieverFactory.Create(account.ShipmentType).Save(account);

                DialogResult = DialogResult.OK;
            }
            catch (ORMConcurrencyException)
            {
                messageHelper.ShowError(this, "Your changes cannot be saved because another use has deleted the account.");

                DialogResult = DialogResult.Abort;
            }
        }
    }
}