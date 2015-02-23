using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Defaults;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// A dialog for activating the USPS shipment type and creating 
    /// a new account or converting an existing account.
    /// </summary>
    public partial class UspsActivateDiscountDlg : Form
    {
        private ShippingSettingsEntity settings;
        private ShipmentEntity shipment;

        private bool requiresSignup = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsActivateDiscountDlg"/> class.
        /// </summary>
        public UspsActivateDiscountDlg()
        {
            InitializeComponent();

            convertToExpeditedControl.DescriptionText = "You can now save up to 46% on USPS Priority Mail and Priority Mail Express Shipments with ShipWorks " +
                                                        "and IntuiShip, all through one single Stamps.com account. " + Environment.NewLine + Environment.NewLine +
                                                        "There are no additional monthly fees and the service, tracking, and labels are exactly the same. " +
                                                        "The only difference is that you pay less for postage!";

            convertToExpeditedControl.LinkText = "Click here to add these discounted rates from IntuiShip through your existing Stamps.com account at no additional cost.";

            signUpForExpeditedControl.DiscountText = "You can now save up to 46% on USPS Priority Mail and Priority Mail Express Shipments with ShipWorks " +
                                                     "and IntuiShip, all through one single Stamps.com account." + Environment.NewLine + Environment.NewLine +
                                                     "To get these discounts, you just need to open a Stamps.com account which will enable you to easily " +
                                                     "print both USPS Priority Mail and Priority Mail Express labels and First Class shipping labels.";
        }

        /// <summary>
        /// Initializes the the form based on the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public virtual void Initialize(ShipmentEntity shipment)
        {
            this.settings = ShippingSettings.Fetch();
            this.shipment = shipment;

            if (shipment.ShipmentType == (int)ShipmentTypeCode.Usps && UspsAccountManager.UspsAccounts.Any())
            {
                // There are USPS-backed accounts, so we want to show the control to convert their existing account
                requiresSignup = false;
                signUpForExpeditedControl.Visible = false;
                convertToExpeditedControl.Visible = true;

                convertToExpeditedControl.Top = signUpForExpeditedControl.Top;
                Height = convertToExpeditedControl.Bottom + 60;
                close.Top = Height - 60;
                close.Left = Right - close.Width - 22;

                convertToExpeditedControl.AccountConverted += OnAccountConverted;
                convertToExpeditedControl.AccountConverting += OnAccountConverting;
                
                UspsAccountEntity accountToConvert = UspsAccountManager.GetAccount(shipment.Postal.Usps.UspsAccountID);
                convertToExpeditedControl.Initialize(accountToConvert);
            }
            else
            {
                // Prompt the user to sign up/choose an existing account if there aren't any 
                // USPS accounts or they are the shipment type isn't Stamps.com
                requiresSignup = true;
                signUpForExpeditedControl.Visible = true;
                convertToExpeditedControl.Visible = false;

                Height = signUpForExpeditedControl.Bottom + 80;
                close.Top = Height - 70;
                close.Left = signUpForExpeditedControl.Right - close.Width - 22;

                signUpForExpeditedControl.LoadSettings();
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
            ShippingSettings.Save(settings);

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
                if (signUpForExpeditedControl.UseExpedited && signUpForExpeditedControl.ExpeditedAccountID <= 0)
                {
                    MessageHelper.ShowMessage(this, "Please select or create a USPS account.");
                    e.Cancel = true;
                    return;
                }

                ShippingManager.RefreshShipment(shipment);

                // Only way we should require a signup is not already using a USPS account for 
                // this shipment, so we need to change the shipment type to USPS
                // in order to take advantage of the new rates (since USPS API doesn't match 
                // with Endicia API and shipment configurations differ).
                shipment.ShipmentType = (int) ShipmentTypeCode.Usps;
                ShippingManager.SaveShipment(shipment);

                // Now that the shipment has been updated, we need to broadcast that the shipping 
                // settings have been changed, so any listeners have a chance to react
                ShippingSettingsEventDispatcher.DispatchUspsAutomaticExpeditedChanged(this, new ShippingSettingsEventArgs((ShipmentTypeCode)shipment.ShipmentType));
            

                // We also need to exclude Endicia and Express1 from the list of active providers since
                // the customer agreed to use USPS 
                ExcludeShipmentType(ShipmentTypeCode.Endicia);
                ExcludeShipmentType(ShipmentTypeCode.Express1Endicia);
                ExcludeShipmentType(ShipmentTypeCode.Express1Usps);

                // Be sure the USPS shipment type is not included in the excluded list
                List<int> excludedTypes = settings.ExcludedTypes.ToList();
                excludedTypes.Remove((int)ShipmentTypeCode.Usps);
                settings.ExcludedTypes = excludedTypes.ToArray();
                
                ShippingSettings.Save(settings);

                // Need to update any rules to swap out Endicia and Express1 with USPS 
                // now that those types are not longer active
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Endicia);
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Express1Endicia);
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Express1Usps);
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Usps);

                RateCache.Instance.Clear();

                DialogResult = signUpForExpeditedControl.UseExpedited ? DialogResult.OK : DialogResult.Cancel;
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
        /// Uses the USPS as the shipping provider for any rules using the given shipment type code.
        /// </summary>
        /// <param name="shipmentTypeCode">The shipment type code to be replaced with USPS.</param>
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
