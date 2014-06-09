using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.BestRate
{
    /// <summary>
    /// Best rate broker for Express1 Endicia rates
    /// </summary>
    public class Express1EndiciaCounterRatesBroker : Express1EndiciaBestRateBroker
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1EndiciaCounterRatesBroker() 
            : base(new Express1EndiciaShipmentType(), new Express1EndiciaCounterAccountRepository(TangoCredentialStore.Instance))
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipmentType">The shipment type should be used with this broker</param>
        /// <param name="accountRepository">The account repository that should be used with this broker</param>
        public Express1EndiciaCounterRatesBroker(EndiciaShipmentType shipmentType, ICarrierAccountRepository<EndiciaAccountEntity> accountRepository)
            : base(shipmentType, accountRepository)
        { }

        /// <summary>
        /// Gets the best rates for for Express1Endicia counter-based prices.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="brokerExceptions">The exception handler.</param>
        /// <returns>A RateGroup containing the counter rates for an Express1Endicia account.</returns>
        public override RateGroup GetBestRates(ShipmentEntity shipment, List<BrokerException> brokerExceptions)
        {
            RateGroup bestRates = new RateGroup(new List<RateResult>());

            string certificateVerificationData = TangoCredentialStore.Instance.Express1EndiciaCertificateVerificationData;
            ShipmentType.CertificateInspector = new CertificateInspector(certificateVerificationData);
            ((EndiciaShipmentType)ShipmentType).AccountRepository = AccountRepository;

            // The dummy account wouldn't have an account number if we couldn't get one from Tango
            EndiciaAccountEntity account = AccountRepository.GetAccount(0);
            if (account == null || string.IsNullOrEmpty(account.AccountNumber))
            {
                brokerExceptions.Add(new BrokerException(new ShippingException("Could not get counter rates for Express1"), BrokerExceptionSeverityLevel.Information, ShipmentType));
                return bestRates;
            }

            try
            {
                bestRates = base.GetBestRates(shipment, brokerExceptions);

                foreach (RateResult rateResult in bestRates.Rates)
                {
                    // We want WebTools account setup wizard to show when a rate is selected so the user 
                    // can create their own WebTools account since these rates are just counter rates 
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
        protected override void UpdateShipmentOriginAddress(ShipmentEntity currentShipment, ShipmentEntity originalShipment, EndiciaAccountEntity account)
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
        /// Displays the Express1Endicia setup wizard.
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
        protected override void UpdateChildAccountId(PostalShipmentEntity postalShipmentEntity, EndiciaAccountEntity account)
        {
            Express1EndiciaAccountRepository ar = new Express1EndiciaAccountRepository();
            EndiciaAccountEntity endiciaAccount = ar.Accounts.FirstOrDefault();

            if (postalShipmentEntity.Endicia != null && endiciaAccount != null)
            {
                postalShipmentEntity.Endicia.EndiciaAccountID = endiciaAccount.EndiciaAccountID;
                account = endiciaAccount;
            }

            base.UpdateChildAccountId(postalShipmentEntity, account);
        }
    }
}
