using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.BestRate;

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
                long? accountID = ShipmentTypeManager.GetType(ShipmentTypeCode.UpsWorldShip).GetPrimaryProfile().Ups.UpsAccountID;
                return GetProfileAccount(ShipmentTypeCode.UpsWorldShip, accountID);
            }
        }
    }
}
