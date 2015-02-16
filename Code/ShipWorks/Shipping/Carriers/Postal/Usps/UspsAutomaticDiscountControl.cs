using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public partial class UspsAutomaticDiscountControl : UserControl
    {
        private ShippingSettingsEntity settings;
        private IUspsAutomaticDiscountControlAdapter discountControlAdapter;
        private IRegistrationPromotion promotion;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsAutomaticDiscountControl"/> class.
        /// </summary>
        public UspsAutomaticDiscountControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        public string HeaderText
        {
            get { return labelDiscountedPostage.Text; }
            set { labelDiscountedPostage.Text = value; }
        }

        /// <summary>
        /// Gets or sets the discount text.
        /// </summary>
        public string DiscountText
        {
            get { return labelDiscountInfo1.Text; }
            set { labelDiscountInfo1.Text = value; }
        }

        /// <summary>
        /// Gets or sets the text displayed for the checkbox.
        /// </summary>
        public string UseExpeditedOptionText
        {
            get { return checkBoxUseExpedited.Text; }
            set { checkBoxUseExpedited.Text = value; }
        }
        
        /// <summary>
        /// Gets a value indicating whether the user opted to [use expedited].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use expedited]; otherwise, <c>false</c>.
        /// </value>
        public bool UseExpedited
        {
            get { return discountControlAdapter.UsingUspsAutomaticExpedited; }
        }

        /// <summary>
        /// Gets the expedited account ID.
        /// </summary>
        public long ExpeditedAccountID
        {
            get { return discountControlAdapter.UspsAutomaticExpeditedAccount; }
        }

        /// <summary>
        /// Load the settings
        /// </summary>
        public void LoadSettings(ShippingSettingsEntity shippingSettings, ShipmentEntity shipment)
        {
            this.settings = shippingSettings;
            discountControlAdapter = new UspsAutomaticDiscountControlAdapterFactory().CreateAdapter(settings, shipment);
            promotion = new RegistrationPromotionFactory().CreateRegistrationPromotion();

            checkBoxUseExpedited.Checked = discountControlAdapter.UsingUspsAutomaticExpedited;
            OnChangeUseExpedited(checkBoxUseExpedited, EventArgs.Empty);

            LoadExpeditedAccounts(settings.StampsUspsAutomaticExpeditedAccount);
        }

        /// <summary>
        /// Load the UI for Stamps.com Expedited accounts
        /// </summary>
        private void LoadExpeditedAccounts(long accountId)
        {
            List<UspsAccountEntity> accounts = UspsAccountManager.UspsAccounts;

            expeditedAccounts.Left = expeditedSignup.Left;
            expeditedAccounts.Width = 250;

            expeditedAccounts.Visible = (accounts.Count > 0);
            expeditedSignup.Visible = (accounts.Count == 0);
            expeditedLearnMore.Visible = (accounts.Count == 0);

            expeditedAccounts.DataSource = null;
            expeditedAccounts.DisplayMember = "Display";
            expeditedAccounts.ValueMember = "Value";

            if (accounts.Count > 0)
            {
                expeditedAccounts.DataSource = accounts.Select(a => new { Display = a.Description, value = a.UspsAccountID }).ToList();
                expeditedAccounts.SelectedValue = accountId;

                if (expeditedAccounts.SelectedIndex < 0)
                {
                    expeditedAccounts.SelectedIndex = accounts.Count - 1;
                }
            }
        }

        /// <summary>
        /// Initiate the signup for a new Express1 account
        /// </summary>
        private void OnSignup(object sender, EventArgs e)
        {
            bool added = false;

            ShippingSettings.CheckForChangesNeeded();

            using (UspsSetupWizard setupWizard = new UspsSetupWizard(promotion, false))
            {
                // Pre-load the account address details
                setupWizard.InitialAccountAddress = GetDefaultAccountPerson();

                UspsShipmentType shipmentType = new UspsShipmentType();
                if (ShippingManager.IsShipmentTypeConfigured(shipmentType.ShipmentTypeCode))
                {
                    added = (setupWizard.ShowDialog(this) == DialogResult.OK);
                }
                else
                {
                    // The shipping type still needs to be set up, so hand off to the shipment setup control
                    added = ShipmentTypeSetupControl.SetupShipmentType(this, shipmentType.ShipmentTypeCode, setupWizard);
                }
            }

            // Reload the account list if a new account has been added
            if (added)
            {
                LoadExpeditedAccounts(UspsAccountManager.UspsAccounts.Max(a => a.UspsAccountID));
            }
        }

        /// <summary>
        /// Changing whether account should use an Expedited account for all Priority and Express shipments
        /// </summary>
        private void OnChangeUseExpedited(object sender, EventArgs e)
        {
            discountControlAdapter.UsingUspsAutomaticExpedited = checkBoxUseExpedited.Checked;
            panelDiscountAccount.Enabled = checkBoxUseExpedited.Checked;
        }

        /// <summary>
        /// The selected Expedited account has changed
        /// </summary>
        private void OnExpeditedAccountsSelectedIndexChanged(object sender, EventArgs e)
        {
            discountControlAdapter.UspsAutomaticExpeditedAccount = (expeditedAccounts.SelectedIndex >= 0) ? (long)expeditedAccounts.SelectedValue : 0;
        }

        /// <summary>
        /// Learn more about using Expedited with USPS provider
        /// </summary>
        private void OnExpedited1LearnMore(object sender, EventArgs e)
        {
            MessageHelper.ShowInformation(this,
                                          "With IntuiShip you get some of the best postal rates available, saving you significant money on each of your " +
                                          "domestic and international Priority and Express shipments." + Environment.NewLine + Environment.NewLine +
                                          "Simply create a Stamps.com account and ShipWorks will automatically utilize it for discounted rates from " +
                                          "IntuiShip when creating postage labels." + Environment.NewLine + Environment.NewLine + "For more information, " +
                                          "please contact us at www.interapptive.com/company/contact.html.");
        }

        /// <summary>
        /// Gets a person to use as the default for new USPS (Stamps.com Expedited) accounts
        /// </summary>
        private PersonAdapter GetDefaultAccountPerson()
        {
            List<UspsAccountEntity> accounts = UspsAccountManager.GetAccounts(UspsResellerType.None);
            return accounts.Count == 1 ? new PersonAdapter(accounts.Single(), "") : null;
        }
    }
}
