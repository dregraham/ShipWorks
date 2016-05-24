using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Wizard for registering to use FedEx
    /// </summary>
    public partial class FedExSetupWizard : ShipmentTypeSetupWizardForm
    {
        FedExAccountEntity account;

        // track if the account is one being migrated from 2
        bool migrating2xAccount = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExSetupWizard()
        {
            InitializeComponent();

            account = new FedExAccountEntity();
        }

        /// <summary>
        /// Constructor for specifying the account to be configured
        /// </summary>
        public FedExSetupWizard(FedExAccountEntity account)
        {
            InitializeComponent();

            if (!account.Is2xMigrationPending)
            {
                throw new InvalidOperationException("This constructor is only meant for accounts that need migrated to 3x");
            }

            this.account = account;
            migrating2xAccount = true;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.FedEx);

            // Some values are already configured via the SW upgrade
            if (!migrating2xAccount)
            {
                account.CountryCode = "US";
                account.SmartPostHubList = "<Root />";
                account.InitializeNullsToDefault();
            }

            personControl.LoadEntity(new PersonAdapter(account, ""));
            accountSettingsControl.LoadAccount(account);

            accountNumber.Text = account.AccountNumber;

            // If its not already setup or configured, load all the the settings\configuration pages
            if (!ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.FedEx))
            {
                Pages.Add(new ShippingWizardPageOrigin(shipmentType));
                Pages.Add(new ShippingWizardPageDefaults(shipmentType));
                Pages.Add(new ShippingWizardPagePrinting(shipmentType));
                Pages.Add(new ShippingWizardPageAutomation(shipmentType));

                // If we are not migrating a specific account - but all accounts are not migrated, that means we're here JUST to go through configuration,
                // and not specific account setup.
                if (!migrating2xAccount && FedExAccountManager.Accounts.Count > 0 && FedExAccountManager.Accounts.All(a => a.Is2xMigrationPending))
                {
                    Pages.Remove(wizardPageInitial);
                    Pages.Remove(wizardPageContactInfo);
                    Pages.Remove(wizardPageSettings);

                    account = null;
                }
                else
                {
                    optionsControl.LoadSettings();
                }
            }
            // Otherwise it will just be to setup the account
            else
            {
                optionsControl.Visible = false;
                accountSettingsControl.Top = optionsControl.Top;
            }

            Pages.Add(new ShippingWizardPageFinish(shipmentType));
            Pages[Pages.Count - 1].SteppingInto += new EventHandler<WizardSteppingIntoEventArgs>(OnSteppingIntoFinish);

            licenseAgreement.Rtf = ResourceUtility.ReadString("ShipWorks.Shipping.Carriers.FedEx.FedExEULA.rtf");
        }

        /// <summary>
        /// Open the FedEx website
        /// </summary>
        private void OnClickLinkFedEx(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.fedex.com/us/oadr/en/discounts/index.html", this);
        }

        /// <summary>
        /// Stepping next from the initial page.
        /// </summary>
        private void OnStepNextInitialPage(object sender, WizardStepEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(accountNumber.Text))
            {
                MessageHelper.ShowInformation(this, "Please enter your FedEx account number.");
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Stepping next from the account information page
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnStepNextAccountInfo(object sender, WizardStepEventArgs e)
        {
            account.AccountNumber = accountNumber.Text;
            account.SignatureRelease = "";

            personControl.SaveToEntity(new PersonAdapter(account, string.Empty));

            account.Phone = new string(account.Phone.Where(char.IsDigit).ToArray());

            RequiredFieldChecker checker = new RequiredFieldChecker();
            checker.Check("Account", account.AccountNumber);
            checker.Check("Full Name", account.FirstName);
            checker.Check("Company", account.Company);
            checker.Check("Street Address", account.Street1);
            checker.Check("City", account.City);
            checker.Check("State", account.StateProvCode);
            checker.Check("Postal Code", account.PostalCode);
            checker.Check("Email", account.Email);
            checker.Check("Phone", account.Phone);
            checker.Check("Website", account.Website);

            if (!checker.Validate(this))
            {
                e.NextPage = CurrentPage;
                return;
            }

            if (account.Phone.Length != 10)
            {
                e.NextPage = CurrentPage;
                MessageHelper.ShowError(this, "The phone number must be 10 digits.");
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                IShippingClerk clerk = new FedExShippingClerkFactory().CreateShippingClerk(null, new FedExSettingsRepository());
                clerk.RegisterAccount(account);

                account.Description = FedExAccountManager.GetDefaultDescription(account);

                // Save now so it shows up in the settings section
                if (account.IsNew)
                {
                    FedExAccountManager.SaveAccount(account);
                }
            }
            catch (FedExException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Stepping next from the settings page
        /// </summary>
        private void OnStepNextSettings(object sender, WizardStepEventArgs e)
        {
            if (optionsControl.Visible)
            {
                ShippingSettingsEntity settings = ShippingSettings.Fetch();
                optionsControl.SaveSettings(settings);
                ShippingSettings.Save(settings);
            }

            try
            {
                accountSettingsControl.SaveToAccount(account);
            }
            catch (CarrierException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Stepping into the final page
        /// </summary>
        private void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            if (account != null)
            {
                FedExAccountManager.SaveAccount(account);
            }
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // If we are canceling - but had created an account - we need to undo that
            if (DialogResult == DialogResult.Cancel && account != null)
            {
                if (!account.IsNew && !migrating2xAccount)
                {
                    FedExAccountManager.DeleteAccount(account);
                }
                else
                {
                    account.RollbackChanges();
                }
            }
            else if (DialogResult == DialogResult.OK)
            {
                // We need to clear out the rate cache since rates (especially best rate) are no longer valid now
                // that a new account has been added.
                RateCache.Instance.Clear();

                if (FedExAccountManager.Accounts.Count == 1)
                {
                    // Update any profiles to use this FedEx account if this is the only account
                    // in the system. This is to account for the situation where there a multiple
                    // profiles that may be associated with a previous FedEx account that has since
                    // been deleted.
                    foreach (ShippingProfileEntity shippingProfileEntity in ShippingProfileManager.Profiles.Where(p => p.ShipmentType == (int) ShipmentTypeCode.FedEx))
                    {
                        if (shippingProfileEntity.FedEx.FedExAccountID.HasValue)
                        {
                            shippingProfileEntity.FedEx.FedExAccountID = account.FedExAccountID;
                            ShippingProfileManager.SaveProfile(shippingProfileEntity);
                        }
                    }
                }

                // Make sure the shipment is marked as configured and activated
                ShippingSettings.MarkAsActivated(ShipmentTypeCode.FedEx);
                ShippingSettings.MarkAsConfigured(ShipmentTypeCode.FedEx);
            }
        }

        /// <summary>
        /// Changing if they accept the license agreement
        /// </summary>
        private void OnChangeAcceptAgreement(object sender, EventArgs e)
        {
            NextEnabled = radioAcceptAgreement.Checked;
        }

        /// <summary>
        /// Stepping into the license page
        /// </summary>
        private void OnSteppingIntoLicense(object sender, WizardSteppingIntoEventArgs e)
        {
            radioDeclineAgreement.Checked = !radioAcceptAgreement.Checked;
            NextEnabled = radioAcceptAgreement.Checked;
        }

        /// <summary>
        /// Begin the printing process
        /// </summary>
        private void OnPrintAgreement(object sender, EventArgs e)
        {
            PrintUtility.PrintText(this, "ShipWorks - FedEx License Agreement", licenseAgreement.Text, true);
        }
    }
}
