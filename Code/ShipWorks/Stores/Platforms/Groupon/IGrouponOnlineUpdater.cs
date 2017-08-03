using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// Uploads shipment details to Groupon
    /// </summary>
    public interface IGrouponOnlineUpdater
    {
        /// <summary>
        /// Push the shipment details to the store.
        /// </summary>
        Task UpdateShipmentDetails(IGrouponStoreEntity store, IEnumerable<long> orderKeys);

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        Task UpdateShipmentDetails(IGrouponStoreEntity store, IOrderEntity order, ShipmentEntity shipment);
    }
}