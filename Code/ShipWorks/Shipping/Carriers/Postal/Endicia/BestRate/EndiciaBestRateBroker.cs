using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.BestRate;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate
{
    /// <summary>
    /// Best rate broker for Endicia accounts
    /// </summary>
    public class EndiciaBestRateBroker : PostalResellerBestRateBroker<EndiciaAccountEntity>
    {
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
            this(shipmentType, accountRepository, "Endicia")
        {

        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        protected EndiciaBestRateBroker(EndiciaShipmentType shipmentType, ICarrierAccountRepository<EndiciaAccountEntity> accountRepository, string carrierDescription) :
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
            // Save a reference to the Endicia shipment entity because if we set the shipment id while it's 
            // attached to the Postal entity, the Endicia entity will be set to null
            EndiciaShipmentEntity newEndiciaShipment = rateShipment.Endicia;
            newEndiciaShipment.ShipmentID = selectedShipment.ShipmentID;

            selectedShipment.Postal.Endicia = newEndiciaShipment;
            selectedShipment.Postal.Endicia.IsNew = false;
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
    }
}
