using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public partial class UspsAutomaticDiscountControl : UserControl
    {
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
        /// Gets a value indicating whether [use expedited].
        /// </summary>
                public bool UseExpedited 
        {
            get { return checkBoxUseExpedited.Checked; }
        }

        /// <summary>
        /// Gets the expedited account identifier.
        /// </summary>
        public long ExpeditedAccountID
        {
            get
            {
                long expeditedAccountID;

                if (expeditedAccounts.Items.Count == 0)
                {
                    expeditedAccountID = 0;
                }
                else
                {
                    expeditedAccountID = (long) expeditedAccounts.SelectedValue;
                }

                return expeditedAccountID;
            }
        }

        /// <summary>
        /// Load the settings
        /// </summary>
        public void LoadSettings()
        {
            promotion = new RegistrationPromotionFactory().CreateRegistrationPromotion();
            
            OnChangeUseExpedited(checkBoxUseExpedited, EventArgs.Empty);

            LoadExpeditedAccounts();
        }

        /// <summary>
        /// Load the UI for USPS Expedited accounts
        /// </summary>
        private void LoadExpeditedAccounts()
        {
            List<UspsAccountEntity> accounts = UspsAccountManager.UspsAccounts.Where(account => account.ContractType == (int) UspsAccountContractType.Reseller).ToList();

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
                LoadExpeditedAccounts();
            }
        }

        /// <summary>
        /// Changing whether account should use an Expedited account for all Priority and Express shipments
        /// </summary>
        private void OnChangeUseExpedited(object sender, EventArgs e)
        {
            panelDiscountAccount.Enabled = checkBoxUseExpedited.Checked;
        }

        /// <summary>
        /// Learn more about using Expedited with USPS provider
        /// </summary>
        private void OnExpedited1LearnMore(object sender, EventArgs e)
        {
            MessageHelper.ShowInformation(this,
                                          "With IntuiShip you get some of the best postal rates available, saving you significant money on each of your " +
                                          "domestic and international Priority and Express shipments." + Environment.NewLine + Environment.NewLine +
                                          "Simply create a USPS account and ShipWorks will automatically utilize it for discounted rates from " +
                                          "IntuiShip when creating postage labels." + Environment.NewLine + Environment.NewLine + "For more information, " +
                                          "please contact us at www.interapptive.com/company/contact.html.");
        }

        /// <summary>
        /// Gets a person to use as the default for new USPS accounts
        /// </summary>
        private PersonAdapter GetDefaultAccountPerson()
        {
            List<UspsAccountEntity> accounts = UspsAccountManager.GetAccounts(UspsResellerType.None);
            return accounts.Count == 1 ? new PersonAdapter(accounts.Single(), "") : null;
        }
    }
}
