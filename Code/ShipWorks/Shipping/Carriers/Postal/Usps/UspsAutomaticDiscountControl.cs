using System;
using System.Collections.Generic;
using System.Drawing;
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
            set
            {
                labelDiscountInfo1.Text = value;
                using (Graphics g = CreateGraphics())
                {
                    // Adjust the location of the checkbox and account panel based on the size of the
                    // text otherwise the description text may appear truncated
                    SizeF size = g.MeasureString(labelDiscountInfo1.Text, labelDiscountInfo1.Font, labelDiscountInfo1.Size);
                    
                    checkBoxUseExpedited.Top = labelDiscountInfo1.Top + (int) size.Height + 5;
                    panelDiscountAccount.Top = checkBoxUseExpedited.Bottom + 5;
                }
            }
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

            LoadResellerAccounts();
        }

        /// <summary>
        /// Load the UI for USPS reseller accounts
        /// </summary>
        private void LoadResellerAccounts()
        {
            LoadAccounts(UspsAccountManager.UspsAccounts.Where(account => account.ContractType == (int)UspsAccountContractType.Reseller).ToList());
        }

        /// <summary>
        /// Load the UI with the accounts provided.
        /// </summary>
        private void LoadAccounts(List<UspsAccountEntity> accounts)
        {
            expeditedAccounts.Left = expeditedSignup.Left;
            expeditedAccounts.Width = 250;

            expeditedAccounts.Visible = (accounts.Count > 0);
            expeditedSignup.Visible = (accounts.Count == 0);
            expeditedLearnMore.Visible = (accounts.Count == 0);

            expeditedAccounts.DataSource = null;
            expeditedAccounts.DisplayMember = "Display";
            expeditedAccounts.ValueMember = "Value";

            if (accounts.Any())
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
                LoadResellerAccounts();

                if (expeditedAccounts.Items.Count == 0)
                {
                    // We know we added a new account but haven't gotten the contract type back yet,
                    // so force the account to be loaded into the dropdown. Otherwise, you won't be able
                    // to close the postage discount dialog this control is hosted in.
                    LoadAccounts(UspsAccountManager.UspsAccounts);
                }
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
