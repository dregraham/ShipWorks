using System.Threading.Tasks;
using Interapptive.Shared;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Client for updating an eBay order with shipped/paid status
    /// </summary>
    public interface IEbayOnlineUpdater
    {
        /// <summary>
        /// Leave Feedback
        /// </summary>
        Task LeaveFeedback(IEbayStoreEntity store, long entityID, CommentTypeCodeType feedbackType, string feedback);

        /// <summary>
        /// Sends a message to the buyer associated with the entityID (shipment or order)
        /// </summary>
        [NDependIgnoreTooManyParams]
        Task SendMessage(IEbayStoreEntity store, long entityID, EbaySendMessageType messageType, string subject, string message, bool copySender);

        /// <summary>
        /// Push the online status for an order.
        /// </summary>
        void UpdateOnlineStatus(IEbayStoreEntity store, long orderID, bool? paid, bool? shipped);

        /// <summary>
        /// Push the online status for an order.
        /// </summary>
        void UpdateOnlineStatus(IEbayStoreEntity store, long orderID, bool? paid, bool? shipped, UnitOfWork2 unitOfWork);
    }
}