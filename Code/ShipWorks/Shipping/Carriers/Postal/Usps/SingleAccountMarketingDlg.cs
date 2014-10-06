using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Defaults;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public partial class SingleAccountMarketingDlg : Form
    {
        private readonly ShipmentEntity shipment;
        private readonly ShippingSettingsEntity settings;
        private IUspsAutomaticDiscountControlAdapter discountControlAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleAccountMarketingDlg"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public SingleAccountMarketingDlg(ShipmentEntity shipment)
        {
            this.shipment = shipment;
            settings = ShippingSettings.Fetch();

            discountControlAdapter = new UspsAutomaticDiscountControlAdapterFactory().CreateAdapter(settings, shipment);

            InitializeComponent();
        }

        /// <summary>
        /// Closing the window
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (expeditedDiscountControl.UseExpedited)
            {
                // Make sure the settings are valid before trying to save them
                if (discountControlAdapter.UsingUspsAutomaticExpedited && discountControlAdapter.UspsAutomaticExpeditedAccount <= 0)
                {
                    MessageHelper.ShowMessage(this, "Please select or create a USPS account.");
                    e.Cancel = true;
                    return;
                }

                // Only way this dialog should be presented to the use is if they are using Express1, 
                // so we need to change the shipment type to USPS (Stamps.com Expedited) 
                // in order to take advantage of the new rates (since Stamps.com API doesn't match 
                // with Endicia API and shipment configurations differ).
                shipment.ShipmentType = (int)ShipmentTypeCode.Usps;
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

                DialogResult = discountControlAdapter.UsingUspsAutomaticExpedited ? DialogResult.OK : DialogResult.Cancel;
            }
        }

        /// <summary>
        /// Excludes the given shipment type from the list of active shipping providers.
        /// </summary>
        /// <param name="shipmentTypeCode">The shipment type code to be excluded.</param>
        private void ExcludeShipmentType(ShipmentTypeCode shipmentTypeCode)
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
