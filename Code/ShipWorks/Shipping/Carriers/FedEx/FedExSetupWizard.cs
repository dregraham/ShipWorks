﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.Shipping.ShipEngine.DTOs.CarrierAccount;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Wizard for registering to use FedEx
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypeSetupWizard), ShipmentTypeCode.FedEx)]
    public partial class FedExSetupWizard : WizardForm, IShipmentTypeSetupWizard
    {
        FedExAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExSetupWizard()
        {
            InitializeComponent();

            account = new FedExAccountEntity();
        }

        /// <summary>

        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.FedEx);
            account.CountryCode = "US";
            account.SmartPostHubList = "<Root />";
            account.InitializeNullsToDefault();

            personControl.LoadEntity(new PersonAdapter(account, ""));
            accountSettingsControl.LoadAccount(account);

            accountNumber.Text = account.AccountNumber;

            // If its not already setup or configured, load all the settings\configuration pages
            if (!ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.FedEx))
            {
                Pages.Add(new ShippingWizardPageOrigin(shipmentType));
                Pages.Add(new ShippingWizardPageDefaults(shipmentType));
                Pages.Add(new ShippingWizardPagePrinting(shipmentType));
                Pages.Add(new ShippingWizardPageAutomation(shipmentType));

                optionsControl.LoadSettings();
            }
            // Otherwise it will just be to setup the account
            else
            {
                optionsControl.Visible = false;
                accountSettingsControl.Top = optionsControl.Top;
            }

            Pages.Add(new ShippingWizardPageFinish(shipmentType));
            Pages[Pages.Count - 1].SteppingInto += OnSteppingIntoFinish;

            licenseAgreement.Rtf = ResourceUtility.ReadString("ShipWorks.Shipping.Carriers.FedEx.FedExEULA.rtf");

            // The RichTextBox doesn't have padding, and margin doesn't seem to push it over, so this does...
            licenseAgreement.SelectAll();
            licenseAgreement.SelectionIndent += 3;
            licenseAgreement.DeselectAll();
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
        private async void OnStepNextAccountInfo(object sender, WizardStepEventArgs e)
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

            if (!string.IsNullOrWhiteSpace(account.CountryCode) &&
                (account.CountryCode == "US" || account.CountryCode == "CA"))
            {
                checker.Check("State", account.StateProvCode);
            }

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

                await RegisterAccount(account);

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
        /// Registers a FedEx account for use with the FedEx API.
        /// </summary>
        /// <param name="fedExAccount">The account.</param>
        private async Task RegisterAccount(FedExAccountEntity fedExAccount)
        {
            using (var lifetimeScope = IoC.BeginLifetimeScope())
            {
                var webClient = lifetimeScope.Resolve<IShipEngineWebClient>();

                // Response contains the ShipEngine carrier id
                var response = await webClient.ConnectFedExAccount(new FedExRegistrationRequest
                {
                    Nickname = fedExAccount.Email,
                    AccountNumber = fedExAccount.AccountNumber,
                    Company = fedExAccount.Company,
                    FirstName = fedExAccount.FirstName,
                    LastName = fedExAccount.LastName,
                    Phone = fedExAccount.Phone,
                    Address1 = fedExAccount.Street1,
                    Address2 = fedExAccount.Street2,
                    City = fedExAccount.City,
                    State = fedExAccount.StateProvCode,
                    PostalCode = fedExAccount.PostalCode,
                    CountryCode = fedExAccount.CountryCode,
                    Email = fedExAccount.Email,
                    AgreeToEula = "true"
                });

                if (response.Success)
                {
                    fedExAccount.ShipEngineCarrierID = response.Value;
                }
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
                if (!account.IsNew)
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
                    foreach (ShippingProfileEntity shippingProfileEntity in ShippingProfileManager.Profiles.Where(p => p.ShipmentType == ShipmentTypeCode.FedEx))
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

        /// <summary>
        /// Gets the wizard without any wrapping wizards
        /// </summary>
        public IShipmentTypeSetupWizard GetUnwrappedWizard() => this;
    }
}
