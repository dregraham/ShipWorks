using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Walmart.OnlineUpdating
{
    /// <summary>
    /// Online updater for Walmart
    /// </summary>
    public interface IShipmentDetailsUpdater
    {
        /// <summary>
        /// Upload carrier and tracking information for the given orders
        /// </summary>
        Task UpdateShipmentDetails(IWalmartStoreEntity store, long orderID);

        /// <summary>
        /// Upload carrier and tracking information for the given shipment
        /// </summary>
        /// <remarks>
        /// Only uploads if there is at least one line that has an OnlineStatus = Acknowledged.
        /// If Walmart returns an error, we download the order again, save it and try again if
        /// there is still an acknowledged line.
        /// </remarks>
        Task UpdateShipmentDetails(IWalmartStoreEntity store, ShipmentEntity shipment);
    }
}