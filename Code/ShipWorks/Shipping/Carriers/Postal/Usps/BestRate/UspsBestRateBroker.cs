using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.BestRate
{
    /// <summary>
    /// Best rate broker for USPS accounts
    /// </summary>
    public class UspsBestRateBroker : PostalResellerBestRateBroker<UspsAccountEntity>
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
        {
            GetRatesAction = (shipment, type) => GetRatesFunction(shipment);
        }

        /// <summary>
        /// Gets the insurance provider.
        /// </summary>
        public override InsuranceProvider GetInsuranceProvider(ShippingSettingsEntity settings)
        {
            return (InsuranceProvider)settings.UspsInsuranceProvider;
        }

        /// <summary>
        /// Configures a postal reseller shipment for use in the get rates method
        /// </summary>
        /// <param name="shipment">Test shipment that will be used to get rates</param>
        protected override void CreateShipmentChild(ShipmentEntity shipment)
        {
            base.CreateShipmentChild(shipment);
            shipment.Postal.Usps = new UspsShipmentEntity();
        }

        /// <summary>
        /// Updates the account id on the postal reseller shipment
        /// </summary>
        /// <param name="postalShipmentEntity">Postal shipment on which the account id should be set</param>
        /// <param name="account">Account that should be used for this shipment</param>
        protected override void UpdateChildAccountId(PostalShipmentEntity postalShipmentEntity, UspsAccountEntity account)
        {
            postalShipmentEntity.Usps.UspsAccountID = account.UspsAccountID;
        }

        /// <summary>
        /// Gets a description from the specified account
        /// </summary>
        protected override string AccountDescription(UspsAccountEntity account)
        {
            return account.Description;
        }
    }
}
