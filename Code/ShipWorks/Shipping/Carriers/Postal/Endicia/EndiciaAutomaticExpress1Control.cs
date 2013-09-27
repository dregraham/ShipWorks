using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using Interapptive.Shared.Business;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Automatically use Express1 when appropriate settings
    /// </summary>
    public partial class EndiciaAutomaticExpress1Control : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaAutomaticExpress1Control()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the settings
        /// </summary>
        public void LoadSettings()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            checkBoxUseExpress1.Checked = settings.EndiciaAutomaticExpress1;
            OnEndiciaChangeUseExpress1(checkBoxUseExpress1, EventArgs.Empty);

            LoadEndiciaExpress1Accounts(settings.EndiciaAutomaticExpress1Account);
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        public void SaveSettings(ShippingSettingsEntity settings)
        {
            settings.EndiciaAutomaticExpress1 = checkBoxUseExpress1.Checked;
            settings.EndiciaAutomaticExpress1Account = (express1Accounts.SelectedIndex >= 0) ? (long) express1Accounts.SelectedValue : 0;                
        }

        /// <summary>
        /// Load the UI for endicia express 1 accounts
        /// </summary>
        private void LoadEndiciaExpress1Accounts(long accountID)
        {
            List<EndiciaAccountEntity> accounts = EndiciaAccountManager.GetAccounts(EndiciaReseller.Express1);

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
                express1Accounts.DataSource = accounts.Select(a => new { Display = a.Description, Value = a.EndiciaAccountID }).ToList();
                express1Accounts.SelectedValue = accountID;

                if (express1Accounts.SelectedIndex < 0)
                {
                    express1Accounts.SelectedIndex = accounts.Count - 1;
                }
            }
        }

        /// <summary>
        /// Initiate the signup for a new express1 account
        /// </summary>
        private void OnEndiciaExpress1Signup(object sender, EventArgs e)
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
        /// Changing whether Endicia should use an Express1 account for all Priority and Express shipments
        /// </summary>
        private void OnEndiciaChangeUseExpress1(object sender, EventArgs e)
        {
            panelExpress1Account.Enabled = checkBoxUseExpress1.Checked;
        }

        /// <summary>
        /// Learn more about using Express1 with Endicia
        /// </summary>
        private void OnEndiciaExpress1LearnMore(object sender, EventArgs e)
        {
            MessageHelper.ShowInformation(this,
                "With Express1 you get some of the best postal rates available, saving you significant money on each of your domestic and international Priority and Express " +
                "shipments.\n\n" +
                "Simply create a free Express1 account and ShipWorks will automatically utilize it for discounted rates when creating postal labels.\n\n" +
                "For more information please contact us at www.interapptive.com/company/contact.html.");
        }
    }
}
