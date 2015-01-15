using System;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.EquaShip
{
    /// <summary>
    /// Setup Wizard for EquaShip
    /// </summary>
    public partial class EquashipSetupWizard : ShipmentTypeSetupWizardForm
    {
        EquaShipAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public EquashipSetupWizard()
        {
            InitializeComponent();

            // new account
            account = new EquaShipAccountEntity();
            account.InitializeNullsToDefault();
        }

        /// <summary>
        /// Load/setup the wizard page
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.EquaShip);

            // load the account
            personControl.LoadEntity(new PersonAdapter(account, ""));
            optionsControl.LoadSettings();

            username.Text = account.Username;
            password.Text = SecureText.Decrypt(account.Password, account.Username);

            if (!ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.EquaShip))
            {
                // see if we need to collect configuration information
                Pages.Add(new ShippingWizardPageOrigin(shipmentType));
                Pages.Add(new ShippingWizardPageDefaults(shipmentType));
                Pages.Add(new ShippingWizardPageAutomation(shipmentType));
                Pages.Add(new ShippingWizardPagePrinting(shipmentType));
            }
            else
            {
                Pages.Remove(wizardPageSettings);
            }

            Pages.Add(new ShippingWizardPageFinish(shipmentType));
            Pages[Pages.Count - 1].SteppingInto += new EventHandler<WizardSteppingIntoEventArgs>(OnSteppingIntoFinish);
        }

        /// <summary>
        /// Stepping next from the settings page
        /// </summary>
        private void OnStepNextSettings(object sender, EventArgs e)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            optionsControl.SaveSettings();
            ShippingSettings.Save(settings);
        }

        /// <summary>
        /// Stepping away from the initial page - demographic and account information
        /// </summary>
        private void OnStepNextAccount(object sender, WizardStepEventArgs e)
        {
            account.Username = username.Text.Trim();
            account.Password = SecureText.Encrypt(password.Text.Trim(), account.Username);

            personControl.SaveToEntity(new PersonAdapter(account, string.Empty));

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // validate credentials
                EquaShipClient.TestConnection(account);

                // set the default description
                account.Description = EquaShipAccountManager.GetDefaultDescription(account);

                // attempt to confirm the account information
                EquaShipAccountManager.SaveAccount(account);
            }
            catch (EquaShipException ex)
            {
                MessageHelper.ShowMessage(this, ex.Message);

                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Stepping into the Finish page of the wizard
        /// </summary>
        void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            if (account != null)
            {
                EquaShipAccountManager.SaveAccount(account);
            }
        }

        /// <summary>
        /// Form is closing, so cleanup if necessary
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel && account != null)
            {
                if (!account.IsNew)
                {
                    EquaShipAccountManager.DeleteAccount(account);
                }
            }
        }
    }
}
