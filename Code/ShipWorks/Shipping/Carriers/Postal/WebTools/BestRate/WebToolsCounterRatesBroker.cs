using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using ShipWorks.Data;
using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools.BestRate
{
    /// <summary>
    /// Gets counter rates for USPS
    /// </summary>
    public class WebToolsCounterRatesBroker : PostalResellerBestRateBroker<NullEntity>
    {
        private readonly PostalShipmentType actualPostalShipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public WebToolsCounterRatesBroker(PostalShipmentType actualPostalShipmentType)
            : this(new PostalWebShipmentType(), new WebToolsAccountRepository())
        {
            this.actualPostalShipmentType = actualPostalShipmentType;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        private WebToolsCounterRatesBroker(PostalWebShipmentType shipmentType, ICarrierAccountRepository<NullEntity> accountRepository) :
            base(shipmentType, accountRepository, "USPS")
        {

        }

        /// <summary>
        /// Shipment type for the broker
        /// </summary>
        public override ShipmentType ShipmentType
        {
            get { return actualPostalShipmentType; }
        }

        /// <summary>
        /// Gets the best rates for for WebTools counter-based prices.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="brokerExceptions">The exception handler.</param>
        /// <returns>A RateGroup containing the counter rates for a generic WebTools account.</returns>
        public override RateGroup GetBestRates(ShipmentEntity shipment, List<BrokerException> brokerExceptions)
        {
            RateGroup bestRates = new RateGroup(new List<RateResult>());

            try
            {
                bestRates = base.GetBestRates(shipment, brokerExceptions);

                foreach (RateResult rateResult in bestRates.Rates)
                {
                    rateResult.Description = rateResult.Description.Replace("(w/o Postage) ", string.Empty);

                    // We want WebTools account setup wizard to show when a rate is selected so the user 
                    // can create their own WebTools account since these rates are just counter rates 
                    // using a ShipWorks account.
                    BestRateResultTag bestRateResultTag = (BestRateResultTag)rateResult.Tag;
                    bestRateResultTag.SignUpAction = DisplaySetupWizard;
                }
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count == 1 && ex.InnerExceptions.OfType<CounterRatesOriginAddressException>().Any())
                {
                    // There was a problem with the origin address, so add the invalid store address footer factory 
                    // to the rate group and eat the exception
                    bestRates.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(ShipmentType));
                }
                else
                {
                    // Some other kind of exceptions were encountered that we want to bubble up
                    throw;
                }
            }

            return bestRates;
        }

        /// <summary>
        /// Displays the WebTools setup wizard.
        /// </summary>
        private bool DisplaySetupWizard()
        {
            using (Form setupWizard = actualPostalShipmentType.CreateSetupWizard())
            {
                DialogResult result = setupWizard.ShowDialog();

                if (result == DialogResult.OK)
                {
                    ShippingSettings.MarkAsConfigured(actualPostalShipmentType.ShipmentTypeCode);

                    // We also want to ensure sure that the provider is no longer excluded in
                    // the global settings
                    ShippingSettingsEntity settings = ShippingSettings.Fetch();
                    settings.ExcludedTypes = settings.ExcludedTypes.Where(shipmentType => shipmentType != (int)actualPostalShipmentType.ShipmentTypeCode).ToArray();

                    ShippingSettings.Save(settings);
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

        /// <summary>
        /// Changes the shipment type on the specified shipment
        /// </summary>
        protected override void ChangeShipmentType(ShipmentEntity selectedShipment)
        {
            selectedShipment.ShipmentType = (int)actualPostalShipmentType.ShipmentTypeCode;
        }

        /// <summary>
        /// Updates the account id on the postal reseller shipment
        /// </summary>
        /// <param name="postalShipmentEntity">Postal shipment on which the account id should be set</param>
        /// <param name="account">Account that should be used for this shipment</param>
        protected override void UpdateChildAccountId(PostalShipmentEntity postalShipmentEntity, NullEntity account)
        {

        }

        /// <summary>
        /// Gets the insurance provider.
        /// </summary>
        public override InsuranceProvider GetInsuranceProvider(ShippingSettingsEntity settings)
        {
            return InsuranceProvider.ShipWorks;
        }
    }
}
