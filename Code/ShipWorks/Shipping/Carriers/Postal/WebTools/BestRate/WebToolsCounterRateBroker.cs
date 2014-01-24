using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools.BestRate
{
    public class WebToolsCounterRatesBroker : WebToolsBestRateBroker
    {
        /// <summary>
        /// Gets the best rates for for WebTools counter-based prices.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="exceptionHandler">The exception handler.</param>
        /// <returns>A RateGroup containing the counter rates for a generic WebTools account.</returns>
        public override RateGroup GetBestRates(ShipmentEntity shipment, Action<BrokerException> exceptionHandler)
        {
            // Use a settings repository to get counter rates
            //((PostalWebShipmentType)ShipmentType).SettingsRepository = settingsRepository;
            RateGroup bestRates = base.GetBestRates(shipment, exceptionHandler);

            foreach (RateResult rateResult in bestRates.Rates)
            {
                rateResult.Description = rateResult.Description.Replace("(w/o Postage) ", string.Empty);

                // We want WebTools account setup wizard to show when a rate is selected so the user 
                // can create their own WebTools account since these rates are just counter rates 
                // using a ShipWorks account.
                BestRateResultTag bestRateResultTag = (BestRateResultTag)rateResult.Tag;
                bestRateResultTag.SignUpAction = DisplaySetupWizard;
            }

            return bestRates;
        }

        /// <summary>
        /// Displays the WebTools setup wizard.
        /// </summary>
        private bool DisplaySetupWizard()
        {
            using(Form setupWizard = ShipmentType.CreateSetupWizard())
            {
                DialogResult result = setupWizard.ShowDialog();

                if (result == DialogResult.OK)
                {
                    ShippingSettings.MarkAsConfigured(ShipmentTypeCode.PostalWebTools);
                }

                return result == DialogResult.OK;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the broker is a counter broker.
        /// </summary>
        /// <value>
        ///   Returns false.
        /// </value>
        public override bool IsCounterRate
        {
            get
            {
                return true;
            }
        }
    }
}
