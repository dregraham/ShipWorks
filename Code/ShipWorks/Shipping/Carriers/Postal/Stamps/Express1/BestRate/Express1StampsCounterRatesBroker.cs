using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1.BestRate
{
    /// <summary>
    /// A broker to obtain counter rates using Express1 for Stamps.com.
    /// </summary>
    public class Express1StampsCounterBestRateBroker : Express1StampsBestRateBroker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Express1StampsCounterBestRateBroker"/> class.
        /// </summary>
        public Express1StampsCounterBestRateBroker()
            : base(new Express1StampsShipmentType(), new Express1StampsCounterRatesAccountRepository(TangoCounterRatesCredentialStore.Instance))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Express1StampsCounterBestRateBroker"/> class.
        /// </summary>
        /// <param name="shipmentType">The shipment type should be used with this broker</param>
        /// <param name="accountRepository">The account repository that should be used with this broker</param>
        public Express1StampsCounterBestRateBroker(StampsShipmentType shipmentType, ICarrierAccountRepository<StampsAccountEntity> accountRepository)
            : base(shipmentType, accountRepository)
        { }

        /// <summary>
        /// Gets the best rates for for Express1Stamps counter-based prices.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="exceptionHandler">The exception handler.</param>
        /// <returns>A RateGroup containing the counter rates for an Express1Stamps account.</returns>
        public override RateGroup GetBestRates(ShipmentEntity shipment, Action<BrokerException> exceptionHandler)
        {
            ((StampsShipmentType)ShipmentType).AccountRepository = AccountRepository;
            RateGroup bestRates = base.GetBestRates(shipment, exceptionHandler);

            foreach (RateResult rateResult in bestRates.Rates)
            {
                // We want WebTools account setup wizard to show when a rate is selected so the user 
                // can create their own WebTools account since these rates are just counter rates 
                // using a ShipWorks account.
                BestRateResultTag bestRateResultTag = (BestRateResultTag)rateResult.Tag;
                bestRateResultTag.SignUpAction = DisplaySetupWizard;
            }

            return bestRates;
        }

        /// <summary>
        /// Displays the Express1Stamps setup wizard.
        /// </summary>
        private bool DisplaySetupWizard()
        {
            using (Form setupWizard = ShipmentType.CreateSetupWizard())
            {
                DialogResult result = setupWizard.ShowDialog();

                if (result == DialogResult.OK)
                {
                    ShippingSettings.MarkAsConfigured(ShipmentType.ShipmentTypeCode);
                }

                return result == DialogResult.OK;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the broker is a counter broker.
        /// </summary>
        public override bool IsCounterRate
        {
            get
            {
                return true;
            }
        }
    }
}
