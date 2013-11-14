using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate
{
    public class StampsBestRateBroker : PostalResellerBestRateBroker<StampsAccountEntity>
    {
        private readonly StampsShipmentType shipmentType;

        public StampsBestRateBroker() : this(new StampsShipmentType(), new StampsAccountRepository())
        {
            
        }

        public StampsBestRateBroker(StampsShipmentType shipmentType, ICarrierAccountRepository<StampsAccountEntity> accountRepository) :
            base(accountRepository, "Stamps")
        {
            this.shipmentType = shipmentType; 
        }

        /// <summary>
        /// Gets the shipment type code for the postal reseller shipment type
        /// </summary>
        protected override ShipmentTypeCode ShipmentCode
        {
            get { return ShipmentTypeCode.Stamps; }
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
        /// Configures a postal reseller shipment for use in the get rates method
        /// </summary>
        /// <param name="shipment">Test shipment that will be used to get rates</param>
        protected override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            shipment.Postal.Stamps = new StampsShipmentEntity();
            shipmentType.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Gets rates for the specified shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to get rates</param>
        /// <returns>List of rates</returns>
        protected override IEnumerable<RateResult> GetRates(ShipmentEntity shipment)
        {
            return shipmentType.GetRates(shipment).Rates;
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
    }
}
