﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
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

        private Func<ShippingSettingsEntity, bool> getUseExpress1;
        private Action<ShippingSettingsEntity, bool> setUseExpress1;
        private Func<ShippingSettingsEntity, long> getExpress1Account;
        private Action<ShippingSettingsEntity, long> setExpress1Account;

        public void SetAccessors(Func<ShippingSettingsEntity, bool> useExpress1Getter, 
            Action<ShippingSettingsEntity, bool> useExpress1Setter,
            Func<ShippingSettingsEntity, long> express1AccountGetter,
            Action<ShippingSettingsEntity, long> express1AccountSetter )
        {
            getUseExpress1 = useExpress1Getter;
            setUseExpress1 = useExpress1Setter;
            getExpress1Account = express1AccountGetter;
            setExpress1Account = express1AccountSetter;
        }

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
        public void LoadSettings()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            checkBoxUseExpress1.Checked = getUseExpress1(settings);
            OnChangeUseExpress1(checkBoxUseExpress1, EventArgs.Empty);

            LoadEndiciaExpress1Accounts(getExpress1Account(settings));
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        public virtual void SaveSettings(ShippingSettingsEntity settings)
        {
            setUseExpress1(settings, checkBoxUseExpress1.Checked);
            setExpress1Account(settings,
                               (express1Accounts.SelectedIndex >= 0) ? (long) express1Accounts.SelectedValue : 0);                
        }

        /// <summary>
        /// Load the UI for endicia express 1 accounts
        /// </summary>
        private void LoadEndiciaExpress1Accounts(long accountID)
        {
            var accounts = GetAccountList();

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
                express1Accounts.SelectedValue = accountID;

                if (express1Accounts.SelectedIndex < 0)
                {
                    express1Accounts.SelectedIndex = accounts.Count - 1;
                }
            }
        }

        /// <summary>
        /// TODO: Move this into the facade class
        /// </summary>
        /// <returns></returns>
        protected virtual ICollection<KeyValuePair<string, long>> GetAccountList()
        {
            throw new NotImplementedException("This method must be overridden in subclasses.");
        }

        /// <summary>
        /// Initiate the signup for a new express1 account
        /// </summary>
        private void OnExpress1Signup(object sender, EventArgs e)
        {
            bool added = false;

            ShippingSettings.CheckForChangesNeeded();

            if (ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.Express1Endicia))
            {
                using (Express1EndiciaSetupWizard setupDlg = new Express1EndiciaSetupWizard(true))
                {
                    if (EndiciaAccountManager.GetAccounts(EndiciaReseller.None).Count == 1)
                    {
                        setupDlg.InitialAccountAddress = new PersonAdapter(EndiciaAccountManager.GetAccounts(EndiciaReseller.None)[0], "");
                    }

                    added = (setupDlg.ShowDialog(this) == DialogResult.OK);
                }
            }
            else
            {
                Express1EndiciaSetupWizard setupDlg = (Express1EndiciaSetupWizard) ShipmentTypeManager.GetType(ShipmentTypeCode.Express1Endicia).CreateSetupWizard();
                setupDlg.HideDetailedConfiguration = true;

                if (EndiciaAccountManager.GetAccounts(EndiciaReseller.None).Count == 1)
                {
                    setupDlg.InitialAccountAddress = new PersonAdapter(EndiciaAccountManager.GetAccounts(EndiciaReseller.None)[0], "");
                }

                added = ShipmentTypeSetupControl.SetupShipmentType(this, ShipmentTypeCode.Express1Endicia, setupDlg);
            }

            if (added)
            {
                LoadEndiciaExpress1Accounts(EndiciaAccountManager.GetAccounts(EndiciaReseller.Express1).Max(a => a.EndiciaAccountID));
            }
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
