﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;

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
            : base(new Express1EndiciaShipmentType(), new Express1EndiciaCounterAccountRepository(TangoCounterRatesCredentialStore.Instance))
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
        /// <param name="exceptionHandler">The exception handler.</param>
        /// <returns>A RateGroup containing the counter rates for an Express1Endicia account.</returns>
        public override RateGroup GetBestRates(ShipmentEntity shipment, Action<BrokerException> exceptionHandler)
        {
            RateGroup bestRates = new RateGroup(new List<RateResult>());

            ((EndiciaShipmentType)ShipmentType).AccountRepository = AccountRepository;

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
        protected override void UpdateShipmentOriginAddress(ShipmentEntity currentShipment, ShipmentEntity originalShipment, EndiciaAccountEntity account)
        {
            base.UpdateShipmentOriginAddress(currentShipment, originalShipment, account);

            if (currentShipment.OriginOriginID == (int)ShipmentOriginSource.Account)
            {
                // We don't have an account for counter rates, so we need to use the store address
                PersonAdapter.Copy(currentShipment.Order.Store, currentShipment, "Origin");
            }

            // Check to see if the address is incomplete
            CounterRatesOriginAddressValidator.Validate(currentShipment);
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
