using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Editing;
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
        /// <param name="exceptionHandler">The exception handler.</param>
        /// <returns>A RateGroup containing the counter rates for an Express1Stamps account.</returns>
        public override RateGroup GetBestRates(ShipmentEntity shipment, Action<BrokerException> exceptionHandler)
        {
            RateGroup bestRates = new RateGroup(new List<RateResult>());

            ((StampsShipmentType)ShipmentType).AccountRepository = AccountRepository;

            try
            {
                bestRates = base.GetBestRates(shipment, exceptionHandler);

                foreach (RateResult rateResult in bestRates.Rates)
                {
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

            if (currentShipment.OriginOriginID == (int)ShipmentOriginSource.Account
                || (currentShipment.OriginOriginID == (int)ShipmentOriginSource.Other && !CounterRatesOriginAddressValidator.IsValidate(currentShipment)))
            {
                // We don't have an account for counter rates or "Other" is selected and is incomplete, 
                // so we'll try to use the store address
                OrderEntity order = DataProvider.GetEntity(currentShipment.OrderID) as OrderEntity;
                StoreEntity store = DataProvider.GetEntity(order.StoreID) as StoreEntity;

                PersonAdapter.Copy(store, string.Empty, currentShipment, "Origin");
            }

            if (!CounterRatesOriginAddressValidator.IsValidate(currentShipment))
            {
                // The store address is incomplete, too, so the origin address is still incomplete
                throw new CounterRatesOriginAddressException(currentShipment, "The origin address of this shipment is invalid for getting counter rates.");
            }
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
