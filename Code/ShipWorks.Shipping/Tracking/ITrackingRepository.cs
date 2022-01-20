using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Tracking
{
    /// <summary>
    /// Interface for the tracking repository
    /// </summary>
    public interface ITrackingRepository
    {
        /// <summary>
        /// Marks the shipment with a status of AwaitingResponse
        /// </summary>
        void MarkAsSent(ShipmentEntity shipment);

        /// <summary>
        /// Save the notification to a shipment
        /// </summary>
        void SaveNotification(TrackingNotification notification);
    }
}