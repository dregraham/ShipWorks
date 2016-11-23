using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Stores.Platforms.Magento
{
    public interface IMagentoOnlineUpdater
    {
        /// <summary>
        /// Uploads shipment details to Magento
        /// </summary>
        void UploadShipmentDetails(long orderID, string action, string comments, bool emailCustomer);

        /// <summary>
        /// Uploads shipment details to Magento
        /// </summary>
        void UploadShipmentDetails(long orderID, string action, string comments, bool emailCustomer, UnitOfWork2 unitOfWork);
    }
}