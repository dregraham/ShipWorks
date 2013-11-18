using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// Best rate broker for Express1 Stamps accounts
    /// </summary>
    public class Express1StampsBestRateBroker : StampsBestRateBroker
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1StampsBestRateBroker() : this(new Express1StampsShipmentType(), new Express1StampsAccountRepository())
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1StampsBestRateBroker(StampsShipmentType shipmentType, ICarrierAccountRepository<StampsAccountEntity> accountRepository) :
            base(shipmentType, accountRepository, "Express1 Stamps")
        {

        }
    }
}
