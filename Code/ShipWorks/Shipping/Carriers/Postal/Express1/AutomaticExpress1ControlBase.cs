using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Microsoft.Web.Services3.Security;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Automatically use Express1 when appropriate settings
    /// </summary>
    public partial class AutomaticExpress1ControlBase : UserControl
    {
        private IExpress1SettingsFacade express1Settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public AutomaticExpress1ControlBase()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Load the settings
        /// </summary>
        public void LoadSettings(IExpress1SettingsFacade settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            express1Settings = settings;

            checkBoxUseExpress1.Checked = express1Settings.UseExpress1;
            OnChangeUseExpress1(checkBoxUseExpress1, EventArgs.Empty);

            LoadExpress1Accounts(settings.Express1Account);
        }

        /// <summary>
        /// Load the UI for endicia express 1 accounts
        /// </summary>
        private void LoadExpress1Accounts(long accountId)
        {
            ICollection<KeyValuePair<string, long>> accounts = express1Settings.Express1Accounts;

            express1Accounts.Left = express1Signup.Left;
            express1Accounts.Width = 250;

            express1Accounts.Visible = (accounts.Count > 0);
            express1Signup.Visible = (accounts.Count == 0);
            express1LearnMore.Visible = (accounts.Count == 0);
            
            express1Accounts.DataSource = null;
            express1Accounts.DisplayMember = "Display";
            express1Accounts.ValueMember = "Value";

            if (accounts.Count > 0)
            {
                express1Accounts.DataSource = accounts.Select(a => new { Display = a.Key, value = a.Value }).ToList();
                express1Accounts.SelectedValue = accountId;

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

            using (ILifetimeScope lifetimeScope = IoC.Current.BeginLifetimeScope())
            {
                using (Form setupDlg = shipmentType.CreateSetupWizard(lifetimeScope))
                {
                    // Ensure that the setup dialog is actually an Express1 setup wizard
                    var setupWizard = setupDlg as Express1SetupWizard;
                    Debug.Assert(setupWizard != null, "AutomaticExpress1Control can only create Express1 shipment types.");

                    // Pre-load the account address details
                    setupWizard.InitialAccountAddress = express1Settings.DefaultAccountPerson;

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
            }

            // Reload the account list if a new account has been added
            if (added)
            {
                LoadExpress1Accounts(express1Settings.Express1Accounts.Max(x => x.Value));
            }
        }

        /// <summary>
        /// Changing whether account should use an Express1 account for all Priority and Express shipments
        /// </summary>
        private void OnChangeUseExpress1(object sender, EventArgs e)
        {
            express1Settings.UseExpress1 = checkBoxUseExpress1.Checked;
            panelExpress1Account.Enabled = checkBoxUseExpress1.Checked;
        }

        /// <summary>
        /// The selected Express1 account has changed
        /// </summary>
        private void OnExpress1AccountsSelectedIndexChanged(object sender, EventArgs e)
        {
            express1Settings.Express1Account =
                (express1Accounts.SelectedIndex >= 0) ? (long)express1Accounts.SelectedValue : 0;
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
