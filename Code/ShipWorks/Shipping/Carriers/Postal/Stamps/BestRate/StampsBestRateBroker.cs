using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate
{
    /// <summary>
    /// Best rate broker for Stamps accounts
    /// </summary>
    public class StampsBestRateBroker : PostalResellerBestRateBroker<StampsAccountEntity>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StampsBestRateBroker() : this(new StampsShipmentType(), new StampsAccountRepository())
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsBestRateBroker(StampsShipmentType shipmentType, ICarrierAccountRepository<StampsAccountEntity> accountRepository) :
            this(shipmentType, accountRepository, "USPS")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected StampsBestRateBroker(StampsShipmentType shipmentType, ICarrierAccountRepository<StampsAccountEntity> accountRepository, string carrierDescription) :
            base(shipmentType, accountRepository, carrierDescription)
        {

        }

        /// <summary>
        /// Gets the insurance provider.
        /// </summary>
        public override InsuranceProvider GetInsuranceProvider(ShippingSettingsEntity settings)
        {
            return InsuranceProvider.ShipWorks;
        }

        /// <summary>
        /// Updates the account id on the postal reseller shipment
        /// </summary>
        /// <param name="postalShipmentEntity">Postal shipment on which the account id should be set</param>
        /// <param name="account">Account that should be used for this shipment</param>
        protected override void UpdateChildAccountId(PostalShipmentEntity postalShipmentEntity, StampsAccountEntity account)
        {
            postalShipmentEntity.Stamps.StampsAccountID = account.StampsAccountID;
        }

        /// <summary>
        /// Configures the specified broker settings.
        /// </summary>
        public override void Configure(IBestRateBrokerSettings brokerSettings)
        {
            base.Configure(brokerSettings);

            ((StampsShipmentType)ShipmentType).ShouldRetrieveExpress1Rates = brokerSettings.CheckExpress1Rates(ShipmentType);
        }

        /// <summary>
        /// Gets a description from the specified account
        /// </summary>
        protected override string AccountDescription(StampsAccountEntity account)
        {
            return account.Description;
        }
    }
}
