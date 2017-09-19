using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    /// <summary>
    /// Uploads shipment information to SparkPay
    /// </summary>
    public interface ISparkPayOnlineUpdater
    {
        /// <summary>
        /// Changes the status of an SparkPay order to that specified
        /// </summary>
        Task UpdateOrderStatus(SparkPayStoreEntity store, long orderID, int statusCode);

        /// <summary>
        /// Changes the status of an SparkPay order to that specified
        /// </summary>
        Task UpdateOrderStatus(SparkPayStoreEntity store, long orderID, int statusCode, UnitOfWork2 unitOfWork);

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        Task UpdateShipmentDetails(ISparkPayStoreEntity store, ShipmentEntity shipment);
    }
}