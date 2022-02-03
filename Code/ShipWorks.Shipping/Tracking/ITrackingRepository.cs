using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Tracking.DTO;

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
        Task MarkAsSent(ShipmentEntity shipment);

        /// <summary>
        /// Save the notification to a shipment
        /// </summary>
        Task SaveNotification(TrackingNotification notification);

        /// <summary>
        /// Fetch a batch of Shipments to track
        /// </summary>
        Task<IEnumerable<ShipmentEntity>> FetchShipmentsToTrack();

        /// <summary>
        /// Gets the latest notification date from all the tracked shipments
        /// </summary>
        Task<DateTime> GetLatestNotificationDate();
    }
}