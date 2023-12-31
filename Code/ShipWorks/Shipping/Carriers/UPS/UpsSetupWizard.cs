﻿using System;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration;
using ShipWorks.Shipping.Carriers.UPS.OneBalance;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Wizard for setting up UPS OLT for the first time
    /// </summary>
    [KeyedComponent(typeof(IOneBalanceSetupWizard), ShipmentTypeCode.UpsOnLineTools)]
    [KeyedComponent(typeof(IShipmentTypeSetupWizard), ShipmentTypeCode.UpsOnLineTools)]
    public partial class UpsSetupWizard : WizardForm, IShipmentTypeSetupWizard, IOneBalanceSetupWizard
    {
        private readonly ShipmentType shipmentType;
        private readonly bool forceAccountOnly;

        private string upsLicense;

        // The ups shipper we are creating
        private readonly UpsAccountEntity upsAccount = new UpsAccountEntity();

        private OneBalanceTermsAndConditionsPage oneBalanceTandCPage = new OneBalanceTermsAndConditionsPage();
        private OneBalanceAccountAddressPage oneBalanceAddressPage;
        private OneBalanceFinishPage oneBalanceFinishPage = new OneBalanceFinishPage();
        private bool newAccountOnly;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsSetupWizard(IShipmentTypeManager shipmentTypeManager, Func<UpsAccountEntity, OneBalanceAccountAddressPage> oneBalanceAddressPageFactory) :
            this(ShipmentTypeCode.UpsOnLineTools, false, shipmentTypeManager, oneBalanceAddressPageFactory)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsSetupWizard(ShipmentTypeCode shipmentTypeCode, bool forceAccountOnly, IShipmentTypeManager shipmentTypeManager,
                              Func<UpsAccountEntity, OneBalanceAccountAddressPage> oneBalanceAddressPageFactory)
        {
            InitializeComponent();

            if (shipmentTypeCode != ShipmentTypeCode.UpsWorldShip && shipmentTypeCode != ShipmentTypeCode.UpsOnLineTools)
            {
                throw new InvalidOperationException("ShipmentTypeCode must be UPS");
            }

            shipmentType = shipmentTypeManager.Get(shipmentTypeCode);
            this.forceAccountOnly = forceAccountOnly;

            oneBalanceAddressPage = oneBalanceAddressPageFactory(upsAccount);
            oneBalanceTandCPage.SteppingInto += OnSteppingIntoOneBalanceTermsAndConditions;
        }

        /// <summary>
        /// Setup UPS Account. 
        /// </summary>
        public DialogResult SetupOneBalanceAccount(IWin32Window owner)
        {
            // Always set up a new account when using One Balance
            newAccountOnly = true;
            return ShowDialog(owner);
        }

        /// <summary>
        /// Now that we have two separate selections, new or existing account, we have to reset to the original list of pages.
        /// Otherwise, if the user clicks next, pages for the currently selection option would be set, but if they click back
        /// and choose the other option, it's pages wouldn't be in the collection.
        /// </summary>
        [NDependIgnoreLongMethod]
        private void ResetWizardPagesCollection()
        {
            Pages.Clear();

            Pages.AddRange(new[] {
                wizardPageWelcomeOlt,
                wizardPageWelcomeWorldShip,
                oneBalanceTandCPage,
                oneBalanceAddressPage,
                wizardPageAccountList,
                wizardPageLicense,
                wizardPageAccount,
                wizardPageInvoiceAuthentication,
                wizardPageRates,
                wizardPageOptionsOlt,
                wizardPageOptionsWorldShip,
                wizardPageFinishOlt,
                wizardPageFinishAddAccount,
                oneBalanceFinishPage
            });

            bool addAccountOnly = ShippingManager.IsShipmentTypeConfigured(shipmentType.ShipmentTypeCode) || forceAccountOnly;

            // Prepare the correct Welcome page
            if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.UpsOnLineTools)
            {
                Pages.Remove(wizardPageWelcomeWorldShip);

                if (newAccountOnly)
                {
                    UpdateLogo(UpsLogoType.UpsFromShipWorks);
                    Pages.Remove(wizardPageWelcomeOlt);
                    Pages.Remove(wizardPageLicense);
                }
            }
            else
            {
                Pages.Remove(wizardPageWelcomeOlt);
            }

            if (!newAccountOnly)
            {
                Pages.Remove(oneBalanceTandCPage);
                Pages.Remove(oneBalanceAddressPage);
                Pages.Remove(oneBalanceFinishPage);
            }
            else
            {
                Pages.Remove(wizardPageLicense);
                Pages.Remove(wizardPageRates);
                Pages.Remove(wizardPageInvoiceAuthentication);
                Pages.Remove(wizardPageAccount);
                // Only way to create new account is through One Balance, so remove the other finish pages so that
                // the One Balance finish pages shows.
                Pages.Remove(wizardPageFinishOlt);
                Pages.Remove(wizardPageFinishAddAccount);
            }

            // Sets initial values and resets existing values depending on when this is called.
            // A new entry is needed here when introducing a not null field to the UpsAccount table.
            upsAccount.CountryCode = "US";
            upsAccount.InvoiceAuth = false;
            upsAccount.RateType = (int) UpsRateType.DailyPickup;
            upsAccount.InitializeNullsToDefault();

            // UPS Promo no longer supported.
            upsAccount.PromoStatus = 0;
            upsAccount.LocalRatingEnabled = false;

            personControl.LoadEntity(new PersonAdapter(upsAccount, ""));

            Pages.Remove(wizardPageAccountList);

            // it will just be to setup to add a new account
            if (addAccountOnly)
            {
                // And remove the settings pages
                Pages.Remove(wizardPageOptionsOlt);
                Pages.Remove(wizardPageOptionsWorldShip);
            }
            // Otherwise setup to configure settings completely
            else
            {
                // Add in the correct options page
                if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.UpsOnLineTools)
                {
                    optionsControlOlt.LoadSettings();
                    Pages.Remove(wizardPageOptionsWorldShip);
                }
                else
                {
                    optionsControlWorldShip.LoadSettings();
                    Pages.Remove(wizardPageOptionsOlt);
                }
            }

            Pages.Add(new ShippingWizardPageOrigin(shipmentType));
            Pages.Add(new ShippingWizardPageDefaults(shipmentType));

            ShippingWizardPageAutomation automationPage = new ShippingWizardPageAutomation(shipmentType);

            // WorldShip doesn't need printing
            if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.UpsOnLineTools)
            {
                Pages.Add(new ShippingWizardPagePrinting(shipmentType));
            }

            Pages.Add(automationPage);
            Pages.Remove(wizardPageFinishAddAccount);

            // Insure finish is last
            if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.UpsOnLineTools)
            {
                if (!newAccountOnly)
                {
                    Pages.Remove(wizardPageFinishOlt);
                    Pages.Add(wizardPageFinishOlt);
                }
                else
                {
                    Pages.Remove(oneBalanceFinishPage);
                    Pages.Add(oneBalanceFinishPage);
                }
            }
            else
            {
                Pages.Remove(wizardPageFinishOlt);

                if (addAccountOnly)
                {
                    Pages.Add(wizardPageFinishAddAccount);
                }
                else
                {
                    Pages.Add(new ShippingWizardPageFinish(shipmentType));
                }
            }

            if (Pages.Contains(wizardPageAccountList))
            {
                accountControl.Initialize(shipmentType.ShipmentTypeCode);
            }

            // Listen for finish
            Pages[Pages.Count - 1].SteppingInto += OnSteppingIntoFinish;

            // Add in the first page
            SetCurrent(0);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            ResetWizardPagesCollection();
        }

        /// <summary>
        /// Stepping next from the welcome page
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnStepNextWelcome(object sender, WizardStepEventArgs e)
        {
            string accountNumber = EnteredAccountNumber();
            if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip)
            {
                if (!worldShipAgree1.Checked || !worldShipAgree2.Checked)
                {
                    MessageHelper.ShowInformation(this, "You must read and agree to the WorldShip information statements.");

                    e.NextPage = CurrentPage;
                    return;
                }
            }

            // Validate the entered account number
            if (string.IsNullOrWhiteSpace(accountNumber) && !newAccountOnly)
            {
                // Note: this will need to be refactored when we unhide the ability to create
                // a new UPS account from ShipWorks
                MessageHelper.ShowMessage(this, "Please enter your account number.");
                e.NextPage = CurrentPage;
                return;
            }

            // Start with a fresh page collection
            ResetWizardPagesCollection();

            // If this is for an existing account, remove the open account and one balance wizard pages.
            if (!newAccountOnly)
            {
                e.NextPage = wizardPageLicense;
            }
            else
            {
                e.NextPage = oneBalanceTandCPage;
            }

            // If the account list page is present, that means we arent creating accounts from this wizard flow directly
            if (Pages.Contains(wizardPageAccountList))
            {
                return;
            }

            // We already got the license, don't need to do it again
            if (upsLicense != null)
            {
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // Extract the license text and return it to be passed on to the next stage of the wizard
                upsLicense = UpsUtility.GetAccessLicenseText();
            }
            catch (UpsException ex)
            {
                MessageBox.Show(this,
                    "An error occurred: " + ex.Message,
                    "ShipWorks",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Stepping into the agreement page
        /// </summary>
        private void OnSteppingIntoAgreement(object sender, WizardSteppingIntoEventArgs e)
        {
            if (e.StepReason != WizardStepReason.StepBack)
            {
                licenseAgreement.Text = upsLicense;

                // The RichTextBox doesn't have padding, and margin doesn't seem to push it over, so this does...
                licenseAgreement.SelectAll();
                licenseAgreement.SelectionIndent += 3;
                licenseAgreement.DeselectAll();

                radioDeclineAgreement.Checked = true;
                NextEnabled = false;
            }
        }

        /// <summary>
        /// Stepping next from the license agreement page
        /// </summary>
        private void OnSteppingNextAgreement(object sender, WizardStepEventArgs e)
        {
            // Code below was for certification purposes

            //
            // This is a UPS requirement - but it causes annoyance and problems for our customers. They probably will never notice.
            //
            //NativeMethods.POINT point = new NativeMethods.POINT();
            //NativeMethods.SendMessage(licenseAgreement.Handle, NativeMethods.EM_GETSCROLLPOS, IntPtr.Zero, ref point);

            //if (point.y <= 7550)
            //{
            //    MessageHelper.ShowInformation(this, "UPS requires that we make you scroll to the bottom of the agreement before moving on.");
            //    e.NextPage = CurrentPage;
            //}
        }

        /// <summary>
        /// Changing if they accept the license agreement
        /// </summary>
        private void OnChangeAcceptAgreement(object sender, EventArgs e)
        {
            NextEnabled = radioAcceptAgreement.Checked;
        }

        /// <summary>
        /// Begin the printing process
        /// </summary>
        private void OnPrintAgreement(object sender, EventArgs e)
        {
            PrintUtility.PrintText(this, "ShipWorks - UPS License Agreement", upsLicense, true);
        }

        /// <summary>
        /// Open the link to open a UPS account
        /// </summary>
        private void OnLinkOpenAccount(object sender, EventArgs e)
        {
            string url = "https://www.ups.com/myups/info/openacct";

            PersonAdapter personAdapter = new PersonAdapter();
            personControl.SaveToEntity(personAdapter);

            if (personAdapter.CountryCode == "CA")
            {
                url = "https://www.ups.com/one-to-one/login?loc=en_CA";
            }
            else if (personAdapter.CountryCode == "PR")
            {
                url = "https://www.ups.com/one-to-one/login?loc=en_PR";
            }

            WebHelper.OpenUrl(url, this);
        }

        /// <summary>
        /// Gets the account number the user entered
        /// </summary>
        /// <returns></returns>
        private string EnteredAccountNumber()
        {
            if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip)
            {
                // UPS wants account numbers to be upper case, so do it.
                return wsUpsAccountNumber.Text.Trim().ToUpperInvariant();
            }

            // UPS wants account numbers to be upper case, so do it.
            return account.Text.Trim().ToUpperInvariant();
        }

        /// <summary>
        /// Checks to see if the given account number is allowed based on the edition of ShipWorks
        /// </summary>
        private bool AccountAllowed(string upsAccountNumber)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                bool accountAllowed = lifetimeScope.Resolve<ILicenseService>()
                        .HandleRestriction(EditionFeature.UpsAccountNumbers, upsAccountNumber, this);

                return accountAllowed;
            }
        }

        /// <summary>
        /// Stepping next from the account page
        /// </summary>
        private void OnStepNextAccount(object sender, WizardStepEventArgs e)
        {
            personControl.SaveToEntity(new PersonAdapter(upsAccount, string.Empty));
            upsAccount.AccountNumber = EnteredAccountNumber();

            // Edition check
            if (!AccountAllowed(upsAccount.AccountNumber) || !ValidateEnteredAccountInformation())
            {
                e.NextPage = CurrentPage;
                return;
            }

            try
            {
                UpsRegistrationStatus registrationStatus;

                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    IUpsClerk clerk = lifetimeScope.Resolve<IUpsClerk>(new TypedParameter(typeof(UpsAccountEntity), upsAccount));
                    registrationStatus = clerk.RegisterAccount(upsAccount);
                }

                switch (registrationStatus)
                {
                    case UpsRegistrationStatus.Success:
                        // Force invoice auth to be false at this point because
                        // registration was successful without the need for
                        // invoice auth
                        upsAccount.InvoiceAuth = false;
                        e.NextPage = wizardPageRates;
                        break;
                    case UpsRegistrationStatus.Failed:
                        e.NextPage = CurrentPage;
                        break;
                    case UpsRegistrationStatus.InvoiceAuthenticationRequired:
                        e.NextPage = wizardPageInvoiceAuthentication;
                        break;
                }

                GetUpsAccessKey();

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    upsAccount.Description = UpsAccountManager.GetDefaultDescription(upsAccount);
                    UpsAccountManager.SaveAccount(upsAccount);

                    adapter.Commit();
                }
            }
            catch (UpsException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Validates the entered account information.
        /// </summary>
        private bool ValidateEnteredAccountInformation()
        {
            RequiredFieldChecker checker = new RequiredFieldChecker();
            checker.Check("UPS Account", upsAccount.AccountNumber);
            checker.Check("Full Name", upsAccount.FirstName);
            checker.Check("Company", upsAccount.Company);
            checker.Check("Street Address", upsAccount.Street1);
            checker.Check("City", upsAccount.City);
            checker.Check("State", upsAccount.StateProvCode);
            checker.Check("Postal Code", upsAccount.PostalCode);
            checker.Check("Phone", upsAccount.Phone);
            checker.Check("Website", upsAccount.Website);
            checker.Check("Email", upsAccount.Email);

            return checker.Validate(this);
        }

        /// <summary>
        /// Step into Invoice Authentication page
        /// </summary>

        private void OnStepIntoInvoiceAuthentication(object sender, WizardSteppingIntoEventArgs e)
        {
            string days = "45";
            if (upsAccount.CountryCode == "US" || upsAccount.CountryCode == "CA")
            {
                days = "90";
            }

            invoiceAuthenticationInstructions.Text = @"You must validate your account by providing information from " +
                                                     $"a valid invoice. {Environment.NewLine}{Environment.NewLine}" +
                                                     $"You must use any of the last 3 invoices issued within the past {days} days.";
        }

        /// <summary>
        /// Step next from invoice authentication page
        /// </summary>
        private void OnStepNextInvoiceAuthentication(object sender, WizardStepEventArgs e)
        {
            try
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    IUpsClerk clerk = lifetimeScope.Resolve<IUpsClerk>(new TypedParameter(typeof(UpsAccountEntity), upsAccount));
                    UpsRegistrationStatus result = clerk.RegisterAccount(upsAccount, upsInvoiceAuthorizationControl.InvoiceAuthorizationData);

                    if (result != UpsRegistrationStatus.Success)
                    {
                        HandleInvoiceAuthError("Error attempting to perform invoice authentication, please check the info provided and try again. ", e);
                    }
                }
            }
            catch (UpsWebServiceException ex)
            {
                HandleInvoiceAuthError(ex.Message, e);
            }
        }

        /// <summary>
        /// Show the user an error when invoice auth fails and stay on the same page
        /// </summary>
        private void HandleInvoiceAuthError(string message, WizardStepEventArgs e)
        {
            string errorMessage = message + Environment.NewLine + Environment.NewLine +
                                  "Note: UPS will lock out accounts for a 24 hour period if your invoice information cannot be " +
                                  "authenticated after three attempts.";
            MessageHelper.ShowError(this, errorMessage);
            e.NextPage = CurrentPage;
        }

        /// <summary>
        /// Set UpsAccessKey in ShippingSettings - Used for UPS Authentication
        /// </summary>
        /// <exception cref="UpsException"></exception>
        /// <remarks>
        /// If UpsAccessKey is already set in ShippingSettings return
        /// If UpsAccessKey is not set, it is retrieved from UPS and set in ShippingSettings
        /// </remarks>
        private void GetUpsAccessKey()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            if (!string.IsNullOrEmpty(settings.UpsAccessKey))
            {
                // Already been set
                return;
            }

            UpsUtility.FetchAndSaveUpsAccessKey(upsAccount, upsLicense);
        }

        /// <summary>
        /// Stepping next from the options page
        /// </summary>
        private void OnStepNextOptionsOlt(object sender, WizardStepEventArgs e)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            optionsControlOlt.SaveSettings();

            ShippingSettings.Save(settings);
        }

        /// <summary>
        /// Stepping into the options page
        /// </summary>
        private void OnStepIntoOptionsOlt(object sender, WizardSteppingIntoEventArgs e)
        {
            // Disable the back button if we created a One Balance account
            BackEnabled = !newAccountOnly;
        }

        /// <summary>
        /// Stepping next from the WorldShip options page
        /// </summary>
        private void OnStepNextOptionsWorldShip(object sender, WizardStepEventArgs e)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            optionsControlWorldShip.SaveSettings(settings);

            ShippingSettings.Save(settings);
        }

        /// <summary>
        /// Stepping into the finishing page
        /// </summary>
        private void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip)
            {
                WorldShipImportMonitor.Start();
            }

            // If the account list page is present that means we didn't create the account from this wizard
            if (!Pages.Contains(wizardPageAccountList))
            {
                upsAccount.Description = UpsAccountManager.GetDefaultDescription(upsAccount);
                UpsAccountManager.SaveAccount(upsAccount);

                // Mark the new account as configured
                ShippingSettings.MarkAsConfigured(shipmentType.ShipmentTypeCode);

                // If this is the only account, update this UPS shipment type profiles with this account
                if (UpsAccountManager.Accounts.Count == 1)
                {
                    UpsAccountEntity upsAccountEntity = UpsAccountManager.Accounts.First();

                    foreach (ShippingProfileEntity shippingProfileEntity in ShippingProfileManager.Profiles.Where(p => p.ShipmentType == shipmentType.ShipmentTypeCode))
                    {
                        shippingProfileEntity.Ups.UpsAccountID = upsAccountEntity.UpsAccountID;
                        ShippingProfileManager.SaveProfile(shippingProfileEntity);
                    }
                }

                // We created a new UPS account
                if (newAccountOnly)
                {
                    labelSetupComplete1.Text = "Congratulations, you successfully created a UPS account within ShipWorks!";
                    labelSetupComplete2.Text = $"Your new UPS account number: {upsAccount.AccountNumber}";
                    labelSetupComplete3.Text = "Please watch your email for a confirmation from UPS and more information on how to use your account.";
                }
            }
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // If we are canceling - but had created an account - we need to undo that
            if (DialogResult == DialogResult.Cancel && upsAccount != null && !upsAccount.IsNew)
            {
                UpsAccountManager.DeleteAccount(upsAccount);
            }
            else if (DialogResult == DialogResult.OK)
            {
                // We need to clear out the rate cache since rates (especially best rate) are no longer valid now
                // that a new account has been added.
                RateCache.Instance.Clear();
            }
        }

        /// <summary>
        /// Called when [stepping into rates].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="WizardSteppingIntoEventArgs"/> instance containing the event data.</param>
        private void OnSteppingIntoRates(object sender, WizardSteppingIntoEventArgs e)
        {
            upsRateTypeControl.Initialize(upsAccount, newAccountOnly);
        }

        /// <summary>
        /// Called when [step next wizard page rates].
        /// </summary>
        private void OnStepNextWizardPageRates(object sender, WizardStepEventArgs e)
        {
            if (!upsRateTypeControl.RegisterAndSaveToEntity())
            {
                e.NextPage = CurrentPage;
            }
        }

         /// <summary>
        /// When stepping into the welcome page, show the UPS logo
        /// </summary>
        private void OnSteppingIntoWelcome(object sender, WizardSteppingIntoEventArgs e)
        {
            UpdateLogo(UpsLogoType.Ups);
        }

        /// <summary>
        /// When stepping into the One Balance terms and conditions page, show the UPS from ShipWorks logo
        /// </summary>
        private void OnSteppingIntoOneBalanceTermsAndConditions(object sender, WizardSteppingIntoEventArgs e)
        {
            UpdateLogo(UpsLogoType.UpsFromShipWorks);
        }

        /// <summary>
        /// Update which logo is shown in the wizard
        /// </summary>
        private void UpdateLogo(UpsLogoType logo)
        {
            if (logo == UpsLogoType.Ups)
            {
                upsFromShipWorksLogo.Visible = false;
                pictureBox.Visible = true;
            }
            else
            {
                pictureBox.Visible = false;
                upsFromShipWorksLogo.Visible = true;
            }
        }

        /// <summary>
        /// Gets the wizard without any wrapping wizards
        /// </summary>
        public IShipmentTypeSetupWizard GetUnwrappedWizard() => this;
    }
}
