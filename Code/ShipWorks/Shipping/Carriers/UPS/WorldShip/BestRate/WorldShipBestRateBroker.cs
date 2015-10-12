using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate
{
    /// <summary>
    /// Rate broker that finds the best rates for UPS accounts
    /// </summary>
    public class WorldShipBestRateBroker : UpsBestRateBroker
    {
        /// <summary>
        /// Creates a broker with the default shipment type and account repository
        /// </summary>
        /// <remarks>This is designed to be used within ShipWorks</remarks>
        public WorldShipBestRateBroker(ShipmentType shipmentType) : base(shipmentType, new WorldShipAccountRepository(), new UpsSettingsRepository())
        {
        }

        /// <summary>
        /// Creates a broker with the specified shipment type and account repository
        /// </summary>
        /// <param name="shipmentType">Instance of a UPS WorldShip shipment type that will be used to get rates</param>
        /// <param name="accountRepository">Instance of an account repository that will get UPS accounts</param>
        /// <remarks>This is designed to be used by tests</remarks>
        public WorldShipBestRateBroker(ShipmentType shipmentType, ICarrierAccountRepository<UpsAccountEntity> accountRepository) : base(shipmentType, accountRepository, new UpsSettingsRepository())
        {
        }
    }
}
