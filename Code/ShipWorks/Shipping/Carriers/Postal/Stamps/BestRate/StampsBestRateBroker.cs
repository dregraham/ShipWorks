using ShipWorks.Data.Model.EntityClasses;
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
            this(shipmentType, accountRepository, "Stamps")
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
        /// Convert the best rate shipment into the specified postal reseller shipment
        /// </summary>
        /// <param name="rateShipment">Postal shipment on which to set reseller shipment data</param>
        /// <param name="selectedShipment">Best rate shipment that is being converted</param>
        protected override void SelectChildShipment(PostalShipmentEntity rateShipment, ShipmentEntity selectedShipment)
        {
            // Save a reference to the stamps shipment entity because if we set the shipment id while it's 
            // attached to the Postal entity, the Stamps entity will be set to null
            StampsShipmentEntity newStampsShipment = rateShipment.Stamps;
            newStampsShipment.ShipmentID = selectedShipment.ShipmentID;

            selectedShipment.Postal.Stamps = newStampsShipment;
            selectedShipment.Postal.Stamps.IsNew = false;
        }

        /// <summary>
        /// Gets the insurance provider.
        /// </summary>
        public override InsuranceProvider GetInsuranceProvider(ShippingSettingsEntity settings)
        {
            return InsuranceProvider.ShipWorks;
        }

        /// <summary>
        /// Configures a postal reseller shipment for use in the get rates method
        /// </summary>
        /// <param name="shipment">Test shipment that will be used to get rates</param>
        protected override void CreateShipmentChild(ShipmentEntity shipment)
        {
            base.CreateShipmentChild(shipment);
            shipment.Postal.Stamps = new StampsShipmentEntity();
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

        protected override void AddCarrierNameToFootnoteText(RateGroup rates)
        {
            //throw new System.NotImplementedException();
        }
    }
}
