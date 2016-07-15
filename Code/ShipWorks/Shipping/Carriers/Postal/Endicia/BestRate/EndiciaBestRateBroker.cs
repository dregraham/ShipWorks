using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate
{
    /// <summary>
    /// Best rate broker for Endicia accounts
    /// </summary>
    public class EndiciaBestRateBroker : PostalResellerBestRateBroker<EndiciaAccountEntity>
    {
        bool isEndiciaDhlEnabled;
        bool isEndiciaConsolidatorEnabled;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaBestRateBroker() : this(new EndiciaShipmentType(), new EndiciaAccountRepository())
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaBestRateBroker(EndiciaShipmentType shipmentType, ICarrierAccountRepository<EndiciaAccountEntity> accountRepository) :
            this(shipmentType, accountRepository, "USPS")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected EndiciaBestRateBroker(EndiciaShipmentType shipmentType, ICarrierAccountRepository<EndiciaAccountEntity> accountRepository, string carrierDescription) :
            base(shipmentType, accountRepository, carrierDescription)
        {
            GetRatesAction = (shipment, type) => GetRatesFunction(shipment);
        }

        /// <summary>
        /// Gets the insurance provider.
        /// </summary>
        public override InsuranceProvider GetInsuranceProvider(ShippingSettingsEntity settings)
        {
            return (InsuranceProvider) settings.EndiciaInsuranceProvider;
        }

        /// <summary>
        /// Configures a postal reseller shipment for use in the get rates method
        /// </summary>
        /// <param name="shipment">Test shipment that will be used to get rates</param>
        protected override void CreateShipmentChild(ShipmentEntity shipment)
        {
            base.CreateShipmentChild(shipment);
            shipment.Postal.Endicia = new EndiciaShipmentEntity();
        }

        /// <summary>
        /// Updates the account id on the postal reseller shipment
        /// </summary>
        /// <param name="postalShipmentEntity">Postal shipment on which the account id should be set</param>
        /// <param name="account">Account that should be used for this shipment</param>
        protected override void UpdateChildAccountId(PostalShipmentEntity postalShipmentEntity, EndiciaAccountEntity account)
        {
            postalShipmentEntity.Endicia.EndiciaAccountID = account.EndiciaAccountID;
        }

        /// <summary>
        /// Gets best rates for Endicia
        /// </summary>
        /// <returns>Best rates from Endicia</returns>
        /// <remarks>Adds an informational error when no DHL rates are returned</remarks>
        public override RateGroup GetBestRates(ShipmentEntity shipment, List<BrokerException> brokerExceptions)
        {
            RateGroup rates = base.GetBestRates(shipment, brokerExceptions);

            if (isEndiciaDhlEnabled)
            {
                brokerExceptions.Add(new BrokerException(new ShippingException("Endicia did not provide DHL rates."), BrokerExceptionSeverityLevel.Information, ShipmentType));
            }

            if (isEndiciaConsolidatorEnabled)
            {
                brokerExceptions.Add(new BrokerException(new ShippingException("Endicia did not provide consolidator rates."), BrokerExceptionSeverityLevel.Information, ShipmentType));
            }

            return rates;
        }

        /// <summary>
        /// Configures the specified broker settings.
        /// </summary>
        public override void Configure(IBestRateBrokerSettings brokerSettings)
        {
            base.Configure(brokerSettings);

            isEndiciaDhlEnabled = brokerSettings.IsEndiciaDHLEnabled();
            isEndiciaConsolidatorEnabled = brokerSettings.IsEndiciaConsolidatorEnabled();

            ((EndiciaShipmentType) ShipmentType).ShouldRetrieveExpress1Rates = brokerSettings.CheckExpress1Rates(ShipmentType);
        }

        /// <summary>
        /// Gets a description from the specified account
        /// </summary>
        protected override string AccountDescription(EndiciaAccountEntity account)
        {
            return account.Description;
        }
    }
}
