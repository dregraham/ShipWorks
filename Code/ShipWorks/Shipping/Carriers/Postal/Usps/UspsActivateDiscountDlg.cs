using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Defaults;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public partial class UspsActivateDiscountDlg : Form
    {
        private readonly ShippingSettingsEntity settings;
        private readonly ShipmentEntity shipment;

        private bool requiresSignup = true;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsActivateDiscountDlg"/> class.
        /// </summary>
        public UspsActivateDiscountDlg(ShipmentEntity shipment)
        {
            settings = ShippingSettings.Fetch();
            this.shipment = shipment;

            InitializeComponent();
        }
        
        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (shipment.ShipmentType == (int) ShipmentTypeCode.Stamps && StampsAccountManager.StampsAccounts.Any())
            {
                // There are Stamps accounts, so we want to show the control to convert their existing account
                requiresSignup = false;
                signUpForExpeditedControl.Visible = false;
                convertToExpeditedControl.Visible = true;

                convertToExpeditedControl.Top = signUpForExpeditedControl.Top;
                Height = convertToExpeditedControl.Bottom + 60;

                convertToExpeditedControl.AccountConverted += OnAccountConverted;
                convertToExpeditedControl.AccountConverting += OnAccountConverting;
                
                StampsAccountEntity accountToConvert = StampsAccountManager.GetAccount(shipment.Postal.Stamps.StampsAccountID);
                convertToExpeditedControl.Initialize(accountToConvert);
            }
            else
            {
                // Prompt the user to sign up/choose an existing account if there aren't any 
                // Stamps.com accounts or they are the shipment type isn't Stamps.com
                requiresSignup = true;
                signUpForExpeditedControl.Visible = true;
                convertToExpeditedControl.Visible = false;

                Height = signUpForExpeditedControl.Bottom + 60;

                signUpForExpeditedControl.LoadSettings(settings);
            }
        }

        /// <summary>
        /// Called when an account is being converted.
        /// </summary>
        private void OnAccountConverting(object sender, EventArgs eventArgs)
        {
            // Just want to update the cursor here
            Cursor.Current = Cursors.WaitCursor;
        }

        /// <summary>
        /// Called when an account has been converted.
        /// </summary>
        private void OnAccountConverted(object sender, UspsAccountConvertedEventArgs eventArgs)
        {
            // Clear the cache since rates are now outdated
            RateCache.Instance.Clear();

            Cursor.Current = Cursors.Default;
            MessageHelper.ShowInformation(this, "Your account has been converted to take advantage of postage discounts.");

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Closing the window
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (requiresSignup && signUpForExpeditedControl.UseExpedited)
            {
                // Make sure the settings are valid before trying to save them
                if (settings.StampsAutomaticExpedited && settings.StampsAutomaticExpeditedAccount <= 0)
                {
                    MessageHelper.ShowMessage(this, "Please select or create a USPS account.");
                    e.Cancel = true;
                    return;
                }

                // Only way we should require a signup is not already using a Stamps.com account for 
                // this shipment, so we need to change the shipment type to USPS (Stamps.com Expedited) 
                // in order to take advantage of the new rates (since Stamps.com API doesn't match 
                // with Endicia API and shipment configurations differ).
                shipment.ShipmentType = (int) ShipmentTypeCode.Usps;
                ShippingManager.SaveShipment(shipment);

                // We also need to exclude Endicia and Express1 from the list of active providers since
                // the customer agreed to use USPS (Stamps.com Expedited)
                ExcludeShipmentType(ShipmentTypeCode.Endicia);
                ExcludeShipmentType(ShipmentTypeCode.Express1Endicia);
                ExcludeShipmentType(ShipmentTypeCode.Express1Stamps);
                
                ShippingSettings.Save(settings);

                // Need to update any rules to swap out Endicia and Express1 with USPS (Stamps.com Expedited)
                // now that those types are not longer active
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Endicia);
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Express1Endicia);
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Express1Stamps);

                RateCache.Instance.Clear();

                DialogResult = settings.StampsAutomaticExpedited ? DialogResult.OK : DialogResult.Cancel;
            }
        }

        /// <summary>
        /// Excludes the given shipment type from the list of active shipping providers.
        /// </summary>
        /// <param name="shipmentTypeCode">The shipment type code to be excluded.</param>
        private void ExcludeShipmentType(ShipmentTypeCode shipmentTypeCode)
        {
            if (!settings.ExcludedTypes.Any(t => t == (int) shipmentTypeCode))
            {
                List<int> excludedTypes = settings.ExcludedTypes.ToList();
                excludedTypes.Add((int)shipmentTypeCode);

                settings.ExcludedTypes = excludedTypes.ToArray();
            }
        }

        /// <summary>
        /// Uses the USPS (Stamps.com Expedited) as the shipping provider for any rules using the given shipment type code.
        /// </summary>
        /// <param name="shipmentTypeCode">The shipment type code to be replaced with USPS (Stamps.com Expedited) .</param>
        private void UseUspsInDefaultShippingRulesFor(ShipmentTypeCode shipmentTypeCode)
        {
            List<ShippingDefaultsRuleEntity> rules = ShippingDefaultsRuleManager.GetRules(shipmentTypeCode);
            foreach (ShippingDefaultsRuleEntity rule in rules)
            {
                rule.ShipmentType = (int) ShipmentTypeCode.Usps;
                ShippingDefaultsRuleManager.SaveRule(rule);
            }
        }
    }
}
