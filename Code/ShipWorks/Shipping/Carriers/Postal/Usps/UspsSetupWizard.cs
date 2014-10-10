using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Defaults;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// A setup wizard for the USPS (Stamps.com Expedited) shipment type.
    /// </summary>
    public class UspsSetupWizard : StampsSetupWizard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsSetupWizard"/> class.
        /// </summary>
        /// <param name="promotion">The promotion.</param>
        /// <param name="allowRegisteringExistingAccount">if set to <c>true</c> [allow registering existing account].</param>
        public UspsSetupWizard(IRegistrationPromotion promotion, bool allowRegisteringExistingAccount)
            : base(promotion, allowRegisteringExistingAccount, ShipmentTypeCode.Usps)
        { }

        /// <summary>
        /// Gets or sets the initial account address that to use when adding an account.
        /// </summary>
        public PersonAdapter InitialAccountAddress { get; set; }
        
        /// <summary>
        /// Initialization
        /// </summary>
        protected override void OnLoad(object sender, System.EventArgs e)
        {
            base.OnLoad(sender, e);

            if (InitialAccountAddress != null)
            {
                // Pre-load the person control with our initial account address (in the event an account is being
                // created via the Activate Postage Discount dialog
                PersonControl.LoadEntity(InitialAccountAddress);
            }

        }
        /// <summary>
        /// Prepares the stamps account for save. Just sets the reseller type to expedited.
        /// </summary>
        protected override void PrepareStampsAccountForSave()
        {
            base.PrepareStampsAccountForSave();
            StampsAccount.StampsReseller = (int) StampsResellerType.StampsExpedited;
        }

        protected override void OnFormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            base.OnFormClosing(sender, e);

            if (DialogResult == DialogResult.OK)
            {
                ShippingSettingsEntity settings = ShippingSettings.Fetch();

                // We also need to exclude Endicia, Express1, and the original Stamps.com from the list 
                // of active providers since the customer agreed to use USPS (Stamps.com Expedited)
                ExcludeShipmentType(settings, ShipmentTypeCode.Endicia);
                ExcludeShipmentType(settings, ShipmentTypeCode.Express1Endicia);
                ExcludeShipmentType(settings, ShipmentTypeCode.Express1Stamps);
                ExcludeShipmentType(settings, ShipmentTypeCode.Stamps);

                ShippingSettings.Save(settings);

                // Need to update any rules to swap out Endicia, Express1, and the original Stamps.com 
                // with USPS (Stamps.com Expedited) now that those types are not longer active
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Endicia);
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Express1Endicia);
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Express1Stamps);
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Stamps);
            }
        }

        /// <summary>
        /// Excludes the given shipment type from the list of active shipping providers.
        /// </summary>
        /// <param name="settings">The settings being updated.</param>
        /// <param name="shipmentTypeCode">The shipment type code to be excluded.</param>
        private void ExcludeShipmentType(ShippingSettingsEntity settings, ShipmentTypeCode shipmentTypeCode)
        {
            if (!settings.ExcludedTypes.Any(t => t == (int)shipmentTypeCode))
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
                rule.ShipmentType = (int)ShipmentTypeCode.Usps;
                ShippingDefaultsRuleManager.SaveRule(rule);
            }
        }
    }
}
