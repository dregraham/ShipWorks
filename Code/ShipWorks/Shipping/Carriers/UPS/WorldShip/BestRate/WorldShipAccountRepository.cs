using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate
{
    /// <summary>
    /// Implementation of ICarrierAccountRepository for WorldShip.
    /// </summary>
    public class WorldShipAccountRepository : UpsAccountRepository
    {
        /// <summary>
        ///  Returns the default account as defined by the primary profile
        ///  </summary>
        public override UpsAccountEntity DefaultProfileAccount
        {
            get
            {
                long? accountID = GetPrimaryProfile(ShipmentTypeCode.UpsWorldShip).Ups.UpsAccountID;
                return GetProfileAccount(ShipmentTypeCode.UpsWorldShip, accountID);
            }
        }
    }
}
