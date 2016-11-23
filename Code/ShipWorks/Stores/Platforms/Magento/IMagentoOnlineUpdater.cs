using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Stores.Platforms.Magento
{
    public interface IMagentoOnlineUpdater
    {
        /// <summary>
        /// Executes an action on the specified order
        /// </summary>
        void UploadShipmentDetails(long orderID, string action, string comments, bool emailCustomer);

        /// <summary>
        /// Executes an action on the specified order
        /// </summary>
        void UploadShipmentDetails(long orderID, string action, string comments, bool emailCustomer, UnitOfWork2 unitOfWork);
    }
}