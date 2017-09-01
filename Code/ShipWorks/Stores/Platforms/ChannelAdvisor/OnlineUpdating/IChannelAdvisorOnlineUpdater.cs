using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.OnlineUpdating
{
    /// <summary>
    /// Posts shipping information
    /// </summary>
    public interface IChannelAdvisorOnlineUpdater
    {
        /// <summary>
        /// Upload the tracking number of the shipment
        /// </summary>
        Task UploadTrackingNumber(IChannelAdvisorStoreEntity store, long shipmentID);

        /// <summary>
        /// Upload the tracking number of the shipment
        /// </summary>
        Task UploadTrackingNumber(IChannelAdvisorStoreEntity store, ShipmentEntity shipment);
    }
}