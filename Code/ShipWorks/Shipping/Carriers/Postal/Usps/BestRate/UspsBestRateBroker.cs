using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.BestRate
{
    /// <summary>
    /// Best rate broker for USPS (Stamps.com Expedited) accounts
    /// </summary>
    public class UspsBestRateBroker : StampsBestRateBroker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsBestRateBroker"/> class.
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        /// <param name="accountRepository">The account repository.</param>
        public UspsBestRateBroker(UspsShipmentType shipmentType, ICarrierAccountRepository<UspsAccountEntity> accountRepository) :
            this(shipmentType, accountRepository, "USPS")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsBestRateBroker"/> class.
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="carrierDescription">The carrier description.</param>
        protected UspsBestRateBroker(UspsShipmentType shipmentType, ICarrierAccountRepository<UspsAccountEntity> accountRepository, string carrierDescription) :
            base(shipmentType, accountRepository, carrierDescription)
        { }

        /// <summary>
        /// Configures the specified broker settings. Overridden to explicitly indicate that Express1 rates
        /// should not be checked.
        /// </summary>
        /// <param name="brokerSettings"></param>
        public override void Configure(Carriers.BestRate.IBestRateBrokerSettings brokerSettings)
        {
            base.Configure(brokerSettings);

            // Make sure that Express1 rates are not checked.
            ((UspsShipmentType) ShipmentType).ShouldRetrieveExpress1Rates = false;
        }
    }
}
