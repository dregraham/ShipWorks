﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.UI.Controls;
using ShipWorks.UI.Wizard;
using System.Drawing;

namespace ShipWorks.Shipping.Carriers.BestRate.Setup
{
    /// <summary>
    /// Wizard for creating a shipping account when a counter rate has been selected as
    /// the cheapest/best rate.
    /// </summary>
    public partial class CounterRateProcessingSetupWizard : WizardForm
    {
        private readonly ShipmentType initialShipmentType;
        private readonly RateResult initialRate;

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterRateProcessingSetupWizard" /> class.
        /// </summary>
        /// <param name="filteredRates">The filtered rates.</param>
        /// <param name="selectedRate">The selected rate that is going to be in the "Sign up" section of the wizard.</param>
        /// <param name="shipmentsInBatch">The shipments in the batch.</param>
        public CounterRateProcessingSetupWizard(RateGroup filteredRates, RateResult selectedRate, IEnumerable<ShipmentEntity> shipmentsInBatch)
        {
            InitializeComponent();

            FilteredRates = filteredRates;

            // Assume the user will opt to use an existing account, so we only have to 
            // change the value in in one spot (when signing up for an account)
            IgnoreAllCounterRates = true;

            // Make note of this otherwise we'd be running Rates.First() a number of times
            initialRate = selectedRate ?? FilteredRates.Rates.FirstOrDefault();
            initialShipmentType = DetermineCounterRateShipmentTypeForCounterRateSetupWizard(initialRate.ShipmentType);

            // Swap out the "tokenized" provider text with that of the provider of the initial rate
            LoadInitialRateInfo();
            LoadExistingAccountRateInfo();

            if (shipmentsInBatch.Count() <= 1)
            {
                // There's only one shipment, so it doesn't make sense to show the UI 
                // regarding remaining shipments in the batch
                useExistingAccountsForRemainingLabel.Visible = false;
            }

            NextEnabled = false;
            NextVisible = false;
        }


        /// <summary>
        /// Gets the filtered rates.
        /// </summary>
        public RateGroup FilteredRates
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets or sets the selected shipment type based on the rate that
        /// the user selected.
        /// </summary>
        public ShipmentType SelectedShipmentType
        {
            get; 
            private set; 
        }

        /// <summary>
        /// Gets a value indicating whether the user chose to [ignore all counter rates] for
        /// the batch of shipments being processed.
        /// </summary>
        public bool IgnoreAllCounterRates
        {
            get;
            private set;
        }


        /// <summary>
        /// Loads the rate information for the best rate that was found.
        /// </summary>
        private void LoadInitialRateInfo()
        {
            LoadCreateCarrierDescriptionText();
            
            if (ShipmentTypeManager.IsPostal(initialShipmentType.ShipmentTypeCode))
            {
                // Use the general USPS logo for all postal types 
                bestRateAccountCarrierLogo.Image = EnumHelper.GetImage(ShipmentTypeCode.PostalWebTools);

                // Some service types already contain USPS in the service description, so only add it if
                // the service description does not already start with USPS
                bestRateCarrierName.Text = string.Format("{0}{1}", initialRate.Description.StartsWith("USPS ", StringComparison.OrdinalIgnoreCase) ? string.Empty : "USPS ", initialRate.Description);
            
            }
            else
            {
                // Use the actual logo and shipment type description for non-postal providers
                bestRateAccountCarrierLogo.Image = EnumHelper.GetImage(initialShipmentType.ShipmentTypeCode);
                // "Unmask" the description of non-competitive rates to be consistent with the logo
                NoncompetitiveRateResult noncompetitiveRate = initialRate as NoncompetitiveRateResult;
                bestRateCarrierName.Text = noncompetitiveRate == null ? initialRate.Description : noncompetitiveRate.OriginalRate.Description;
            }

            bestRateAmount.Text = string.Format("{0:C2}", initialRate.Amount);
        }

        /// <summary>
        /// Loads the rate information for the "existing account" panels.
        /// </summary>
        private void LoadExistingAccountRateInfo()
        {
            // We're always only going to show one of the "existing account" panels, so adjust
            // the location to be on top of each other
            useExistingAccountPanel.Top = addExistingAccountPanel.Top;

            RateResult existingAccountRate = FilteredRates.Rates.FirstOrDefault(r => !r.IsCounterRate);

            if (existingAccountRate != null)
            {
                // We have a rate that is not a counter rate, so we know the user has an
                // account setup, so we'll hide the panel for adding an account, adjust 
                // the location of the "use existing" panel, and resize the height of
                // the wizard accordingly
                addExistingAccountPanel.Visible = false;
                useExistingAccountPanel.BringToFront();

                // Populate information based on the rate's shipment type
                ShipmentType existingRateShipmentType = ShipmentTypeManager.GetType(existingAccountRate.ShipmentType);
                useExistingCarrierLogo.Image = EnumHelper.GetImage(existingRateShipmentType.ShipmentTypeCode);

                // "Unmask" the description of non-competitive rates to be consistent with the logo
                NoncompetitiveRateResult noncompetitiveRate = existingAccountRate as NoncompetitiveRateResult;
                useExistingCarrierServiceDescription.Text = noncompetitiveRate == null ? existingAccountRate.Description : noncompetitiveRate.OriginalRate.Description;

                useExistingAccountDescription.Text = useExistingAccountDescription.Text.Replace("{ProviderName}", EnumHelper.GetDescription(existingRateShipmentType.ShipmentTypeCode));
                
                // Show the actual amount and the difference between the best rate and 
                // the cheapest available rate
                existingAccountRateAmount.Text = string.Format("{0:C2}", existingAccountRate.Amount);

                decimal difference = existingAccountRate.Amount - initialRate.Amount;
                existingAccountRateDifference.Text = string.Format("({0:C2} {1})", difference, difference > 0 ? "more" : "less");
                existingAccountRateDifference.ForeColor = difference > 0 ? Color.Red : Color.Green;
                existingAccountRateDifference.Left = existingAccountRateAmount.Right - 3;

                Height = useExistingAccountPanel.Bottom + 185;
                Width = useExistingAccountPanel.Width > createCarrierAccountDescription.Width ?
                    useExistingAccountPanel.Width + 75
                    : createCarrierAccountDescription.Width + 75;
            }
            else
            {
                // There are only counter rates available, so we want to hide the panel that allows
                // the user to select a rate from an existing account
                useExistingAccountPanel.Visible = false;
                addExistingAccountPanel.BringToFront();
                Height = addExistingAccountPanel.Bottom + 200;
                Width = createCarrierAccountDescription.Width + 150;

                LoadShippingProviders();
            }
        }

        /// <summary>
        /// Loads the shipping providers into the setup existing provider combo box.
        /// </summary>
        private void LoadShippingProviders()
        {
            // Further restrict the shipment types so these are not included
            // in the list to setup an account for
            List<ShipmentTypeCode> excludedTypes = new List<ShipmentTypeCode>
            {
                ShipmentTypeCode.BestRate,
                ShipmentTypeCode.None,
                ShipmentTypeCode.Other,
                ShipmentTypeCode.PostalWebTools
            };

            setupExistingProvider.Items.Add("Choose...");

            foreach (ShipmentTypeCode typeCode in ShipmentTypeManager.EnabledShipmentTypes.Select(s => s.ShipmentTypeCode).Except(excludedTypes))
            {
                setupExistingProvider.Items.Add(new ImageComboBoxItem(EnumHelper.GetDescription(typeCode), typeCode, EnumHelper.GetImage(typeCode)));
            }

            setupExistingProvider.SelectedIndex = 0;
        }

        /// <summary>
        /// Loads the text for describing the carrier that has the best rate.
        /// </summary>
        private void LoadCreateCarrierDescriptionText()
        {
            string description = string.Empty;

            switch (initialShipmentType.ShipmentTypeCode)
            {
                case ShipmentTypeCode.Endicia:
                    description = "USPS partners with Endicia to enable printing USPS shipping labels directly from your printer. To continue, you’ll need " + 
                                  "an account with Endicia.";
                    break;

                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Stamps:
                    description = "USPS partners with Express1 to enable printing USPS shipping labels directly from your printer. To continue you'll need an " + 
                                  "account with Express1. There is no monthly fee for the account.";
                    break;
                    
                case ShipmentTypeCode.Stamps:
                    description = "USPS partners with Stamps.com to enable printing USPS shipping labels directly from your printer. To continue, you’ll need " +
                                  "an account with Stamps.com.";
                    break;

                case ShipmentTypeCode.FedEx:
                    description = "To continue, you’ll need an account with FedEx. " + 
                                  "There is no monthly fee for the account.";
                    break;

                case ShipmentTypeCode.UpsOnLineTools:
                    description = "To continue, you’ll need an account with UPS. " +
                                  "There is no monthly fee for the account.";
                    break;
            }

            createCarrierAccountDescription.Text = description;
        }

        /// <summary>
        /// Called when the sign up button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnSignUp(object sender, System.EventArgs e)
        {
            IgnoreAllCounterRates = false;

            // Launch the setup wizard of the selected shipment type
            DialogResult result = ShipmentTypeSetupWizardForm.RunFromHostWizard(this, initialShipmentType);

            if (result == DialogResult.OK)
            {
                SelectedShipmentType = ShipmentTypeManager.GetType(initialShipmentType.ShipmentTypeCode);
                MarkSelectedShipmentTypeAsBeingUsed();
            }
            
            // Close the dialog regardless of whether they canceled out of the setup wizard
            DialogResult = result;
            Close();
        }

        /// <summary>
        /// Called when the value in the setup existing provider drop down list has changed.
        /// </summary>
        private void OnSetupExistingProviderChanged(object sender, System.EventArgs e)
        {
            // Disable the "Add my account" button if a provider is not selected
            ImageComboBoxItem selectedItem = setupExistingProvider.SelectedItem as ImageComboBoxItem;
            setupExistingAccountButton.Enabled = selectedItem != null;
        }

        /// <summary>
        /// Called when the "Add my account" button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnAddExistingAccount(object sender, System.EventArgs e)
        {
            ImageComboBoxItem selectedItem = setupExistingProvider.SelectedItem as ImageComboBoxItem;

            if (selectedItem == null)
            {
                MessageHelper.ShowError(this, "Please select a shipping provider.");
            }
            else
            {
                SelectedShipmentType = ShipmentTypeManager.GetType((ShipmentTypeCode)selectedItem.Value);
                IgnoreAllCounterRates = true;

                // Launch the setup wizard of the selected shipment type
                DialogResult result = ShipmentTypeSetupWizardForm.RunFromHostWizard(this, SelectedShipmentType);
                
                if (result == DialogResult.OK)
                {   
                    MarkSelectedShipmentTypeAsBeingUsed();

                    DialogResult = result;
                    Close();
                }
                else
                {
                    // Give the user a chance to cancel out of the wizard if they accidentally 
                    // chose the wrong provider type and canceled out of the wizard
                    Show(this.Owner);
                }
            }
        }

        /// <summary>
        /// Helper method to designate the selected shipment as being configured and 
        /// included in the global shipping settings
        /// </summary>
        private void MarkSelectedShipmentTypeAsBeingUsed()
        {
            // Mark the shipment as being configured now that an account has been added
            ShippingSettings.MarkAsConfigured(SelectedShipmentType.ShipmentTypeCode);

            // We also want to ensure sure that the provider is no longer excluded in
            // the global settings
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            settings.ExcludedTypes = settings.ExcludedTypes.Where(shipmentType => shipmentType != (int)SelectedShipmentType.ShipmentTypeCode).ToArray();

            ShippingSettings.Save(settings);
        }

        /// <summary>
        ///  Called when the "Use my existing account" button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnUseExistingAccount(object sender, System.EventArgs e)
        {
            // Choosing to ignore counter rates for the rest of the batch
            IgnoreAllCounterRates = true; 
            
            // Grab the shipment type of the first non-counter rate
            RateResult existingAccountRate = FilteredRates.Rates.First(r => !r.IsCounterRate);
            SelectedShipmentType = ShipmentTypeManager.GetType(existingAccountRate.ShipmentType);
            
            // Make sure to close the form with OK as the result
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// For a given shipment type, determines which shipment type should be used for the setup wizard.
        /// </summary>
        private static ShipmentType DetermineCounterRateShipmentTypeForCounterRateSetupWizard(ShipmentTypeCode shipmentTypeCode)
        {
            ShipmentType setupShipmentType;

            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    setupShipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.UpsOnLineTools);
                    break;
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.Stamps:
                case ShipmentTypeCode.PostalWebTools:
                    setupShipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.Endicia);
                    break;
                case ShipmentTypeCode.Express1Endicia:
                    setupShipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.Express1Endicia);
                    break;
                case ShipmentTypeCode.Express1Stamps:
                    setupShipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.Express1Stamps);
                    break;
                case ShipmentTypeCode.FedEx:
                    setupShipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.FedEx);
                    break;
                default:
                    throw new InvalidOperationException("The requested shipment type is not a valid counter rate shipment type.");
            }

            return setupShipmentType;
        }
    }
}
