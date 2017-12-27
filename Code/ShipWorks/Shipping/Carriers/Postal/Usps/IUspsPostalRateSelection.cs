using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Interface for Postal rate selection that is aware of accounts
    /// </summary>
    public interface IUspsPostalRateSelection
    {
        /// <summary>
        /// Accounts associated with this rate
        /// </summary>
        List<IUspsAccountEntity> Accounts { get; }

        /// <summary>
        /// Is the rate compatible with the specified shipment
        /// </summary>
        bool IsRateFor(IShipmentEntity shipment);
    }
}