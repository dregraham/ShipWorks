using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.UI;
using Interapptive.Shared.Business;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Automatically use Express1 when appropriate settings
    /// </summary>
    public partial class AutomaticExpress1ControlBase : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AutomaticExpress1ControlBase()
        {
            InitializeComponent();
        }

        private IExpress1SettingsFacade express1Settings;

        /// <summary>
        /// Gets whether the use wants to use Express1
        /// </summary>
        public bool UseExpress1
        {
            get { return checkBoxUseExpress1.Checked; }
        }

        /// <summary>
        /// Load the settings
        /// </summary>
        public void LoadSettings(IExpress1SettingsFacade express1Settings)
        {
            this.express1Settings = express1Settings;

            checkBoxUseExpress1.Checked = express1Settings.UseExpress1;
            OnChangeUseExpress1(checkBoxUseExpress1, EventArgs.Empty);

            LoadEndiciaExpress1Accounts();
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        /// <param name="settings"></param>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            express1Settings.UseExpress1 = checkBoxUseExpress1.Checked;
            express1Settings.Express1Account = 
                (express1Accounts.SelectedIndex >= 0) ? (long)express1Accounts.SelectedValue : 0;
        }

        /// <summary>
        /// Load the UI for endicia express 1 accounts
        /// </summary>
        private void LoadEndiciaExpress1Accounts()
        {
            ICollection<KeyValuePair<string, long>> accounts = express1Settings.GetAccountList();

            express1Accounts.Left = express1Signup.Left;
            express1Accounts.Width = 250;

            express1Accounts.Visible = (accounts.Count > 0);
            express1Signup.Visible = (accounts.Count == 0);
            express1LearnMore.Visible = (accounts.Count == 0);

            express1Accounts.DisplayMember = "Display";
            express1Accounts.ValueMember = "Value";
            express1Accounts.DataSource = null;

            if (accounts.Count > 0)
            {
                express1Accounts.DataSource = accounts.ToList();
                express1Accounts.SelectedValue = accounts.Max(x=>x.Value);

                if (express1Accounts.SelectedIndex < 0)
                {
                    express1Accounts.SelectedIndex = accounts.Count - 1;
                }
            }
        }

        /// <summary>
        /// Initiate the signup for a new Express1 account
        /// </summary>
        private void OnExpress1Signup(object sender, EventArgs e)
        {
            bool added = false;

            ShippingSettings.CheckForChangesNeeded();

            ShipmentType shipmentType = express1Settings.ShipmentType;

            using (Form setupDlg = shipmentType.CreateSetupWizard())
            {
                // Ensure that the setup dialog is actually an Express1 setup wizard
                var setupWizard = setupDlg as Express1SetupWizard;
                Debug.Assert(setupWizard != null, "AutomaticExpress1Control can only create Express1 shipment types.");

                // Pre-load the account address details
                setupWizard.InitialAccountAddress = GetPersonFromFirstAccount();

                if (ShippingManager.IsShipmentTypeConfigured(shipmentType.ShipmentTypeCode))
                {
                    // The shipping type has already been set up, so just add a new account
                    setupWizard.ForceAccountOnly = true;

                    added = (setupWizard.ShowDialog(this) == DialogResult.OK);
                }
                else
                {
                    // The shipping type still needs to be set up, so hand off to the shipment setup control
                    setupWizard.HideDetailedConfiguration = true;

                    added = ShipmentTypeSetupControl.SetupShipmentType(this, shipmentType.ShipmentTypeCode, setupWizard);  
                }
            }

            if (added)
            {
                LoadEndiciaExpress1Accounts();
            }
        }

        /// <summary>
        /// Gets a person from the first account of the current type that will be used to 
        /// populate the defaults of the address control for the new Express1 account
        /// </summary>
        /// <returns></returns>
        private PersonAdapter GetPersonFromFirstAccount()
        {
            if (EndiciaAccountManager.GetAccounts(EndiciaReseller.None).Count == 1)
            {
                return new PersonAdapter(EndiciaAccountManager.GetAccounts(EndiciaReseller.None)[0], "");
            }

            return null;
        }

        /// <summary>
        /// Changing whether account should use an Express1 account for all Priority and Express shipments
        /// </summary>
        private void OnChangeUseExpress1(object sender, EventArgs e)
        {
            panelExpress1Account.Enabled = checkBoxUseExpress1.Checked;
        }

        /// <summary>
        /// Learn more about using Express1 with USPS provider
        /// </summary>
        private void OnExpress1LearnMore(object sender, EventArgs e)
        {
            MessageHelper.ShowInformation(this,
                "With Express1 you get some of the best postal rates available, saving you significant money on each of your domestic and international Priority and Express " +
                "shipments.\n\n" +
                "Simply create a free Express1 account and ShipWorks will automatically utilize it for discounted rates when creating postal labels.\n\n" +
                "For more information please contact us at www.interapptive.com/company/contact.html.");
        }
    }
}
