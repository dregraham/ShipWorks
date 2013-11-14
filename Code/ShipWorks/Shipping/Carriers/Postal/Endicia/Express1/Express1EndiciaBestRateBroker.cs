using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Best rate broker for Express1 Endicia accounts
    /// </summary>
    public class Express1EndiciaBestRateBroker : EndiciaBestRateBroker
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1EndiciaBestRateBroker() : this(new Express1EndiciaShipmentType(), new Express1EndiciaAccountRepository())
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1EndiciaBestRateBroker(EndiciaShipmentType shipmentType, ICarrierAccountRepository<EndiciaAccountEntity> accountRepository) :
            base(shipmentType, accountRepository, "Express1 Endicia")
        {

        }

        /// <summary>
        /// Gets the shipment type code for the postal reseller shipment type
        /// </summary>
        protected override ShipmentTypeCode ShipmentCode
        {
            get { return ShipmentTypeCode.Express1Endicia; }
        }
    }
}
