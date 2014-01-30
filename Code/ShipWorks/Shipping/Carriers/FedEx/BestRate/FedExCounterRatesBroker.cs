using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.FedEx.BestRate
{
    public class FedExCounterRatesBroker : FedExBestRateBroker
    {
        private readonly ICarrierSettingsRepository settingsRepository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCounterRatesBroker"/> class using
        /// the <see cref="TangoCounterRatesCredentialStore"/> as the underlying credential store.
        /// </summary>
        public FedExCounterRatesBroker()
            : this(new FedExShipmentType(), new FedExCounterRateAccountRepository(TangoCounterRatesCredentialStore.Instance), new FedExCounterRateAccountRepository(TangoCounterRatesCredentialStore.Instance))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCounterRatesBroker" /> class.
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExCounterRatesBroker(ShipmentType shipmentType, ICarrierAccountRepository<FedExAccountEntity> accountRepository, ICarrierSettingsRepository settingsRepository)
            : base(shipmentType, accountRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Gets the best rates for for FedEx counter-based prices.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="exceptionHandler">The exception handler.</param>
        /// <returns>A RateGroup containing the counter rates for a generic FedEx account.</returns>
        public override RateGroup GetBestRates(ShipmentEntity shipment, Action<BrokerException> exceptionHandler)
        {
            RateGroup bestRates = new RateGroup(new List<RateResult>());

            // Use a settings repository to get counter rates
            ((FedExShipmentType)ShipmentType).SettingsRepository = settingsRepository;

            try
            {
                bestRates = base.GetBestRates(shipment, exceptionHandler);

                foreach (BestRateResultTag bestRateResultTag in bestRates.Rates.Select(rate => (BestRateResultTag)rate.Tag))
                {
                    // We want FedEx account setup wizard to show when a rate is selected so the user 
                    // can create their own FedEx account since these rates are just counter rates 
                    // using a ShipWorks account.
                    bestRateResultTag.SignUpAction = new Func<bool>(DisplaySetupWizard);
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
        protected override void UpdateShipmentOriginAddress(ShipmentEntity currentShipment, ShipmentEntity originalShipment, FedExAccountEntity account)
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
        /// Displays the FedEx setup wizard.
        /// </summary>
        private bool DisplaySetupWizard()
        {
            using(Form setupWizard = ShipmentType.CreateSetupWizard())
            {
                DialogResult result = setupWizard.ShowDialog();

                if (result == DialogResult.OK)
                {
                    ShippingSettings.MarkAsConfigured(ShipmentTypeCode.FedEx);
                }

                return result == DialogResult.OK;
            }
        }

        /// <summary>
        /// Applies FedEx specific data to the specified shipment
        /// </summary>
        /// <param name="currentShipment">Shipment that will be modified</param>
        /// <param name="originalShipment">Shipment that contains original data for copying</param>
        /// <param name="account">Account that will be attached to the shipment</param>
        protected override void UpdateChildShipmentSettings(ShipmentEntity currentShipment, ShipmentEntity originalShipment, FedExAccountEntity account)
        {
            base.UpdateChildShipmentSettings(currentShipment, originalShipment, account);
            currentShipment.FedEx.SmartPostHubID = string.Empty;
        }

        /// <summary>
        /// Wraps the shipping exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>A new BrokerException with a severity level of Information.</returns>
        protected override BrokerException WrapShippingException(ShippingException ex)
        {
            // Since this is just getting counter rates, we want to have the severity level
            // as information for all shipping exceptions
            return new BrokerException(ex, BrokerExceptionSeverityLevel.Information, ShipmentType);
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
