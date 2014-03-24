using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1.BestRate
{
    /// <summary>
    /// A broker to obtain counter rates using Express1 for Stamps.com.
    /// </summary>
    public class Express1StampsCounterRatesBroker : Express1StampsBestRateBroker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Express1StampsCounterRatesBroker"/> class.
        /// </summary>
        public Express1StampsCounterRatesBroker()
            : base(new Express1StampsShipmentType(), new Express1StampsCounterRatesAccountRepository(TangoCounterRatesCredentialStore.Instance))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Express1StampsCounterRatesBroker"/> class.
        /// </summary>
        /// <param name="shipmentType">The shipment type should be used with this broker</param>
        /// <param name="accountRepository">The account repository that should be used with this broker</param>
        public Express1StampsCounterRatesBroker(StampsShipmentType shipmentType, ICarrierAccountRepository<StampsAccountEntity> accountRepository)
            : base(shipmentType, accountRepository)
        { }

        /// <summary>
        /// Gets the best rates for for Express1Stamps counter-based prices.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="brokerExceptions">The exception handler.</param>
        /// <returns>A RateGroup containing the counter rates for an Express1Stamps account.</returns>
        public override RateGroup GetBestRates(ShipmentEntity shipment, List<BrokerException> brokerExceptions)
        {
            string certificateVerificationData = TangoCounterRatesCredentialStore.Instance.Express1StampsCertificateVerificationData;
            ShipmentType.CertificateInspector = new CertificateInspector(certificateVerificationData);

            RateGroup bestRates = new RateGroup(new List<RateResult>());

            ((StampsShipmentType)ShipmentType).AccountRepository = AccountRepository;

            try
            {
                bestRates = base.GetBestRates(shipment, brokerExceptions);

                foreach (RateResult rateResult in bestRates.Rates)
                {
                    // We want the account setup wizard to show when a rate is selected so the user 
                    // can create their own Express1 account since these rates are just counter rates 
                    // using a ShipWorks account.
                    BestRateResultTag bestRateResultTag = (BestRateResultTag)rateResult.Tag;
                    bestRateResultTag.SignUpAction = DisplaySetupWizard;

                    // The counter rate shouldn't show the Express1 logo
                    rateResult.ProviderLogo = EnumHelper.GetImage(ShipmentTypeCode.PostalWebTools);
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
        /// Updates the shipment origin address for getting counter rates. In cases where a shipment is 
        /// configured to use the Account address or there is an incomplete "Other" address, we want
        /// to use the store address for getting counter rates.
        /// </summary>
        /// <param name="currentShipment">The current shipment.</param>
        /// <param name="originalShipment">The original shipment.</param>
        /// <param name="account">The account.</param>
        protected override void UpdateShipmentOriginAddress(ShipmentEntity currentShipment, ShipmentEntity originalShipment, StampsAccountEntity account)
        {
            base.UpdateShipmentOriginAddress(currentShipment, originalShipment, account);
            CounterRatesOriginAddressValidator.EnsureValidAddress(currentShipment);
        }

        /// <summary>
        /// Wraps the shipping exception.
        /// </summary>
        protected override BrokerException WrapShippingException(ShippingException ex)
        {
            // Since this is just getting counter rates, we want to have the severity level
            // as information for all shipping exceptions
            return new BrokerException(ex, BrokerExceptionSeverityLevel.Information, ShipmentType);
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

                    // We also want to ensure sure that the provider is no longer excluded in
                    // the global settings
                    ShippingSettingsEntity settings = ShippingSettings.Fetch();
                    settings.ExcludedTypes = settings.ExcludedTypes.Where(shipmentType => shipmentType != (int)ShipmentType.ShipmentTypeCode).ToArray();

                    ShippingSettings.Save(settings);
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

        /// <summary>
        /// Updates the shipment account with the actual account
        /// </summary>
        protected override void UpdateChildAccountId(PostalShipmentEntity postalShipmentEntity, StampsAccountEntity account)
        {
            Express1StampsAccountRepository ar = new Express1StampsAccountRepository();
            StampsAccountEntity stampsAccount = ar.Accounts.FirstOrDefault();

            if (postalShipmentEntity.Stamps != null && stampsAccount != null)
            {
                postalShipmentEntity.Stamps.StampsAccountID = stampsAccount.StampsAccountID;
                account = stampsAccount;
            }

            base.UpdateChildAccountId(postalShipmentEntity, account);
        }
    }
}
