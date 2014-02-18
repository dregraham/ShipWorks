﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.UPS.BestRate
{
    public class UpsCounterRatesBroker : UpsBestRateBroker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsCounterRatesBroker"/> class.
        /// </summary>
        /// <remarks>
        /// This is designed to be used within ShipWorks
        /// </remarks>
        public UpsCounterRatesBroker()
            : this(new UpsOltShipmentType(), new UpsCounterRateAccountRepository(TangoCounterRatesCredentialStore.Instance), new UpsCounterRateSettingsRepository(TangoCounterRatesCredentialStore.Instance))
        {}


        /// <summary>
        /// Initializes a new instance of the <see cref="UpsCounterRatesBroker"/> class.
        /// </summary>
        public UpsCounterRatesBroker(UpsShipmentType upsShipmentType, ICarrierAccountRepository<UpsAccountEntity> accountRepository, ICarrierSettingsRepository settingsRepository) 
            : base(upsShipmentType, accountRepository, settingsRepository)
        {

        }

        /// <summary>
        /// Gets a list of UPS rates
        /// </summary>
        /// <param name="shipment">Shipment for which rates should be retrieved</param>
        /// <param name="brokerExceptions">Action that performs exception handling</param>
        /// <returns>A RateGroup containing the counter rates for a generic UPS account.</returns>
        public override RateGroup GetBestRates(ShipmentEntity shipment, List<BrokerException> brokerExceptions)
        {
            RateGroup rates = new RateGroup(new List<RateResult>());
            ((UpsShipmentType)ShipmentType).SettingsRepository = SettingsRepository;
            ((UpsShipmentType)ShipmentType).AccountRepository = AccountRepository;

            // The dummy account wouldn't have an account number if we couldn't get one from Tango
            UpsAccountEntity account = AccountRepository.GetAccount(0);
            if (account == null || string.IsNullOrEmpty(account.UserID))
            {
                brokerExceptions.Add(new BrokerException(new ShippingException("Could not get counter rates for UPS"), BrokerExceptionSeverityLevel.Information, ShipmentType));
                return rates;
            }

            try
            {
                // We're passing in our own exception handler to accumulate broker exceptions otherwise
                // any SurePost exceptions generated will display as having a Warning severity level; later
                // on we're going to make sure any broker exceptions have the severity level set to 
                // information and then call the exception handler provided.
                rates = base.GetBestRates(shipment, brokerExceptions);

                foreach (BestRateResultTag bestRateResultTag in rates.Rates.Select(rate => (BestRateResultTag)rate.Tag))
                {
                    // We want the UPS account setup wizard to show when a rate is selected so the user 
                    // can create their own UPS account since these rates are just counter rates 
                    // using a ShipWorks account.
                    bestRateResultTag.SignUpAction = new Func<bool>(DisplaySetupWizard);
                }

                // Since we sent our own exception handler to the GetBestRates method, we need to 
                // call the exception handler that was provided with any broker exceptions
                foreach (BrokerException brokerException in brokerExceptions)
                {
                    if (brokerException.SeverityLevel != BrokerExceptionSeverityLevel.Information)
                    {
                        // Translate the broker exception to an informational severity level before
                        // sending it to the exception handler
                        brokerExceptions.Add(new BrokerException(brokerException, BrokerExceptionSeverityLevel.Information, brokerException.ShipmentType));
                    }
                    else
                    {
                        brokerExceptions.Add(brokerException);
                    }
                }
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count == 1 && ex.InnerExceptions.OfType<CounterRatesOriginAddressException>().Any())
                {
                    // There was a problem with the origin address, so add the invalid store address footer factory 
                    // to the rate group and eat the exception
                    rates.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(ShipmentType));
                }
                else
                {
                    // Some other kind of exceptions were encountered that we want to bubble up
                    throw;
                }
            }

            return rates;
        }

        /// <summary>
        /// Updates the shipment origin address for getting counter rates. In cases where a shipment is 
        /// configured to use the Account address or there is an incomplete "Other" address, we want
        /// to use the store address for getting counter rates.
        /// </summary>
        /// <param name="currentShipment">The current shipment.</param>
        /// <param name="originalShipment">The original shipment.</param>
        /// <param name="account">The account.</param>
        protected override void UpdateShipmentOriginAddress(ShipmentEntity currentShipment, ShipmentEntity originalShipment, UpsAccountEntity account)
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
        /// Displays the UPS setup wizard.
        /// </summary>
        private bool DisplaySetupWizard()
        {
            using (Form setupWizard = ShipmentType.CreateSetupWizard())
            {
                DialogResult result = setupWizard.ShowDialog();

                if (result == DialogResult.OK)
                {
                    ShippingSettings.MarkAsConfigured(ShipmentTypeCode.UpsOnLineTools);

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
        /// Gets the first account from the repository and returns the ID.
        /// </summary>
        protected override long GetAccountID(UpsAccountEntity account)
        {
            return account.UpsAccountID;
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
        /// Gets a value indicating whether [is counter rate].
        /// </summary>
        /// <value>Always returns <c>true</c>.</value>
        public override bool IsCounterRate
        {
            get { return true; }
        }
    }
}