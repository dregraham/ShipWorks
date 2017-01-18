using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Updates Magento Shipment Details
    /// </summary>
    public interface IMagentoOnlineUpdater
    {
        /// <summary>
        /// Uploads shipment details to Magento
        /// </summary>
        void UploadShipmentDetails(long orderID, MagentoUploadCommand command, string comments, bool emailCustomer);

        /// <summary>
        /// Uploads shipment details to Magento
        /// </summary>
        void UploadShipmentDetails(long orderID, MagentoUploadCommand command, string comments, bool emailCustomer, UnitOfWork2 unitOfWork);
    }
}