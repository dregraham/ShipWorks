using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;

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
            : this(new FedExShipmentType(), new FedExCounterRateAccountRepository(TangoCounterRatesCredentialStore.Instance), new FedExCounterSettingsRepository(TangoCounterRatesCredentialStore.Instance))
        { }

        /// <summary>
        /// Gets or sets the account just created by the user.
        /// </summary>
        public FedExAccountEntity CreatedAccount { get; set; }

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
            // Use a settings repository to get counter rates
            ((FedExShipmentType)ShipmentType).SettingsRepository = settingsRepository;

            return base.GetBestRates(shipment, exceptionHandler);
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
        /// Sets the FedEx Account.  If created account is null, use the account in the parameter.
        /// </summary>
        public override void SetAccount(ShipmentEntity currentShipment, FedExAccountEntity account)
        {
            base.SetAccount(currentShipment, CreatedAccount ?? account);
        }
    }
}
