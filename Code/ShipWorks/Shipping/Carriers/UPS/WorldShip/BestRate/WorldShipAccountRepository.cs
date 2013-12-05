using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.BestRate;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate
{
    /// <summary>
    /// Implementation of ICarrierAccountRepository for WorldShip.  It will just default down to UpsAccountRepository.
    /// </summary>
    public class WorldShipAccountRepository : UpsAccountRepository
    {
    }
}
