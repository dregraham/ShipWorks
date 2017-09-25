using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.OnlineUpdating
{
    /// <summary>
    /// Status updater for NetworkSolutions orders
    /// </summary>
    public interface IOrderStatusUpdater
    {
        /// <summary>
        /// Changes the status of an NetworkSolutions order to that specified
        /// </summary>
        Task UpdateOrderStatus(NetworkSolutionsStoreEntity store, long orderID, long statusCode, string comments);

        /// <summary>
        /// Changes the status of an NetworkSolutions order to that specified
        /// </summary>
        Task UpdateOrderStatus(NetworkSolutionsStoreEntity store, long orderID, long statusCode, string comments, UnitOfWork2 unitOfWork);
    }
}